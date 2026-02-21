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
    public class ServiceImagen : IServiceImagen
    {
        private readonly IRepositoryImagen _repository;
        private readonly IMapper _mapper;

        public ServiceImagen(IRepositoryImagen repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public Task<ImagenDTO?> FindByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ICollection<ImagenDTO>> ListAsync()
        {
            var list = await _repository.ListAsync();
            var collection = _mapper.Map<ICollection<ImagenDTO>>(list);
            return collection;
        }
    }
}
