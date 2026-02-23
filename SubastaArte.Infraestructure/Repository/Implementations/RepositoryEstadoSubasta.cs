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
    public class RepositoryEstadoSubasta : IRepositoryEstadoSubasta
    {
        private readonly SubastasContext _context;

        public RepositoryEstadoSubasta(SubastasContext context)
        {
            _context = context;
        }

        public Task<EstadoSubasta> FindByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ICollection<EstadoSubasta>> ListAsync()
        {
            //Select * from EstadoUsuario
            var collection = await _context.Set<EstadoSubasta>()
                .AsNoTracking()
                .ToListAsync();
            return collection;
        }
    }
}
