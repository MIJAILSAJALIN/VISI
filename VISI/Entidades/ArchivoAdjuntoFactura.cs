using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace VISI.Entidades
{
    public class ArchivoAdjuntoFactura
    {
        public Guid Id { get; set; }
        public uint FacturaId { get; set; }
        public Factura Factura { get; set;}
        [Unicode]
        public string URL { get; set; }
        [StringLength(250)]
        public string Titulo { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}
