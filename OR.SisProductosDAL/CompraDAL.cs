using Microsoft.EntityFrameworkCore;
using OR.SisProductos.EN.Filtros;
using OR.SisProductosEN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OR.SisProductosDAL
{
    public class CompraDAL
    {
        readonly ORSysProductosDbContext dbContext;

        public CompraDAL(ORSysProductosDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<int> CrearAsync (CompraEN pCompra)
        {
            // Agregar la compra con sus detalles
            dbContext.Compras.Add(pCompra);
            int result = await dbContext.SaveChangesAsync();
            if (result > 0)
            {
                // Actualizar stock de productos
                foreach (var detalle in pCompra.DetalleCompras)
                {
                    var producto = await dbContext.Productos.FirstOrDefaultAsync(p => p.Id == detalle.IdProducto);
                    if (producto != null)
                    {
                        producto.CantidadDisponible += detalle.Cantidad;
                    }
                }
            }
            return await dbContext.SaveChangesAsync();
        }
        public async Task<int> AnularAsync(int idCompra)
        {
            var compra = await dbContext.Compras
            .Include(c => c.DetalleCompras)
            .FirstOrDefaultAsync(c => c.Id == idCompra);

            if (compra != null && compra.Estado != (byte)CompraEN.EnumEstadoCompra.Anulada)
            {
                // Marcar la compra como anulada
                compra.Estado = (byte)CompraEN.EnumEstadoCompra.Anulada;

                // Restar la cantidad de los productos comprados
                foreach (var detalle in compra.DetalleCompras)
                {
                    var producto = await dbContext.Productos.FirstOrDefaultAsync(p => p.Id == detalle.IdProducto);
                    if (producto != null)
                    {
                        producto.CantidadDisponible -= detalle.Cantidad;
                    }
                }
                return await dbContext.SaveChangesAsync();
            }
            return 0; // Si ya estaba anulada, no hacer nada
        }
        public async Task<CompraEN> ObtenerPorIdAsync(int idCompra)
        {
            var compra = await dbContext.Compras
                .Include(c => c.DetalleCompras).Include(c => c.Proveedor)
                    .FirstOrDefaultAsync(c => c.Id == idCompra);

            return compra ?? new CompraEN();
        }
        public async Task<List<CompraEN>> ObtenerTodosAsync()
        {
            var compras = await dbContext.Compras
               .Include(c => c.DetalleCompras)
               .Include(c => c.Proveedor).ToListAsync();
            return compras ?? new List<CompraEN>();
        }
        public async Task<List<CompraEN>> ObtenerPorEstadoAsync(byte estado)
        {
            var comprasQuery = dbContext.Compras.AsQueryable();

            if (estado != 0)
            {
                comprasQuery = comprasQuery.Where(c => c.Estado == estado);
            }
            comprasQuery = comprasQuery
                .Include(c => c.DetalleCompras)
                .Include(c => c.Proveedor);

            var compras = await comprasQuery.ToListAsync();

            return compras ?? new List<CompraEN>();
        }
        public async Task<List<CompraEN>> ObtenerReporteComprasAsync(CompraFiltros filtros)
        {
            var comprasQuery = dbContext.Compras
                .Include(c => c.DetalleCompras)
                    .ThenInclude(dc => dc.Producto)
                .Include(c => c.Proveedor)
                .AsQueryable();

            if (filtros.FechaInicio.HasValue)
            {
                DateTime FechaInicio = filtros.FechaInicio.Value.Date; // Eliminar la hora, dejar solo la fecha  
                comprasQuery = comprasQuery.Where(c => c.FechaCompra >= FechaInicio);
            }

            if (filtros.FechaFin.HasValue)
            {
                DateTime FechaFin = filtros
                    .FechaFin.Value.Date.AddDays(1).AddSeconds(-1); // Incluir hasta el final del día  
                comprasQuery = comprasQuery.Where(c => c.FechaCompra <= FechaFin);
            }

            return await comprasQuery.ToListAsync();
        }
    }
}
