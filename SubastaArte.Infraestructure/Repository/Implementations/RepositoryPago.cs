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
    public class RepositoryPago : IRepositoryPago
    {
        private readonly SubastasContext _context;

        public RepositoryPago(SubastasContext context)
        {
            _context = context;
        }

        public Task<Pago?> FindByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ICollection<Pago>> ListAsync()
        {
            //Select * from Pago
            var collection = await _context.Set<Pago>()
                .AsNoTracking()
                .ToListAsync();
            return collection;
        }
    }
}
