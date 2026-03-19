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
    public class ServiceSubasta : IServiceSubasta
    {
        private readonly IRepositorySubasta _repository;
        private readonly IMapper _mapper;

        public ServiceSubasta(IRepositorySubasta repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<int> AddAsync(SubastaDTO dto, string[] selectedObjetos)
        {
            try
            {
                var entity = _mapper.Map<Subasta>(dto);
                return await _repository.AddAsync(entity, selectedObjetos);
            }
            catch (AutoMapperMappingException ex)
            {
                var msg = ex.ToString(); // incluye tipos origen/destino y qué miembro falló 
                throw;
            }
        }

        public async Task ChangeEstadoAsync(int idSubasta, int idEstadoSubasta)
        {
            await _repository.ChangeEstadoAsync(idSubasta, idEstadoSubasta);
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<SubastaDTO> FindByIdAsync(int id)
        {
            var @object = await _repository.FindByIdAsync(id);
            var objectMapped = _mapper.Map<SubastaDTO>(@object);

           objectMapped.PujasSubasta = @object.Puja?.Count ?? 0;


            return objectMapped;
        }

        public async Task<ICollection<SubastaDTO>> ListAsync(int estadoId)
        {
            var list = await _repository.ListAsync(estadoId);

            //var collection = _mapper.Map<ICollection<SubastaDTO>>(list);

            // Cantidad de pujas por subasta / Cambiar Nombres de variables
            var collection = list.Select(s =>
            {
                var dto = _mapper.Map<SubastaDTO>(s);
                dto.PujasSubasta = s.Puja?.Count ?? 0;
                return dto;
            }).ToList();

            return collection;
        }

        public async Task UpdateAsync(int id, SubastaDTO dto, string[] selectedObjetos)
        {
            // Trae la subasta original (trackeada)
            var entity = await _repository.FindByIdAsync(id);
            if (entity == null)
                throw new Exception("Subasta no encontrada");

            // Actualiza solo los campos permitidos
            entity.FechaInicio = dto.FechaInicio;
            entity.FechaCierre = dto.FechaCierre;
            entity.PrecioBase = dto.PrecioBase;
            entity.IncrementoMinimo = dto.IncrementoMinimo;

            // No se modifica: Objeto, Creador, Vendedor, Estado, etc.

            await _repository.UpdateAsync(entity, selectedObjetos);
        }
    }
}
