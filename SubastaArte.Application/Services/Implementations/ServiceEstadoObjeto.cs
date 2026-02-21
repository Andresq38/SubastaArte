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
    public class ServiceEstadoObjeto : IServiceEstadoObjeto
    {
        private readonly IRepositoryEstadoObjeto _repository;
        private readonly IMapper _mapper;

        public ServiceEstadoObjeto(IRepositoryEstadoObjeto repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Task<EstadoObjetoDTO?> FindByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ICollection<EstadoObjetoDTO>> ListAsync()
        {
            var list = await _repository.ListAsync();
            var collection = _mapper.Map<ICollection<EstadoObjetoDTO>>(list);
            return collection;
        }
    }
}
