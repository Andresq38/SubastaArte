using SubastaArte.Infraestructure.Data;
using SubastaArte.Infraestructure.Models;
using SubastaArte.Infraestructure.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubastaArte.Infraestructure.Repository.Implementations
{
    public class RepositoryUsuario : IRepositoryUsuario
    {
        private readonly SubastasContext _context;

        public RepositoryUsuario(SubastasContext context)
        {
            _context = context;
        }

        public async Task<Usuario> FindByIdAsync(int id)
        {
            var @object = await _context.Set<Usuario>().
                                        Where(l => l.IdUsuario == id)
                                        .Include(x => x.IdEstadoUsuarioNavigation)
                                        .Include(x => x.IdRolNavigation)
                                        .Include(x => x.SubastaIdVendedorNavigation)
                                        .Include(x => x.Puja)
                                        .FirstOrDefaultAsync();
            return @object!;
        }

        public async Task<ICollection<Usuario>> ListAsync()
        {
            //Select * from Usuario
            var collection = await _context.Set<Usuario>()
                .Include(x => x.IdEstadoUsuarioNavigation)
                .Include(x => x.IdRolNavigation)
                .Include(x => x.SubastaIdVendedorNavigation)
                .Include(x => x.Puja)
                .AsNoTracking()
                .ToListAsync();
            return collection;
        }

        public async Task UpdateAsync(Usuario entity)
        {
            // Busca el usuario original en la base de datos
            var usuario = await _context.Usuario.FindAsync(entity.IdUsuario);
            if (usuario == null)
                throw new Exception("Usuario no encontrado");

            // Actualiza solo los campos permitidos
            usuario.Nombre = entity.Nombre;
            usuario.Apellido1 = entity.Apellido1;
            usuario.Apellido2 = entity.Apellido2;
            usuario.Email = entity.Email;

            await _context.SaveChangesAsync();
        }
        public async Task ChangeEstadoAsync(int idUsuario, int idEstadoUsuario)
        {
            var usuario = await _context.Usuario.FindAsync(idUsuario);
            if (usuario == null)
                throw new Exception("Usuario no encontrado");

            usuario.IdEstadoUsuario = idEstadoUsuario;

            await _context.SaveChangesAsync();
        }
    }
}
