using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OR.SisProductosEN
{
    public class UsuarioEn
    {
        public string UserName { get; set; }

        [Required(ErrorMessage = "El Password es obligatorio")]
        public string Password { get; set; }

       
    }
}