using DocumentFormat.OpenXml.EMMA;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace VISI.Models
{
    public class AlbaranesViewModel : AlbaranesConDetalle
    {
        //public AlbaranesConDetalle albaranesConDetalle { get; set; } //= new AlbaranesConDetalle();

        public int numLinea { get; set; } = 0;
        public IEnumerable<SelectListItem> ClientesSelector { get; set; }

    }
    public class ListadoAlbaranesViewModel : PaginacionViewModel                   // heredamos del paginador de resultados
    {
        public IEnumerable<AlbaranesConDetalle> listado { get; set; }
        //[Required(ErrorMessage = "El campo es requerido")]
        [DataType(DataType.DateTime)]
        public DateTime desdeFecha { get; set; } = DateTime.Now.AddMonths(-1);
        //= DateOnly.Parse(DateTime.Today.ToString("dd-MM-yy"));        
        [DataType(DataType.DateTime)]
        public DateTime hastaFecha { get; set; } = DateTime.Now;
        ///aquí estarán los filtros de la página index de albaranes
        public int ClienteId { get; set; }
        public IEnumerable<SelectListItem> ClientesSelector { get; set; }



    }
   

}


