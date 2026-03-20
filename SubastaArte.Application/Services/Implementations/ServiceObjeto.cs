using AutoMapper;
using SubastaArte.Application.DTOs;
using SubastaArte.Application.Services.Interfaces;
using SubastaArte.Infraestructure.Models;
using SubastaArte.Infraestructure.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubastaArte.Application.Services.Implementations
{
    public class ServiceObjeto : IServiceObjeto
    {
        private readonly IRepositoryObjeto _repository;
        private readonly IMapper _mapper;

        public ServiceObjeto(IRepositoryObjeto repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<int> AddAsync(ObjetoDTO dto, string[] selectedCategorias)
        {

            try
            {
                var entity = _mapper.Map<Objeto>(dto);
                return await _repository.AddAsync(entity, selectedCategorias);
            }
            catch (AutoMapperMappingException ex)
            {
                var msg = ex.ToString(); // incluye tipos origen/destino y qué miembro falló 
                throw;
            }

        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task<ObjetoDTO> FindByIdAsync(int id)
        {
            var @object = await _repository.FindByIdAsync(id);
            var objectMapped = _mapper.Map<ObjetoDTO>(@object);
            return objectMapped;
        }

        public Task<ICollection<ObjetoDTO>> FindByNameAsync(string nombre)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<ObjetoDTO>> GetObjetoByCategoria(int IdCategoria)
        {
            throw new NotImplementedException();
        }

        public async Task<ICollection<ObjetoDTO>> ListAsync()
        {
            var list = await _repository.ListAsync();
            var collection = _mapper.Map<ICollection<ObjetoDTO>>(list);
            return collection;
        }

        public async Task UpdateAsync(int id, ObjetoDTO dto, string[] selectedCategorias)
        {
            // CREAR una entidad NUEVA solo con los campos básicos - SIN cargar relaciones
            var entity = new Objeto
            {
                IdObjeto = id,
                Nombre = dto.Nombre,
                Descripcion = dto.Descripcion,
                Condicion = dto.Condicion,
                IdEstadoObjeto = dto.IdEstadoObjeto,
                IdVendedor = dto.IdVendedor

            };

            // Mapear imágenes correctamente
            entity.Foto = dto.Foto?
                .Select(x => new Imagen
                {
                    IdImagen = x.IdImagen,
                    Foto = x.Foto,
                    IdObjeto = id
                })
                .ToList() ?? new List<Imagen>();

            await _repository.UpdateAsync(entity, selectedCategorias);
        }




    }
}
