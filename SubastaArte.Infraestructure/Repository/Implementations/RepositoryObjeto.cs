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

        public async Task<int> AddAsync(Objeto entity, string[] selectedCategorias)
        {
            // Asignar categorías al objeto
            await ApplyCategoriasAsync(entity, selectedCategorias);

            // Agregar el objeto a la base de datos
            await _context.Set<Objeto>().AddAsync(entity);
            await _context.SaveChangesAsync();

            return entity.IdObjeto;
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        private async Task ApplyCategoriasAsync(Objeto objetoToUpdate, string[] selectedCategorias)
        {
            // Si no enviaron categorías, se establece vacío
            if (selectedCategorias == null || selectedCategorias.Length == 0)
            {
                objetoToUpdate.IdCategoria = new List<Categoria>();
                return;
            }

            // Parse seguro
            var ids = selectedCategorias
                .Select(x => int.TryParse(x, out var n) ? n : (int?)null)
                .Where(x => x.HasValue)
                .Select(x => x!.Value)
                .Distinct()
                .ToList();

            if (ids.Count == 0)
            {
                objetoToUpdate.IdCategoria = new List<Categoria>();
                return;
            }

            // Trae SOLO las categorías requeridas
            var categorias = await _context.Categoria
                .Where(c => ids.Contains(c.IdCategoria))
                .ToListAsync();

            objetoToUpdate.IdCategoria = categorias;
        }


        public async Task<Objeto> FindByIdAsync(int id)
        {
            var @object = await _context.Set<Objeto>().
                                        Where(l => l.IdObjeto == id)
                                        .Include(x => x.IdVendedorNavigation)
                                        .Include(x => x.IdCategoria)
                                        .Include(x => x.IdEstadoObjetoNavigation)
                                        .Include(x => x.Foto)
                                        .Include(x => x.Subasta)
                                            .ThenInclude(y => y.IdEstadoSubastaNavigation)
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
                                       .Include(x => x.Subasta)
                                            .ThenInclude(y => y.IdEstadoSubastaNavigation)
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
