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
    public class ServiceRol : IServiceRol
    {
        private readonly IRepositoryEstadoUsuario _repository;
        private readonly IMapper _mapper;
        public Task<RolDTO?> FindByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ICollection<RolDTO>> ListAsync()
        {
            var list = await _repository.ListAsync();
            var collection = _mapper.Map<ICollection<RolDTO>>(list);
            return collection;
        }
    }
}
