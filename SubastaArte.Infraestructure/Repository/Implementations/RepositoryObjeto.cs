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

            // Aplica la relación de imágenes
            ApplyImagenes(entity);

            // Agregar el objeto a la base de datos
            await _context.Set<Objeto>().AddAsync(entity);
            await _context.SaveChangesAsync();

            return entity.IdObjeto;
        }

        public async Task DeleteAsync(int id)
        {
            // Buscar el objeto con todas sus relaciones necesarias para validación
            var objeto = await _context.Objeto
                .Include(o => o.Subasta)
                    .ThenInclude(s => s.IdEstadoSubastaNavigation)
                .Include(o => o.Foto) // Incluir imágenes para eliminarlas también
                .Include(o => o.IdCategoria) // ¡AGREGAR ESTA LÍNEA! - Incluir categorías
                .FirstOrDefaultAsync(o => o.IdObjeto == id);

            if (objeto == null)
                throw new Exception($"Objeto con ID {id} no encontrado");

            // Verificar si el objeto pertenece a subastas activas (estado 1) o finalizadas (estado 2)
            bool perteneceASubastaActivaOFinalizada = objeto.Subasta?.Any(s => s.IdEstadoSubasta == 1 || s.IdEstadoSubasta == 2) == true;

            if (perteneceASubastaActivaOFinalizada)
            {
                throw new InvalidOperationException(
                    "No se puede eliminar el objeto porque pertenece a una subasta activa o finalizada.");
            }

            // Si llegamos aquí, el objeto SÍ se puede eliminar

            // 1. LIMPIAR RELACIONES CON CATEGORÍAS (tabla ObjetoCategoria)
            objeto.IdCategoria.Clear();

            // 2. Eliminar todas las imágenes asociadas (cascade delete debería manejar esto, pero por seguridad)
            if (objeto.Foto != null && objeto.Foto.Any())
            {
                _context.Imagen.RemoveRange(objeto.Foto);
            }

            // 3. Eliminar el objeto
            _context.Objeto.Remove(objeto);

            // 4. Guardar cambios
            await _context.SaveChangesAsync();
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

        private void ApplyImagenes(Objeto objetoToUpdate)
        {
            if (objetoToUpdate.Foto == null || !objetoToUpdate.Foto.Any())
                return;

            foreach (var imagen in objetoToUpdate.Foto)
            {
                // Asocia la imagen al objeto (relación inversa)
                imagen.IdObjetoNavigation = objetoToUpdate;
            }
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

        public async Task UpdateAsync(Objeto entity, string[] selectedCategorias)
        {
            // Traer el objeto original desde la base de datos, incluyendo imágenes
            var objetoDb = await _context.Objeto
                .Include(o => o.Foto)
                .Include(o => o.IdCategoria)
                .FirstOrDefaultAsync(o => o.IdObjeto == entity.IdObjeto);

            if (objetoDb == null)
                throw new Exception("Objeto no encontrado");

            // Verifica si el objeto está en una subasta activa
            bool enSubastaActiva = await _context.Subasta
                .AnyAsync(s => s.IdObjeto == entity.IdObjeto && s.IdEstadoSubasta == 1);

            if (enSubastaActiva)
                throw new InvalidOperationException("No se puede editar el objeto porque está en una subasta activa.");

            // *** ACTUALIZAR PROPIEDADES BÁSICAS DEL OBJETO ***
            objetoDb.Nombre = entity.Nombre;
            objetoDb.Descripcion = entity.Descripcion;
            objetoDb.Condicion = entity.Condicion;
            objetoDb.FechaRegistro = DateTime.Now; // <-- AGREGAR ESTA LÍNEA

            // Actualiza categorías
            await ApplyCategoriasAsync(objetoDb, selectedCategorias);

            // Resto del código igual...
            var fotos = entity.Foto?.ToList() ?? new List<Imagen>();

            var idsAConservar = fotos
                .Where(f => f.IdImagen > 0)
                .Select(f => f.IdImagen)
                .ToList();

            foreach (var img in objetoDb.Foto.ToList())
            {
                if (img.IdImagen > 0 && !idsAConservar.Contains(img.IdImagen))
                {
                    _context.Imagen.Remove(img);
                }
            }

            foreach (var img in fotos.Where(f => f.IdImagen == 0 && f.Foto != null))
            {
                objetoDb.Foto.Add(new Imagen
                {
                    Foto = img.Foto
                });
            }

            await _context.SaveChangesAsync();
        }











    }
}
