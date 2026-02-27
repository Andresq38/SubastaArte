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
    public class RepositoryPuja : IRepositoryPuja
    {
        private readonly SubastasContext _context;

        public RepositoryPuja(SubastasContext context)
        {
            _context = context;
        }

        public async Task<Puja> FindByIdAsync(int id)
        {
            var @object = await _context.Set<Puja>().
                                       Where(l => l.IdPuja == id)
                                       .Include(x => x.IdUsuarioNavigation)
                                       .Include(x => x.IdSubastaNavigation)
                                       .FirstOrDefaultAsync();
            return @object!;
        }

        public async Task<ICollection<Puja>> ListAsync()
        {
            //Select * from Puja
            var collection = await _context.Set<Puja>()
                .Include(x => x.IdUsuarioNavigation)
                .Include(x => x.IdSubastaNavigation)
                .AsNoTracking()
                .ToListAsync();
            return collection;
        }

        public async Task<ICollection<Puja>> ListSubastaIdAsync(int idSubasta)
        {
            //Select * from Puja con ID subasta determinado
            var collection = await _context.Set<Puja>()
                .Where(x => x.IdSubasta == idSubasta)
                .Include(x => x.IdUsuarioNavigation)
                .Include(x => x.IdSubastaNavigation)
                .OrderByDescending(x => x.FechaHora)
                .AsNoTracking()
                .ToListAsync();
            return collection;
        }
    }
}
