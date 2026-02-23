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
                                        .Include(x => x.IdObjetoNavigation)
                                            .ThenInclude(y => y.Foto)
                                        .Include(x => x.IdCreadorNavigation)
                                        .Include(x => x.IdEstadoSubastaNavigation)
                                        .Include(x => x.IdVendedorNavigation)
                                        .FirstOrDefaultAsync();
            return @object!;
        }

        public async Task<ICollection<Subasta>> ListAsync(int estadoId)
        {
            //Select * from Subasta
            var collection = await _context.Set<Subasta>()
                .Where(x => x.IdEstadoSubasta == estadoId)
                .Include(x => x.IdObjetoNavigation)
                .ThenInclude(y => y.Foto)
                .Include(x => x.IdCreadorNavigation)
                .Include(x => x.IdEstadoSubastaNavigation)
                .Include(x => x.IdVendedorNavigation)
                .AsNoTracking()
                .ToListAsync();
            return collection;
        }
    }
}
