using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OR.SisProductosEN
{
    public class ProductoEN
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public decimal Precio { get; set; }
        public int CantidadDisponible { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}