using DocumentFormat.OpenXml.Office2013.Excel;
using System.ComponentModel.DataAnnotations;
using VISI.Migrations;

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
        public string FormaDePago { get; set; }
        public uint AdministradorId { get; set; }
        public bool Pagada { get; set; }
        public float TipoDeIva { get; set; }
        public decimal Subtotal { get; set; } 
        //private decimal miSubtotal  => LineasFacturas.Sum(x => (x != null ? x.Precio * x.Cantidad : 0));
        //si intento calcular el resultado no se me graba en la base de datos, será cosa de Entity...
        public List<LineasFactura> LineasFacturas { get; set; }
        public List<ArchivoAdjuntoFactura> ArchivoAdjuntoFactura { get; set; }
    }
    

}
