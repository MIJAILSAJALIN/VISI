using DocumentFormat.OpenXml.Wordprocessing;
using System.ComponentModel.DataAnnotations;
using VISI.Migrations;


namespace VISI.Entidades
{
    // definida en modelos...

    /*
    public class xxdbClientes //: IValidatableObject        interface para validar a nivel de la clase
    {
        
        public int Id { get; set; }        
        [Required(ErrorMessage = " El campo '{0}' es requerido.")]
        [StringLength(maximumLength: 50, ErrorMessage = "La longitud no puede superar los {1} caracteres.")]
        [Display(Name = "Nombre del cliente")]
        //[Remote(action: "existeCliente", controller: "Clientes")]
        public string Nombre { get; set; }
        [StringLength(maximumLength: 15)]
        //[Remote(action: "existeCliente", controller: "Clientes")]
        public string Nif { get; set; }
        [StringLength(maximumLength: 100)]
        public string Direccion { get; set; }
        [Range(minimum: 0, maximum: 999999999, ErrorMessage = "El teléfono no es correcto.")]
        public int? Telefono { get; set; }
        [Range(minimum: 0, maximum: 99999, ErrorMessage = "El código postal no es correcto.")]
        [Display(Name = "Código Postal")]
        public int? Cp { get; set; }
        [StringLength(maximumLength: 24)]
        public string Iban { get; set; }
        [StringLength(maximumLength: 30)]
        [Display(Name = "Forma de Pago")]
        public string Formapago { get; set; }
        [Display(Name = "Administrador")]
        [Range(minimum: 0, maximum: 999999, ErrorMessage = "El código del administrador no es correcto.")]
        public int? AdministradorId { get; set; }
        [StringLength(maximumLength: 50)]
        public string Contacto { get; set; }
        [EmailAddress(ErrorMessage = "El formato no es válido.")]
        [StringLength(maximumLength: 50)]
        public string Email { get; set; }
        
      
    }
    */
}
