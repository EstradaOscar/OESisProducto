using OR.SisProductosDAL;
using OR.SisProductosEN;
using OR.SisProductos.EN.Filtros;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OR.SisProductosBL
{
    public class VentaBL
    {
        readonly VentaDAL ventaDAL;
        public VentaBL(VentaDAL pVentaDAL)
        {
            ventaDAL = pVentaDAL;
        }
        public async Task<int> CrearAsync(Venta pVenta)
        {
            return await ventaDAL.CrearAsync(pVenta);
        }
        public async Task<int> AnularAsync(int idVenta)
        {
            return await ventaDAL.AnularAsync(idVenta);
        }
        public async Task<Venta> ObtenerPorIdAsync(int idVenta)
        {
            return await ventaDAL.ObtenerPorIdAsync(idVenta);
        }
        public async Task<List<Venta>> ObtenerTodosAsync()
        {
            return await ventaDAL.ObtenerTodosAsync();
        }
        public async Task<List<Venta>> ObtenerPorEstadoAsync(byte estado)
        {
            return await ventaDAL.ObtenerPorEstadoAsync(estado);
        }
        public async Task<List<Venta>> ObtenerReporteVentasAsync(VentaFiltros filtro)
        {
            return await ventaDAL.ObtenerReporteVentasAsync(filtro);
        }
    }
}
