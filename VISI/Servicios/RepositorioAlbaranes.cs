using Dapper;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using VISI.Models;
using static ClosedXML.Excel.XLPredefinedFormat;
using static System.Runtime.InteropServices.JavaScript.JSType;







namespace VISI.Servicios
{
    public interface IRepositorioAlbaranes
    {
        Task eliminar_detalle(int numAlb, int idLinea);
        Task grabar_nuevo(AlbaranesConDetalle albaranConDetalle);       
        Task<(IEnumerable<AlbaranesConDetalle>, int)> listado(dynamic clienteId = null, dynamic desde = null, dynamic hasta = null, PaginacionViewModel paginacion = null);
    }
    public class RepositorioAlbaranes : IRepositorioAlbaranes
    {       
        private readonly string connectionString;

        public RepositorioAlbaranes(IConfiguration configuration) 
        {           
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task grabar_nuevo(AlbaranesConDetalle alb)
        {
            var conexion = new SqlConnection(connectionString);
            int NumeroAlbaran = await conexion.QueryFirstOrDefaultAsync<int>("select coalesce(max(VISI_albaranes.Numero),0)+1 from VISI_albaranes");
            alb.Numero = NumeroAlbaran;
            
            await conexion.ExecuteAsync(@"INSERT INTO VISI_albaranes (Numero, Fecha, ClienteId, BaseImponible) 
                    VALUES (@Numero, @Fecha, @ClienteId, @BaseImponible)", alb);

            alb.Detalles.RemoveAll(d => (string.IsNullOrWhiteSpace(d.Descripcion))&&
                    (string.IsNullOrEmpty(d.Descripcion) && (d.Cantidad ==0) && (d.Precio==0) )) ;
                            
            alb.Detalles.ForEach(x => x.AlbaranNum = NumeroAlbaran);

            await conexion.ExecuteAsync(@"insert into VISI_Albaranes_detalle (AlbaranNum, LineaNum, Descripcion, 
                    Cantidad, Precio) VALUES (@AlbaranNum, @LineaNum, 
                    @Descripcion,@Cantidad,@Precio)",alb.Detalles);
            
        }
        public async Task eliminar_detalle(int numAlb, int idLinea)
        {
            var conexion = new SqlConnection(connectionString);
            await conexion.ExecuteAsync(@"DELETE FROM VISI_Albaranes_detalle WHERE Albaran_num = @numAlb AND
                                        LineaNum = @idLinea",new {numAlb,idLinea});

        }
        public async Task<(IEnumerable<AlbaranesConDetalle>,int)> listado(dynamic clienteId = null, dynamic desde = null , dynamic hasta = null,
                                                                PaginacionViewModel paginacion = null)
        {
            List<AlbaranesConDetalle> milista = new List<AlbaranesConDetalle>();
            List<Albaranes_detalle> temporal2 = new List<Albaranes_detalle>();


            string miQuery = "select *, nombre as ClienteNombre from VISI_albaranes INNER JOIN VISI_clientes ON ClienteId = VISI_clientes.Id";
            desde = Convert.ToDateTime(desde).ToString("yyyyMMdd");
            hasta = hasta.ToString("yyyyMMdd");
            int totalRegistros = 0;
            var conexion = new SqlConnection(connectionString);   
            if ( clienteId is -1 )
            {   
                
                miQuery += $" where Fecha between '{desde}' and '{hasta}'";
            }            
            else if (clienteId is not null )
            {
                miQuery += $" where ClienteId={clienteId} and Fecha between '{desde}' and '{hasta}'";
            }
                        
            if (paginacion != null)
            {
                string queryContar = miQuery.Replace("*, nombre as ClienteNombre", "count(*)");
                totalRegistros = await conexion.ExecuteScalarAsync<int>(queryContar);     //contamos el total de registros sin paginar...
                miQuery += $" ORDER BY numero OFFSET {paginacion.RegistrosASaltar} ROWS FETCH NEXT {paginacion.RegistrosPorPagina} ROWS ONLY";
            }

            milista = (List<AlbaranesConDetalle>)await conexion.QueryAsync<AlbaranesConDetalle>(miQuery);
    
            temporal2 = (List<Albaranes_detalle>)await conexion.QueryAsync<Albaranes_detalle>
                                                    (@"select * from VISI_albaranes_detalle"); //esto es muy lento, hay que filtrar la búsqueda



            milista.ForEach( x =>
            {
                x.Detalles.Clear();
                temporal2.Where(y => y.AlbaranNum == x.Numero).ToList().ForEach(z =>
                {
                    x.Detalles.Add(z);
                });     //elimina los registros asignados para agilizar el proceso
                temporal2.RemoveAll(y => y.AlbaranNum == x.Numero); 
                
            }
                );        
            return (milista, totalRegistros) ;
        }
        //public async Task<int> Contar() //no tiene sentido contar a secas, hay que obtener el resultado filtrado...
        //{
        //    //using var connection = new SqlConnection(connectionString);
        //    //return await connection.ExecuteScalarAsync<int>("select count(*) from VISI_albaranes");

        //}
        

    }
}
