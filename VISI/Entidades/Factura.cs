using System.ComponentModel.DataAnnotations;

namespace VISI.Entidades
{
    public class Factura
    {
        public uint Id { get; set; }
        public uint Numero { get; set; }
        //public virtual DateTime Fecha { get; set; }
        public DateTime Fecha { get; set; } = DateTime.Now;
        public uint ClienteId { get; set; }
        [StringLength(50)]
        [Required]
        public string FormaDePago { get; set; }
        public uint AdministradorId { get; set; }
        public bool Pagada { get; set; }
        public float TipoDeIva { get; set; }
        public decimal Subtotal { get; set; }
        public List<LineasFactura> LineasFacturas { get; set; }
        public List<ArchivoAdjuntoFactura> ArchivoAdjuntoFactura { get; set; }
    }

}
