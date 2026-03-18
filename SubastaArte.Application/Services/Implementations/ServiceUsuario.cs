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

        public async Task ChangeEstadoAsync(int idUsuario, int idEstadoUsuario)
        {
            await _repository.ChangeEstadoAsync(idUsuario, idEstadoUsuario);
        }

        public async Task<UsuarioDTO?> FindByIdAsync(int id)
        {
            var @object = await _repository.FindByIdAsync(id);
            if (@object == null)
                return null;

            var objectMapped = _mapper.Map<UsuarioDTO>(@object);
            if (objectMapped == null)
                return null;

            if (objectMapped.IdRol == 2)
            {
                objectMapped.CantidadSubastas = @object.SubastaIdVendedorNavigation?.Count ?? 0;
            }
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

        public async Task UpdateAsync(int id, UsuarioDTO dto)
        {
            // Trae el usuario original (trackeado)
            var entity = await _repository.FindByIdAsync(id);
            if (entity == null)
                throw new Exception("Usuario no encontrado");

            // Actualiza solo los campos permitidos
            entity.Nombre = dto.Nombre;
            entity.Apellido1 = dto.Apellido1;
            entity.Apellido2 = dto.Apellido2;
            entity.Email = dto.Email;

            // No se modifica: Rol, FechaRegistro, EstadoUsuario

            await _repository.UpdateAsync(entity);
        }
    }
}
