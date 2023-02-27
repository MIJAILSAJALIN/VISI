using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace VISI.Entidades
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
        // otra forma, la más potente, de configurar los campos de la tabla ↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //También se podía hacer con propiedades en la definición de la entidad, más sencillo
            //Para obtener validaciones es mejor utilizar las propiedades en la creación de la entidad.
            //modelBuilder.Entity<Factura>().Property(x => x.FormaDePago).HasMaxLength(50).IsRequired();

        }
        public DbSet<Factura> VISI_Facturas { get; set; }
        public DbSet<LineasFactura> VISI_LineasFactura { get; set; }
        public DbSet<ArchivoAdjuntoFactura> VISI_ArchivosAdjuntos { get; set; }
    }
}
