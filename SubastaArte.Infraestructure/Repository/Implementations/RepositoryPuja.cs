using Microsoft.EntityFrameworkCore;
using SubastaArte.Infraestructure.Data;
using SubastaArte.Infraestructure.Models;
using SubastaArte.Infraestructure.Repository.Interfaces;using System;
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

        public async Task<Puja> AddAsync(Puja entity)
        {
            await _context.Set<Puja>().AddAsync(entity);
            await _context.SaveChangesAsync();

            var pujaGuardada = await _context.Set<Puja>()
                .Where(x => x.IdPuja == entity.IdPuja)
                .Include(x => x.IdUsuarioNavigation)
                .Include(x => x.IdSubastaNavigation)
                .FirstOrDefaultAsync();

            return pujaGuardada!;
        }

        public async Task<Puja> FindByIdAsync(int id)
        {
            var @object = await _context.Set<Puja>()
                                       .Where(l => l.IdPuja == id)
                                       .Include(x => x.IdUsuarioNavigation)
                                       .Include(x => x.IdSubastaNavigation)
                                       .FirstOrDefaultAsync();
            return @object!;
        }

        public async Task<Puja?> GetHighestBidBySubastaAsync(int idSubasta)
        {
            var pujaLider = await _context.Set<Puja>()
                .Where(x => x.IdSubasta == idSubasta)
                .Include(x => x.IdUsuarioNavigation)
                .Include(x => x.IdSubastaNavigation)
                .OrderByDescending(x => x.Monto)
                .ThenByDescending(x => x.FechaHora)
                .FirstOrDefaultAsync();

            return pujaLider;
        }

        public async Task<ICollection<Puja>> ListAsync()
        {
            var collection = await _context.Set<Puja>()
                .Include(x => x.IdUsuarioNavigation)
                .Include(x => x.IdSubastaNavigation)
                .AsNoTracking()
                .ToListAsync();
            return collection;
        }

        public async Task<ICollection<Puja>> ListSubastaIdAsync(int idSubasta)
        {
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