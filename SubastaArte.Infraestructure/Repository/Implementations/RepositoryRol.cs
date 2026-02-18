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
    public class RepositoryRol : IRepositoryRol
    {
        private readonly SubastasContext _context;

        public RepositoryRol(SubastasContext context)
        {
            _context = context;
        }

        public Task<Rol> FindByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ICollection<Rol>> ListAsync()
        {
            //Select * from Rol
            var collection = await _context.Set<Rol>()
                .AsNoTracking()
                .ToListAsync();
            return collection;
        }
    }
}
