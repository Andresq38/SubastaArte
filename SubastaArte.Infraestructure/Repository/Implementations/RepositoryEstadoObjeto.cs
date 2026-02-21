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
    public class RepositoryEstadoObjeto : IRepositoryEstadoObjeto
    {
        private readonly SubastasContext _context;

        public RepositoryEstadoObjeto(SubastasContext context)
        {
            _context = context;
        }

        public Task<EstadoObjeto> FindByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ICollection<EstadoObjeto>> ListAsync()
        {
            //Select * from Estado Objeto
            var collection = await _context.Set<EstadoObjeto>()
                .AsNoTracking()
                .ToListAsync();
            return collection;
        }
    }
}
