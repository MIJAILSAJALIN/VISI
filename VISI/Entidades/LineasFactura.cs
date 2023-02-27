using System.ComponentModel.DataAnnotations;

namespace VISI.Entidades
{
    public class LineasFactura
    {
        public uint Id { get; set; }
        public uint FacturaId { get; set; }
        public Factura Factura { get; set; }  //esto es una especie de INNER JOIN...Se llama 'propiedad de navegación'
        public uint LineaNumero { get; set; }
        [StringLength(250)]
        [Required]
        public string Descripcion { get; set; }
        public int Cantidad { get; set; }
        public int Precio { get; set; }


    }
}
