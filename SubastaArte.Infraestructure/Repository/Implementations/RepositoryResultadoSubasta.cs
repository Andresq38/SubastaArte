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
    public class RepositoryResultadoSubasta : IRepositoryResultadoSubasta
    {
        private readonly SubastasContext _context;

        public RepositoryResultadoSubasta(SubastasContext context)
        {
            _context = context;
        }

        public async Task<int> AddAsync(ResultadoSubasta entity)
        {
            await _context.Set<ResultadoSubasta>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity.IdResultado;
        }

        public async Task<ResultadoSubasta?> FindByIdAsync(int id)
        {
            var @object = await _context.Set<ResultadoSubasta>()
                .Where(l => l.IdResultado == id)
                .Include(x => x.IdSubastaNavigation)
                .Include(x => x.IdUsuarioGanadorNavigation)
                .FirstOrDefaultAsync();

            return @object;
        }

        public async Task<ResultadoSubasta?> FindBySubastaIdAsync(int idSubasta)
        {
            var resultado = await _context.Set<ResultadoSubasta>()
                .Where(x => x.IdSubasta == idSubasta)
                .Include(x => x.IdSubastaNavigation)
                .Include(x => x.IdUsuarioGanadorNavigation)
                .FirstOrDefaultAsync();

            return resultado;
        }

        public async Task<ICollection<ResultadoSubasta>> ListAsync()
        {
            var collection = await _context.Set<ResultadoSubasta>()
                .Include(x => x.IdSubastaNavigation)
                .Include(x => x.IdUsuarioGanadorNavigation)
                .AsNoTracking()
                .ToListAsync();

            return collection;
        }
    }
}
