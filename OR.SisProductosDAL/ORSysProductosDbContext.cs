using Microsoft.EntityFrameworkCore;
using OR.SisProductosEN;

namespace OR.SisProductosDAL
{
    public class ORSysProductosDbContext : DbContext
    {
        public ORSysProductosDbContext(DbContextOptions<ORSysProductosDbContext> options)
            : base(options)
        {

        }

        public DbSet<ProductoEN> Productos { get; set; } // DbSet para gestionar entidade 
        public DbSet<ProveedorEN> Proveedores { get; set; } // DbSet para gestionar entidades ProveedorEN
        public DbSet<CompraEN> Compras { get; set; } // DbSet para gestionar entidades CompraEN
        public DbSet<DetalleCompra> DetallesCompra { get; set; } // DbSet para gestionar entidades DetalleCompraEN
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DetalleCompra>()
                .HasOne(d => d.Compra)
                .WithMany(c => c.DetalleCompras)
                .HasForeignKey(d => d.IdCompra);

            base.OnModelCreating(modelBuilder);
        }
    }
}