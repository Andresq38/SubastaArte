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

        public async Task<int> AddAsync(Pago entity)
        {
            await _context.Set<Pago>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity.IdPago;
        }

        public async Task ConfirmarPagoAsync(int idSubasta)
        {
            var pago = await _context.Set<Pago>()
                .FirstOrDefaultAsync(x => x.IdSubasta == idSubasta);

            if (pago == null)
            {
                throw new InvalidOperationException("No existe pago asociado a la subasta.");
            }

            if (!string.Equals(pago.EstadoPago, "Pendiente", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            pago.EstadoPago = "Confirmado";
            pago.FechaPago = DateTime.Now;

            await _context.SaveChangesAsync();
        }

        public async Task<Pago?> FindByIdAsync(int id)
        {
            return await _context.Set<Pago>()
                .Where(x => x.IdPago == id)
                .Include(x => x.IdSubastaNavigation)
                .FirstOrDefaultAsync();
        }

        public async Task<Pago?> FindBySubastaIdAsync(int idSubasta)
        {
            return await _context.Set<Pago>()
                .Where(x => x.IdSubasta == idSubasta)
                .Include(x => x.IdSubastaNavigation)
                .FirstOrDefaultAsync();
        }

        public async Task<ICollection<Pago>> ListAsync()
        {
            return await _context.Set<Pago>()
                .Include(x => x.IdSubastaNavigation)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
