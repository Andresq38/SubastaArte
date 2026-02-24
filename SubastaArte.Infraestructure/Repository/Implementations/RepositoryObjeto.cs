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
    public class RepositoryObjeto : IRepositoryObjeto
    {
        private readonly SubastasContext _context;

        public RepositoryObjeto(SubastasContext context)
        {
            _context = context;
        }

        public Task<int> AddAsync(Objeto entity, string[] selectedCategorias)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Objeto> FindByIdAsync(int id)
        {
            var @object = await _context.Set<Objeto>().
                                        Where(l => l.IdObjeto == id)
                                        .Include(x => x.IdVendedorNavigation)
                                        .Include(x => x.IdCategoria)
                                        .Include(x => x.IdEstadoObjetoNavigation)
                                        .Include(x => x.Foto)
                                        .FirstOrDefaultAsync();
            return @object!;
        }

        public Task<ICollection<Objeto>> FindByNameAsync(string nombre)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<Objeto>> GetObjetoByCategoria(int IdCategoria)
        {
            throw new NotImplementedException();
        }

        public async Task<ICollection<Objeto>> ListAsync()
        {
            var collection = await _context.Set<Objeto>()
                                       .Include(x => x.IdVendedorNavigation)
                                       .OrderBy(x => x.IdVendedor)
                                       .Include(x => x.IdCategoria)
                                       .Include(x => x.IdEstadoObjetoNavigation)
                                       .Include(x => x.Foto)
                                       .AsNoTracking()
                                       .ToListAsync();
            return collection;
        }

        public Task UpdateAsync(Objeto entity, string[] selectedCategorias)
        {
            throw new NotImplementedException();
        }
    }
}
