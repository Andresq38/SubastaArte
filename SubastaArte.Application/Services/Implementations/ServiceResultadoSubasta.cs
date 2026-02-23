using AutoMapper;
using SubastaArte.Application.DTOs;
using SubastaArte.Application.Services.Interfaces;
using SubastaArte.Infraestructure.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubastaArte.Application.Services.Implementations
{
    public class ServiceResultadoSubasta : IServiceResultadoSubasta
    {
        private readonly IRepositoryResultadoSubasta _repository;
        private readonly IMapper _mapper;

        public ServiceResultadoSubasta(IRepositoryResultadoSubasta repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ResultadoSubastaDTO?> FindByIdAsync(int id)
        {
            var @object = await _repository.FindByIdAsync(id);
            var objectMapped = _mapper.Map<ResultadoSubastaDTO>(@object);
            return objectMapped;
        }

        public async Task<ICollection<ResultadoSubastaDTO>> ListAsync()
        {
            var list = await _repository.ListAsync();
            var collection = _mapper.Map<ICollection<ResultadoSubastaDTO>>(list);
            return collection;
        }
    }
}
