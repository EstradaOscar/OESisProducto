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
        public DbSet<ProductoEN> Productos { get; set; }
        public DbSet<ProveedorEN> Proveedores { get; set; }
        public DbSet<CompraEN> Compras { get; set; }
        public DbSet<DetalleCompra> DetalleCompras { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Venta> Ventas { get; set; }
        public DbSet<DetalleVenta> DetalleVentas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DetalleCompra>()
                .HasOne(d => d.Compra)
                .WithMany(c => c.DetalleCompras)
                .HasForeignKey(d => d.IdCompra);

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<DetalleVenta>()
           .HasOne(d => d.Venta)
           .WithMany(c => c.DetalleVentas)
           .HasForeignKey(d => d.IdVenta);

            base.OnModelCreating(modelBuilder);
        }
    }
}
