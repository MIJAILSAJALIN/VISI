using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Resources;

namespace VISI.Models
{
    public class Albaranes
    {

        [Required(ErrorMessage = "El campo es requerido")]
        public int id { get; set; }
        [Required(ErrorMessage = " El campo '{0}' es requerido.")]
        [Display(Name = "Número de Albarán")]
        public int Numero { get; set; }
        [Required(ErrorMessage = "El campo es requerido")]
        [DataType(DataType.Date)]
        //public DateTime Fecha { get; set; } = DateTime.Parse(DateTime.Now.ToString("dd-MM-yy"));
        public DateTime Fecha { get; set; } = DateTime.Now;
        [Required(ErrorMessage = "El campo es requerido")]
        [Display(Name = "Cliente")]
        public int ClienteId { get; set; }
        
        public string ClienteNombre { get; set; }
        public bool SeImprime { get; set; }
        [Display(Name = "Nº de factura")]

        public int NumFactura { get; set; }
        public decimal TipoIva { get; set; }
        public decimal BaseImponible { get; set; }
    }
    public class Albaranes_detalle
    {
        [Required(ErrorMessage = "El campo es requerido")]
        public int Id { get; set; }
        [Required(ErrorMessage = "El campo es requerido")]
        public int AlbaranNum { get; set; }
        [Required(ErrorMessage = "El campo es requerido")]
        public int LineaNum { get; set; }
        [StringLength(maximumLength: 100)]
        public string Descripcion { get; set; }
        public decimal Cantidad { get; set; }
        [Range(-1000000, 1000000)]
        public decimal Precio { get; set; }
    }
    public class AlbaranesConDetalle : Albaranes
    {
        public List<Albaranes_detalle> Detalles { get; set; } 
             =  new List<Albaranes_detalle> { new Albaranes_detalle() { LineaNum = 1 } };



        public new decimal BaseImponible => Detalles.Sum(x => (x != null ? x.Precio * x.Cantidad : 0));
        


        //public IEnumerable<SelectListItem> ClientesSelector { get; set; }
        
    }
    

}
