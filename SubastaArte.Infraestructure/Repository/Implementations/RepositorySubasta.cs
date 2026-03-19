using SubastaArte.Infraestructure.Data;
using SubastaArte.Infraestructure.Models;
using SubastaArte.Infraestructure.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;

namespace SubastaArte.Infraestructure.Repository.Implementations
{
    public class RepositorySubasta : IRepositorySubasta
    {
        private readonly SubastasContext _context;

        public RepositorySubasta(SubastasContext context)
        {
            _context = context;
        }

        private async Task ApplyObjetoAsync(Subasta subastaToUpdate, string[] selectedObjetos)
        {
            // Si no enviaron objetos, se establece vacío
            if (selectedObjetos == null || selectedObjetos.Length == 0)
            {
                subastaToUpdate.IdObjeto = 0;
                return;
            }

            // Parse seguro - tomar el primer objeto seleccionado
            var id = int.TryParse(selectedObjetos[0], out var n) ? n : (int?)null;

            if (!id.HasValue || id.Value <= 0)
            {
                subastaToUpdate.IdObjeto = 0;
                return;
            }

            // Trae SOLO el objeto requerido
            var objeto = await _context.Objeto
                .Where(o => o.IdObjeto == id.Value)
                .FirstOrDefaultAsync();

            if (objeto != null)
            {
                subastaToUpdate.IdObjeto = objeto.IdObjeto;
            }
            else
            {
                subastaToUpdate.IdObjeto = 0;
            }
        }



        public async Task<int> AddAsync(Subasta entity, string[] selectedObjetos)
        {
            // Asignar estado inicial: No activa
            //entity.IdEstadoSubasta = 3;  Estado inicial: No activa
            
            // Aplicar objeto a la subasta
            await ApplyObjetoAsync(entity, selectedObjetos);

            // Agregar la subasta a la base de datos
            await _context.Set<Subasta>().AddAsync(entity);
            await _context.SaveChangesAsync();

            return entity.IdSubasta;
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Subasta> FindByIdAsync(int id)
        {
            var @object = await _context.Set<Subasta>().
                                        Where(l => l.IdSubasta == id)
                                        .Include(x => x.IdObjetoNavigation)
                                            .ThenInclude(y => y.Foto)
                                        .Include(x => x.IdObjetoNavigation)
                                            .ThenInclude(y => y.IdCategoria)
                                        .Include(x => x.IdCreadorNavigation)
                                        .Include(x => x.IdEstadoSubastaNavigation)
                                        .Include(x => x.IdVendedorNavigation)
                                        .Include(x => x.Puja)
                                        .FirstOrDefaultAsync();
            return @object!;
        }

        public async Task<ICollection<Subasta>> ListAsync(int estadoId)
        {
            //Select * from Subasta
            var collection = await _context.Set<Subasta>()
                .Where(x => x.IdEstadoSubasta == estadoId)
                .Include(x => x.IdObjetoNavigation)
                    .ThenInclude(y => y.Foto)
                .Include(x => x.IdObjetoNavigation)
                    .ThenInclude(y => y.IdCategoria)
                .Include(x => x.IdCreadorNavigation)
                .Include(x => x.IdEstadoSubastaNavigation)
                .Include(x => x.IdVendedorNavigation)
                .Include(x => x.Puja)
                .AsNoTracking()
                .ToListAsync();
            return collection;
        }

        public async Task UpdateAsync(Subasta entity, string[] selectedObjetos)
        {
            // Buscar la subasta original, incluyendo pujas
            var subastaDb = await _context.Subasta
                .Include(s => s.Puja)
                .FirstOrDefaultAsync(s => s.IdSubasta == entity.IdSubasta);

            if (subastaDb == null)
                throw new Exception("Subasta no encontrada");

            // Solo permitir editar si NO ha iniciado y NO tiene pujas
            if (subastaDb.FechaInicio <= DateTime.Now)
                throw new InvalidOperationException("No se puede editar la subasta porque ya ha iniciado.");

            if (subastaDb.Puja != null && subastaDb.Puja.Any())
                throw new InvalidOperationException("No se puede editar la subasta porque ya tiene pujas registradas.");

            // Actualizar solo los campos permitidos
            subastaDb.FechaInicio = entity.FechaInicio;
            subastaDb.FechaCierre = entity.FechaCierre;
            subastaDb.PrecioBase = entity.PrecioBase;
            subastaDb.IncrementoMinimo = entity.IncrementoMinimo;

            await _context.SaveChangesAsync();
        }

        public async Task ChangeEstadoAsync(int idSubasta, int idEstadoSubasta)
        {
            var subasta = await _context.Subasta.FindAsync(idSubasta);
            if (subasta == null)
                throw new Exception("Subasta no encontrada");

            subasta.IdEstadoSubasta = idEstadoSubasta;

            await _context.SaveChangesAsync();
        }
    }
}
