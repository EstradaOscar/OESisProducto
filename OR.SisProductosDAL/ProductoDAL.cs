using Microsoft.EntityFrameworkCore;
using OR.SisProductosEN;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OR.SisProductosDAL
{
    public class ProductoDAL
    {
        readonly ORSysProductosDbContext _dbContext;

        public ProductoDAL(ORSysProductosDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Método para crear un producto  
        public async Task<int> CrearAsync(ProductoEN pProducto)
        {
            var producto = new ProductoEN // ConvierteProductoEN a la entidad  
            {
                Nombre = pProducto.Nombre,
                Precio = pProducto.Precio,
                CantidadDisponible = pProducto.CantidadDisponible,
                FechaCreacion = DateTime.UtcNow
            };

            _dbContext.Productos.Add(producto); // Agrega el producto al DbContext  
            return await _dbContext.SaveChangesAsync(); // Guarda los cambios  
        }

        // Método para modificar un producto  
        public async Task<int> ModificarAsync(ProductoEN pProducto)
        {
            var producto = await _dbContext.Productos.FirstOrDefaultAsync(p => p.Id == pProducto.Id);
            if (producto != null)
            {
                // Actualiza las propiedades  
                producto.Nombre = pProducto.Nombre;
                producto.Precio = pProducto.Precio;
                producto.CantidadDisponible = pProducto.CantidadDisponible;
                producto.FechaCreacion = pProducto.FechaCreacion;

                // Actualiza el producto en el DbContext  
                _dbContext.Productos.Update(producto);
                return await _dbContext.SaveChangesAsync();
            }
            return 0; // Producto no encontrado  
        }

        // Método para eliminar un producto  
        public async Task<int> EliminarAsync(int productoId)
        {
            var producto = await _dbContext.Productos.FirstOrDefaultAsync(p => p.Id == productoId);
            if (producto != null)
            {
                _dbContext.Productos.Remove(producto);
                return await _dbContext.SaveChangesAsync();
            }
            return 0; // Producto no encontrado  
        }

        // Método para obtener un producto por ID  
        public async Task<ProductoEN> ObtenerPorIdAsync(int id)
        {
            return await _dbContext.Productos.FirstOrDefaultAsync(p => p.Id == id);
        }

        // Método para obtener todos los productos  
        public async Task<List<ProductoEN>> ObtenerTodosAsync()
        {
            return await _dbContext.Productos.ToListAsync();
        }
        public async Task AgregarTodosAsync(List<ProductoEN> productos)
        {
            await _dbContext.Productos.AddRangeAsync(productos);
            await _dbContext.SaveChangesAsync();
        }

        
    }
}