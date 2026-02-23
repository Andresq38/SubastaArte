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
    public class ServiceEstadoSubasta : IServiceEstadoSubasta
    {
        private readonly IRepositoryEstadoSubasta _repository;
        private readonly IMapper _mapper;

        public Task<EstadoSubastaDTO?> FindByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ICollection<EstadoSubastaDTO>> ListAsync()
        {
            var list = await _repository.ListAsync();
            var collection = _mapper.Map<ICollection<EstadoSubastaDTO>>(list);
            return collection;
        }
    }
}
