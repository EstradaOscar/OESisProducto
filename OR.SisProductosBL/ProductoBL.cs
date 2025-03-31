using OR.SisProductosDAL;
using OR.SisProductosEN;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OR.SisProductosBL
{
    public class ProductoBL
    {
        private readonly ProductoDAL _productoDAL;

        public ProductoBL(ProductoDAL productoDAL)
        {
            _productoDAL = productoDAL;
        }

        public async Task<int> CrearAsync(ProductoEN pProducto)
        {
            return await _productoDAL.CrearAsync(pProducto);
        }

        public async Task<int> ModificarAsync(ProductoEN pProducto)
        {
            return await _productoDAL.ModificarAsync(pProducto);
        }

        public async Task<int> EliminarAsync(int productoId)
        {
            return await _productoDAL.EliminarAsync(productoId);
        }

        public async Task<ProductoEN> ObtenerPorIdAsync(int productoId)
        {
            return await _productoDAL.ObtenerPorIdAsync(productoId);
        }

        public async Task<List<ProductoEN>> ObtenerTodosAsync()
        {
            return await _productoDAL.ObtenerTodosAsync();
        }
        public Task AgregarTodosAsync(List<ProductoEN> productos)
        {

            return _productoDAL.AgregarTodosAsync(productos);
        }
    }
}