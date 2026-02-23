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
    public class ServiceCancelacionSubasta : IServiceCancelacionSubasta
    {
        private readonly IRepositoryCancelacionSubasta _repository;
        private readonly IMapper _mapper;

        public ServiceCancelacionSubasta(IRepositoryCancelacionSubasta repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<CancelacionSubastaDTO?> FindByIdAsync(int id)
        {
            var @object = await _repository.FindByIdAsync(id);
            var objectMapped = _mapper.Map<CancelacionSubastaDTO>(@object);
            return objectMapped;
        }

        public async Task<ICollection<CancelacionSubastaDTO>> ListAsync()
        {
            var list = await _repository.ListAsync();
            var collection = _mapper.Map<ICollection<CancelacionSubastaDTO>>(list);
            return collection;
        }
    }
}
