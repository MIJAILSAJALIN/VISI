

/*                                            CLASE DE PRUEBAS PARA INYECCIÓN DE DEPENDENCIA    */
//******************************************************************************************************
using System.Security;

namespace VISI.Models
{
    public interface UserValid
    {
        static UserValid Instance { get; set; }
        static bool Validado { get; set; }
        string usuario { get; set; }
        List<UserActual> ListUserValid { get; set; }

        UserActual isValid(userFormValid userFormValid);
    }
    
    public class UsuarioValidado : UserValid
    {
        public static UserValid Instance { get; set; }
        //[ThreadStatic]
        public static bool Validado { get; set; } = false;
        public string usuario { get; set; }
        public List<UserActual> ListUserValid { get; set; } = new List<UserActual>();
        public UserActual isValid(userFormValid userFormValid)
        {
            usuario = userFormValid.username;
            if (userFormValid.username == "a" && userFormValid.password == "b")
            {
                Validado = true;

                var usua = new UserActual(userFormValid.username, true);
                ListUserValid.Add(usua);
                return usua;
            }
            else
            {
                var usua = new UserActual(userFormValid.username, false);
                ListUserValid.Add(usua);
                return usua;
            }
        }
        
    }
    public class userFormValid
    {
        public string? username { get; set; }
        public string? password { get; set; }
        public string? rememberme { get; set; }
    }
    public class UserActual
    {
        public string? UserName { get; set; }
        public bool UserValid { get; set; }
        public UserActual (string name, bool pasa)
        {
            UserName= name;
            UserValid= pasa;
        }
        
    }

}
