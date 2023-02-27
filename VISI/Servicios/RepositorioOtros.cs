using Dapper;
using Microsoft.Data.SqlClient;
using VISI.Models;

namespace VISI.Servicios
{
    public interface IRepositorioOtros
    {
        Task Alta(string id, string descrip);
        Task BorraId(formasDePago forma);
        Task<bool> Existe(string id, string descrip);
        Task<formasDePago> FindId(string id);
        Task<IEnumerable<formasDePago>> ListaFpg();
        Task Ordenar(IEnumerable<formasDePago> ordenadas);
        Task update(string id, string descrip);
    }
    public class RepositorioOtros : IRepositorioOtros
    {
        private readonly string connectionString;
        public RepositorioOtros(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public async Task<IEnumerable<formasDePago>> ListaFpg()
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<formasDePago>(@"select * from VISI_formasDePago order by orden");
        }
        public async Task<bool> Existe(string id, string descrip)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<bool>(@"SELECT 1 from VISI_formasDePago where id = @id
                                                                    or Descripcion = @descrip", new { id, descrip });
        }
        public async Task Alta(string id, string descrip)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            
            await connection.ExecuteAsync(@"INSERT INTO VISI_formasDePago (Id, Descripcion, orden) values 
                                        (@id,@descrip,(select coalesce(max(orden),0)+1 from VISI_formasDePago))", 
                                                new {id,descrip}); //SELECT coalesce es para obtener el último valor del campo
            // hay que meter los try-catch por algún sitio...
        }
        public async Task<formasDePago> FindId(string id)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<formasDePago>(@"SELECT * from VISI_formasDePago where id = @id",
                                                                            new { id });
        }
        public async Task BorraId(formasDePago f)
        {
            using SqlConnection connection = new SqlConnection(connectionString);            
            await connection.ExecuteAsync("delete from VISI_formasDePago where id = @id and Descripcion = @Descripcion",
                                                        new { f.Id, f.Descripcion });
        }
        public async Task Ordenar(IEnumerable<formasDePago> ordenadas )
        {
            var query = "update VISI_formasDePago set orden=@orden where id=@Id";
            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(query, ordenadas);
        }
        public async Task update(string id, string descrip)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            var query = "update VISI_formasDePago set Descripcion = @descrip where id=@id";
            await connection.ExecuteAsync(query, new {id, descrip});
        }

    }
}
