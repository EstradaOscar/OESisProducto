using Microsoft.EntityFrameworkCore;
using OR.SisProductosEN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OR.SisProductosDAL
{
    public class ProveedorDAL
    {
        readonly ORSysProductosDbContext dbContext;
        public ProveedorDAL(ORSysProductosDbContext sysProductosDB)
        {
            dbContext = sysProductosDB;
        }
        public async Task<int> CrearAsync(ProveedorEN pProveedor)
        {
            ProveedorEN proveedor = new ProveedorEN()
            {
                Nombre = pProveedor.Nombre,
                NRC = pProveedor.NRC,
                Direccion = pProveedor.Direccion,
                Telefono = pProveedor.Telefono,
                Email = pProveedor.Email
            };
            dbContext.Proveedores.Add(proveedor);
            return await dbContext.SaveChangesAsync();
        }
        public async Task<int> EliminarAsync(ProveedorEN pProveedor)
        {
            var proveedor = await dbContext.Proveedores.FirstOrDefaultAsync(s => s.Id == pProveedor.Id);
            if (proveedor != null && proveedor.Id != 0)
            {
                dbContext.Proveedores.Remove(proveedor);
                return await dbContext.SaveChangesAsync();
            }
            else
                return 0;
        }
        public async Task<int> ModificarAsync(ProveedorEN pProveedor)
        {
            var proveedor = await dbContext.Proveedores.FirstOrDefaultAsync(s => s.Id == pProveedor.Id);
            if (proveedor != null && proveedor.Id != 0)
            {
                proveedor.Nombre = pProveedor.Nombre;
                proveedor.NRC = pProveedor.NRC;
                proveedor.Direccion = pProveedor.Direccion;
                proveedor.Telefono = pProveedor.Telefono;
                proveedor.Email = pProveedor.Email;

                dbContext.Update(proveedor);
                return await dbContext.SaveChangesAsync();
            }
            else
                return 0;
        }
        public async Task<ProveedorEN> ObtenerPorIdAsync(ProveedorEN pProveedor)
        {
            var proveedor = await dbContext.Proveedores.FirstOrDefaultAsync(s => s.Id == pProveedor.Id);
            if (proveedor != null && proveedor.Id != 0)
            {
                return new ProveedorEN
                {
                    Id = proveedor.Id,
                    Nombre = proveedor.Nombre,
                    NRC = proveedor.NRC,
                    Direccion = proveedor.Direccion,
                    Telefono = proveedor.Telefono,
                    Email = pProveedor.Email
                };
            }
            else
                return new ProveedorEN();
        }
        public async Task<List<ProveedorEN>> ObtenerTodosAsync()
        {
            var proveedores = await dbContext.Proveedores.ToListAsync();
            if (proveedores != null && proveedores.Count > 0)
            {
                var list = new List<ProveedorEN>();
                proveedores.ForEach(p => list.Add(new ProveedorEN
                {
                    Id = p.Id,
                    Nombre = p.Nombre,
                    NRC = p.NRC,
                    Direccion = p.Direccion,
                    Telefono = p.Telefono,
                    Email = p.Email
                }));
                return list;
            }
            else
                return new List<ProveedorEN>();

        }
        public async Task AgregarTodosAsync(List<ProveedorEN> pProveedores)
        {
            await dbContext.Proveedores.AddRangeAsync(pProveedores);
            await dbContext.SaveChangesAsync();
        }
    }
}
