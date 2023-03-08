using Dapper;
using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using System.Security.Claims;
using VISI.Models;

namespace VISI.Servicios
{
    public interface IRepositorioUsuarios
    {
        Task<Usuario> BuscaUsuarioxEmail(string emailNormalizado);
        Task<int> CrearUsuario(Usuario usuario);
        string ObtenerUsuarioId();
    }
    public class RepositorioUsuarios : IRepositorioUsuarios
    {
        private readonly string connectoinString;
        private HttpContext httpContext;

        public RepositorioUsuarios(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            connectoinString = configuration.GetConnectionString("DefaultConnection");
            httpContext = httpContextAccessor.HttpContext; //podría estar en otro servicio
        }

        public async Task<int> CrearUsuario(Usuario usuario)
        {
            using SqlConnection connection = new SqlConnection(connectoinString);
            var id = await connection.QuerySingleAsync<int>(@"INSERT INTO VISI_usuarios(Email, EmailNormalizado, 
                    PasswordHash) VALUES(@Email, @EmailNormalizado, @PasswordHash);
                    select SCOPE_IDENTITY()"
                    ,usuario);
            return id;
        }
        public async Task<Usuario> BuscaUsuarioxEmail(string emailNormalizado)
        {
            using SqlConnection connection = new SqlConnection(connectoinString);
            return await connection.QuerySingleOrDefaultAsync<Usuario>(@"select * from VISI_usuarios 
                                where EmailNormalizado = @emailNormalizado", 
                                new { emailNormalizado });
        }

        public string ObtenerUsuarioId()   //se podría separar en otro servicio para mayor claridad
        {
            if (httpContext.User.Identity.IsAuthenticated)
            {
                var idClaim = httpContext.User.Claims.Where(t => t.Type == ClaimTypes.NameIdentifier).FirstOrDefault();
                
                return (idClaim.Value);
            }
            else
            {
                throw new Exception("El usuario no está registrado...");
            }
        }

    }
}
