using OR.SisProductosDAL;
using OR.SisProductosEN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OR.SisProductosBL
{
    public class ProveedorBL
    {
        readonly ProveedorDAL proveedorDAL;

        public ProveedorBL(ProveedorDAL proveedorDAL)
        {
            this.proveedorDAL = proveedorDAL;
        }

        public async Task<int> CrearAsync(ProveedorEN pProveedor)
        {
            return await proveedorDAL.CrearAsync(pProveedor);
        }

        public async Task<int> ModificarAsync(ProveedorEN pProveedor)
        {
            return await proveedorDAL.ModificarAsync(pProveedor);
        }

        public async Task<int> EliminarAsync(ProveedorEN pProveedor)
        {
            return await proveedorDAL.EliminarAsync(pProveedor);
        }

        public async Task<ProveedorEN> ObtenerPorIdAsync(ProveedorEN pProveedor)
        {
            return await proveedorDAL.ObtenerPorIdAsync(pProveedor);
        }

        public async Task<List<ProveedorEN>> ObtenerTodoAsync()
        {
            return await proveedorDAL.ObtenerTodosAsync();
        }

        // Método para agregar múltiples proveedores  
        public Task AgregarTodosAsync(List<ProveedorEN> productos)
        {

            return proveedorDAL.AgregarTodosAsync(productos);
        }

    }
}  

