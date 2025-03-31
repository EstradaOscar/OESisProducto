using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using OR.SisProductosBL; // Asegúrate de tener la referencia correcta  
using OR.SisProductosEN; // Asegúrate de tener la referencia correcta  
using Rotativa.AspNetCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OR.SisProductos.Controllers
{
    public class ProductoController : Controller
    {
        private readonly ProductoBL _productoBL;

        // Constructor que inyecta el servicio ProductoBL  
        public ProductoController(ProductoBL productoBL)
        {
            _productoBL = productoBL;
        }

        // GET: Producto  
        public async Task<IActionResult> Index()
        {
            List<ProductoEN> productos = await _productoBL.ObtenerTodosAsync();
            return View(productos); // Devuelve la lista de productos a la vista  
        }

        // GET: Producto/Details/5  
        public async Task<IActionResult> Details(int id)
        {
            ProductoEN producto = await _productoBL.ObtenerPorIdAsync(id);
            if (producto == null)
            {
                return NotFound(); // Devuelve un 404 si no se encuentra el producto  
            }
            return View(producto); // Devuelve el producto a la vista de detalles  
        }

        // GET: Producto/Create  
        public IActionResult Create()
        {
            return View(); // Devuelve la vista de creación  
        }

        // POST: Producto/Create  
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductoEN producto)
        {
            if (ModelState.IsValid)
            {
                await _productoBL.CrearAsync(producto);
                return RedirectToAction(nameof(Index)); // Redirige a la lista de productos  
            }
            return View(producto); // Si hay errores, vuelve a mostrar la vista  
        }

        // GET: Producto/Edit/5  
        public async Task<IActionResult> Edit(int id)
        {
            ProductoEN producto = await _productoBL.ObtenerPorIdAsync(id);
            if (producto == null)
            {
                return NotFound();
            }
            return View(producto); // Devuelve el producto a la vista de edición  
        }

        // POST: Producto/Edit/5  
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductoEN producto)
        {
            if (id != producto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _productoBL.ModificarAsync(producto);
                return RedirectToAction(nameof(Index)); // Redirige a la lista de productos  
            }
            return View(producto); // Si hay errores, vuelve a mostrar la vista  
        }

        // GET: Producto/Delete/5  
        public async Task<IActionResult> Delete(int id)
        {
            ProductoEN producto = await _productoBL.ObtenerPorIdAsync(id);
            if (producto == null)
            {
                return NotFound(); // Devuelve un 404 si no se encuentra el producto  
            }
            return View(producto); // Devuelve el producto a la vista de eliminación  
        }

        // POST: Producto/Delete/5  
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _productoBL.EliminarAsync(id);
            return RedirectToAction(nameof(Index)); // Redirige a la lista de productos  
        }

        public async Task<ActionResult> ReporteProductos()
        {
            var productos = await _productoBL.ObtenerTodosAsync();

            return new ViewAsPdf("ReporteProductos", productos);
        }
        public async Task<JsonResult> Productosjson()
        {
            var productos = await _productoBL.ObtenerTodosAsync();
            var productosData = productos
                .Select(p => new
                {
                    nombre = p.Nombre,
                    stock = p.CantidadDisponible,
                })
            .ToList();

            return Json(productosData);
        }
        public async Task<JsonResult> ProductosJsonPrecio()
        {
            var productos = await _productoBL.ObtenerTodosAsync();

            var productosData = productos
                .Select(p => new
                {
                    fechaCreacion = p.FechaCreacion.ToString("yyyy-MM-dd"),
                    precio = p.Precio
                })
                .ToList();

            var groupedData = productosData
                .GroupBy(p => p.fechaCreacion)
                .Select(g => new
                {
                    fecha = g.Key,
                    precioPromedio = g.Average(p => p.precio) // Calcular el precio promedio  
                })
                .OrderBy(g => g.fecha)
                .ToList();

            return Json(groupedData);
        }

        public async Task<IActionResult> ReporteProductosExcel()
        {
            var productos = await _productoBL.ObtenerTodosAsync();
            using (var package = new ExcelPackage())
            {
                var hojasExcel = package.Workbook.Worksheets.Add("Productos");
                hojasExcel.Cells["A1"].Value = "Nombre";
                hojasExcel.Cells["B1"].Value = "Precio";
                hojasExcel.Cells["C1"].Value = "Cantidad";
                hojasExcel.Cells["D1"].Value = "Fecha";

                int row = 2;
                foreach (var producto in productos)
                {
                    hojasExcel.Cells[row, 1].Value = producto.Nombre;
                    hojasExcel.Cells[row, 2].Value = producto.Precio;
                    hojasExcel.Cells[row, 3].Value = producto.CantidadDisponible;
                    hojasExcel.Cells[row, 4].Value = producto.FechaCreacion.ToString("yyyy-MM-dd");
                    row++;
                }

                hojasExcel.Cells["A:D"].AutoFitColumns();

                var stream = new MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;


                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ReporteProductosExcel.xlsx");
            }
        }
        public async Task<IActionResult> SubirExcelProductos(IFormFile archivoExcel)
        {
            if (archivoExcel == null || archivoExcel.Length == 0)
            {
                return RedirectToAction("Index");
            }

            var productos = new List<ProductoEN>();

            using (var stream = new MemoryStream())
            {
                await archivoExcel.CopyToAsync(stream);
                using (var package = new ExcelPackage(stream))
                {
                    var hojaExcel = package.Workbook.Worksheets[0];
                    int rowCount = hojaExcel.Dimension.Rows;

                    for (int row = 2; row <= rowCount; row++)
                    {
                        var nombre = hojaExcel.Cells[row, 1].Text;
                        var precio = hojaExcel.Cells[row, 2].GetValue<decimal>();
                        var cantidad = hojaExcel.Cells[row, 3].GetValue<int>();
                        var fecha = hojaExcel.Cells[row, 4].GetValue<DateTime>();

                        // Validar campos  
                        if (string.IsNullOrEmpty(nombre) || precio <= 0 || cantidad <= 0)
                            continue;

                        productos.Add(new ProductoEN
                        {
                            Nombre = nombre,
                            Precio = precio,
                            CantidadDisponible = cantidad,
                            FechaCreacion = fecha,
                        });
                    }
                }
            }

            if (productos.Count > 0)
            {
                await _productoBL.AgregarTodosAsync(productos);
            }
            return RedirectToAction("Index");
        }
    }
}
