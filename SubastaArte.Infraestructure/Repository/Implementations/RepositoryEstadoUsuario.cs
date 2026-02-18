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
    public class RepositoryEstadoUsuario : IRepositoryEstadoUsuario
    {
        private readonly SubastasContext _context;

        public RepositoryEstadoUsuario(SubastasContext context)
        {
            _context = context;
        }

        public Task<EstadoUsuario> FindByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ICollection<EstadoUsuario>> ListAsync()
        {
            //Select * from EstadoUsuario
            var collection = await _context.Set<EstadoUsuario>()
                .AsNoTracking()
                .ToListAsync();
            return collection;
        }
    }
}
