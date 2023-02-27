using System.ComponentModel.DataAnnotations;

namespace VISI.Models
{
    public class RegistroViewModel
    {
        public string User { get; set; }  //estoy usando el email como usuario, este campo lo tengo sin uso
        [Required(ErrorMessage = " El campo '{0}' es requerido.")]

        public string Email { get; set; }
        [Required(ErrorMessage = " El campo '{0}' es requerido.")]

        public string Password { get; set; }
        public bool Rememberme { get; set; }
        

    }

}
