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
    public class ServiceUsuario : IServiceUsuario
    {
        private readonly IRepositoryUsuario _repository;
        private readonly IMapper _mapper;

        public ServiceUsuario(IRepositoryUsuario repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<UsuarioDTO?> FindByIdAsync(int id)
        {
            var @object = await _repository.FindByIdAsync(id);
            var objectMapped = _mapper.Map<UsuarioDTO>(@object);

            // Asigna la cantidad de subastas si es vendedor (IdRol == 2)
            if (objectMapped.IdRol == 2)
            {
                objectMapped.CantidadSubastas = @object.SubastaIdVendedorNavigation?.Count ?? 0;
            }
            // Asigna la cantidad de pujas si es comprador (IdRol == 3)
            else if (objectMapped.IdRol == 3)
            {
                objectMapped.CantidadPujas = @object.Puja?.Count ?? 0;
            }

            return objectMapped;
        }

        public async Task<ICollection<UsuarioDTO>> ListAsync()
        {
            var list = await _repository.ListAsync();
            var collection = _mapper.Map<ICollection<UsuarioDTO>>(list);
            return collection;
        }
    }
}
