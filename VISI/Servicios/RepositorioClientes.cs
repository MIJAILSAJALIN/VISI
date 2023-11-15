using Dapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using System.Reflection.Metadata.Ecma335;
using VISI.Entidades;
using VISI.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace VISI.Servicios
{
    public interface IRepositorioClientes
    {
        Task Actualizar(Clientes clientes);
        Task<Clientes> Busca(int id);
        Task Crear(Clientes clientes);
        Task Delete(int id);
        Task<bool> Distintode(string nombre, string nif, int id);
        Task<IEnumerable<Clientes>> FiltraClientes(string embudo);
       
        Task<IEnumerable<Clientes>> ListaClientes();
        Task<IEnumerable<SelectListItem>> RellenaSelectorClientes(bool addseleccionClientes = true);
        Task<bool> Yaexiste(string nombre, string nif);
    }
    public class RepositorioClientes : IRepositorioClientes
    {
        private readonly string connectionString;
        public RepositorioClientes(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public async Task Crear(Clientes clientes)
        {

            using (SqlConnection conexion = new(connectionString))
            {
                var id = await conexion.QuerySingleAsync<int>(@"INSERT INTO VISI_clientes (nombre, nif, direccion, 
                            telefono, cp, iban, formapago, administradorid, contacto,email)
                            values (@Nombre, @Nif, @Direccion, @Telefono, @Cp, @Iban, @Formapago,@Administradorid,
                            @Contacto,@Email);
                            SELECT SCOPE_IDENTITY();", clientes);
                clientes.Id = id;
            }
        }

        public async Task<bool> Yaexiste(string nombre, string nif)
        {
            using (SqlConnection miConex = new(connectionString))
            {
                string sql;
                sql = "select 1 from visi_clientes where @nif = nif OR @nombre = nombre";
                
                bool existe = await miConex.QueryFirstOrDefaultAsync<bool>(sql, new { nombre,nif });
                return existe;
            }
        }
        public async Task<bool> Distintode(string nombre, string nif, int id)
        {
            using (SqlConnection miConex = new(connectionString))
            {
                string sql = "select 1  from visi_clientes where (nif = @nif OR nombre = @nombre) and id != @id";
                bool existe = await miConex.QueryFirstOrDefaultAsync<bool>(sql, new { nombre, nif, id});
                return existe;
            }
        }

        public  async Task<IEnumerable<Clientes>> ListaClientes()
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            return  await connection.QueryAsync<Clientes>(@"select * from VISI_clientes  ");
            
        }

        public async Task Actualizar(Clientes clientes)
        {
            using (SqlConnection miConex = new(connectionString))
            {
                await miConex.ExecuteAsync("UPDATE VISI_CLIENTES SET nombre = @Nombre, nif = @Nif, " +
                    "direccion = @Direccion, telefono = @Telefono, cp = @Cp, iban = @Iban,formapago = @Formapago, " +
                    "administradorId = @AdministradorId, contacto = @Contacto, email = @Email WHERE Id = @Id",
                    clientes);
            }
        }
        public async Task<Clientes> Busca(int id)
        {
            using (SqlConnection miConex = new(connectionString))
            {

                //SELECT * FROM VISI_clientes where id = 1
                //var aa =  await miConex.QueryFirstOrDefaultAsync<Clientes>(@"SELECT * FROM dbo.VISI_Clientes WHERE 
                //                                                    Id = 1",
                //                                                    new { id });
                return await miConex.QueryFirstOrDefaultAsync<Clientes>(@"SELECT * FROM VISI_Clientes WHERE 
                                                                    Id = @Id",
                                                                    new { id });
            }
        }
        public async Task Delete(int id)
        {
            using (SqlConnection miConex = new(connectionString))
            {
                await miConex.ExecuteAsync("DELETE VISI_clientes WHERE Id = @Id", new { id });
            }
        }
        public async Task<IEnumerable<Clientes>> FiltraClientes(string embudo)
        {
            embudo = $"%{embudo}%";
            using (SqlConnection miConex = new(connectionString))
            {
                var sql = @" select * from VISI_clientes where nombre like @embudo or nif like @embudo or direccion like @embudo 
                        or CAST(telefono as varchar(10)) like @embudo or CAST(cp as varchar(5)) like @embudo or
                        iban like @embudo or formapago like @embudo or contacto like @embudo or email like @embudo ";

                return await miConex.QueryAsync<Clientes>(sql, new { embudo });
            }
        }
        public  async Task<IEnumerable<SelectListItem>> RellenaSelectorClientes(bool addseleccionClientes = true)
        {
            var listacliente = (await ListaClientes()).
                                               Select(x => new SelectListItem(x.Nif + " - " + x.Nombre , x.Id.ToString())).ToList();
            //var xdefecto = new SelectListItem("-- Seleccione un cliente --", "0", true);
            //listacliente.Insert(0, new SelectListItem("-- Seleccione un cliente --", "0", true));
            if (addseleccionClientes)
                listacliente.Insert(0, new SelectListItem("-- Todos los clientes --", "0", true));
            return listacliente;
        }

    }
}
