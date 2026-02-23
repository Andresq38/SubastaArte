using Microsoft.EntityFrameworkCore;
using SubastaArte.Infraestructure.Data;
using SubastaArte.Infraestructure.Models;
using SubastaArte.Infraestructure.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubastaArte.Infraestructure.Repository.Implementations
{
    public class RepositoryCancelacionSubasta : IRepositoryCancelacionSubasta
    {
        private readonly SubastasContext _context;

        public RepositoryCancelacionSubasta(SubastasContext context)
        {
            _context = context;
        }

        public async Task<CancelacionSubasta?> FindByIdAsync(int id)
        {
            var @object = await _context.Set<CancelacionSubasta>().
                                       Where(l => l.IdCancelacion == id)
                                       .Include(x => x.IdSubastaNavigation)
                                       .Include(x => x.IdUsuarioNavigation)
                                       .FirstOrDefaultAsync();
            return @object!;
        }

        public async Task<ICollection<CancelacionSubasta>> ListAsync()
        {
            //Select * from Cancelacion Subasta
            var collection = await _context.Set<CancelacionSubasta>()
                .Include(x => x.IdSubastaNavigation)
                .Include(x => x.IdUsuarioNavigation)
                .AsNoTracking()
                .ToListAsync();
            return collection;
        }
    }
}
