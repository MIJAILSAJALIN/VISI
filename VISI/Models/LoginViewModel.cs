using System.ComponentModel.DataAnnotations;

namespace VISI.Models
{
    public class LoginViewModel
    {
        public string User { get; set; }  //estoy usando el email como usuario, este campo lo tengo sin uso
        [Required(ErrorMessage = " El campo '{0}' es requerido.")]
        [EmailAddress(ErrorMessage ="Debe introducir un correo electrónico")]
        public string Email { get; set; }
        [Required(ErrorMessage = " El campo '{0}' es requerido.")]

        public string Password { get; set; }
        [Display(Name = "Recuérdame")]
        public bool Rememberme { get; set; } = true;

    }

}
