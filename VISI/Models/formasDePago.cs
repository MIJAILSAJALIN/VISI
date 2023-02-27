using System.ComponentModel.DataAnnotations;

namespace VISI.Models
{
    public class formasDePago
    {
        [StringLength(maximumLength: 30)]
        public string Id { get; set; }
        [StringLength(maximumLength: 50)]
        public string Descripcion { get; set; }
        public int orden { get; set; }
    }
}
