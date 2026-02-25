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
    }
}
