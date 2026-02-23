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
    public class RepositorySubasta : IRepositorySubasta
    {
        private readonly SubastasContext _context;

        public RepositorySubasta(SubastasContext context)
        {
            _context = context;
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Subasta> FindByIdAsync(int id)
        {
            var @object = await _context.Set<Subasta>().
                                        Where(l => l.IdSubasta == id)
                                        //.Include(x => x.IdEstadoUsuarioNavigation)
                                        //.Include(x => x.IdRolNavigation)
                                        .FirstOrDefaultAsync();
            return @object!;
        }

        public async Task<ICollection<Subasta>> ListAsync()
        {
            //Select * from Subasta
            var collection = await _context.Set<Subasta>()
                //.Include(x => x.IdEstadoUsuarioNavigation)
                //.Include(x => x.IdRolNavigation)
                .AsNoTracking()
                .ToListAsync();
            return collection;
        }
    }
}
