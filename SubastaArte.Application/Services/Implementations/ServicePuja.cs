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
    public class ServicePuja : IServicePuja
    {
        private readonly IRepositoryPuja _repository;
        private readonly IMapper _mapper;

        public ServicePuja(IRepositoryPuja repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<PujaDTO?> FindByIdAsync(int id)
        {
            var @object = await _repository.FindByIdAsync(id);
            var objectMapped = _mapper.Map<PujaDTO>(@object);
            return objectMapped;
        }

        public async Task<ICollection<PujaDTO>> ListAsync()
        {
            var list = await _repository.ListAsync();
            var collection = _mapper.Map<ICollection<PujaDTO>>(list);
            return collection;
        }
    }
}
