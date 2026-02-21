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
    public class RepositoryCategoria : IRepositoryCategoria
    {
        private readonly SubastasContext _context;

        public RepositoryCategoria(SubastasContext context)
        {
            _context = context;
        }

        public async Task<Categoria?> FindByIdAsync(int id)
        {
            return await _context.Set<Categoria>()
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.IdCategoria == id);
        }

        public async Task<ICollection<Categoria>> ListAsync()
        {
            //Select * from Categoria
            var collection = await _context.Set<Categoria>()
                .AsNoTracking()
                .ToListAsync();
            //throw new Exception("Error de prueba en RepositoryCategoria.ListAsync");
            return collection;
        }
    }
}
