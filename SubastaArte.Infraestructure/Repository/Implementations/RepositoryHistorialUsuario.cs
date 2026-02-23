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
    public class RepositoryHistorialUsuario : IRepositoryHistorialUsuario
    {
        private readonly SubastasContext _context;

        public RepositoryHistorialUsuario(SubastasContext context)
        {
            _context = context;
        }

        public Task<HistorialUsuario> FindByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ICollection<HistorialUsuario>> ListAsync()
        {
            //Select * from Historial Usuario
            var collection = await _context.Set<HistorialUsuario>()
                .AsNoTracking()
                .ToListAsync();
            return collection;
        }
    }
}
