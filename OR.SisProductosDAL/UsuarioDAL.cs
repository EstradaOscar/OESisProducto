using Org.BouncyCastle.Crypto.Generators;
using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    namespace OR.SisProductosDAL
    {
        public class UsuarioDAL
        {

            readonly ORSysProductosDbContext _dbContext;

            public UsuarioDAL(ORSysProductosDbContext context)
            {
                _dbContext = context;

                string password = "admin123";
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
                Console.WriteLine(hashedPassword);
            }

        }
    }
