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
    public class ServiceEstadoUsuario : IServiceEstadoUsuario
    {
        private readonly IRepositoryEstadoUsuario _repository;
        private readonly IMapper _mapper;

        public ServiceEstadoUsuario(IRepositoryEstadoUsuario repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Task<EstadoUsuarioDTO?> FindByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ICollection<EstadoUsuarioDTO>> ListAsync()
        {
            var list = await _repository.ListAsync();
            var collection = _mapper.Map<ICollection<EstadoUsuarioDTO>>(list);
            return collection;
        }
    }
}
