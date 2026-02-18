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
                                        .FirstOrDefaultAsync();
            return @object!;
        }

        public async Task<ICollection<Usuario>> ListAsync()
        {
            //Select * from Usuario
            var collection = await _context.Set<Usuario>()
                .Include(x => x.IdEstadoUsuarioNavigation)
                .Include(x => x.IdRolNavigation)
                .AsNoTracking()
                .ToListAsync();
            return collection;
        }
    }
}
