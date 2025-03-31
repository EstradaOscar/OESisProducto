using OR.SisProductosEN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OR.SisProductosBL
{
    public class UsuarioBL
    {
        // Ejemplo de método para agregar un usuario  
        public void AgregarUsuario(UsuarioEn usuario)
        {
            // Aquí podrías implementar la lógica para agregar un nuevo usuario  
            Console.WriteLine($"Usuario {usuario.UserName} agregado.");
        }

        // Ejemplo de otro método para autenticar al usuario  
        public bool AutenticarUsuario(string username, string password)
        {
            // Lógica de autenticación. Aquí puede ir la verificación del usuario en la base de datos.  
            // Esto es solo un ejemplo del flujo de la lógica.  
            Console.WriteLine($"Autenticando usuario {username}...");
            // Lógica de autenticación (por ejemplo, comparar con un hash almacenado)  
            return true; // Retornar verdadero o falso según la autenticación  
        }
    }
}

