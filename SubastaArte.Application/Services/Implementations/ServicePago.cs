using AutoMapper;
using Microsoft.VisualBasic;
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
    public class ServicePago : IServicePago
    {
        private readonly IRepositoryPago _repository;
        private readonly IMapper _mapper;

        public ServicePago(IRepositoryPago repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task ConfirmarPagoAsync(int idSubasta)
        {
            await _repository.ConfirmarPagoAsync(idSubasta);
        }

        public async Task<PagoDTO?> FindByIdAsync(int id)
        {
            var entity = await _repository.FindByIdAsync(id);
            return entity == null ? null : _mapper.Map<PagoDTO>(entity);
        }

        public async Task<PagoDTO?> FindBySubastaIdAsync(int idSubasta)
        {
            var entity = await _repository.FindBySubastaIdAsync(idSubasta);
            return entity == null ? null : _mapper.Map<PagoDTO>(entity);
        }

        public async Task<ICollection<PagoDTO>> ListAsync()
        {
            var list = await _repository.ListAsync();
            return _mapper.Map<ICollection<PagoDTO>>(list);
        }

        public async Task<PagoDTO?> RegistrarPagoPendienteAsync(int idSubasta)
        {
            var existente = await _repository.FindBySubastaIdAsync(idSubasta);
            if (existente != null)
            {
                return _mapper.Map<PagoDTO>(existente);
            }

            var entity = new Pago
            {
                IdSubasta = idSubasta,
                EstadoPago = "Pendiente",
                FechaPago = null
            };

            var idPago = await _repository.AddAsync(entity);
            var guardado = await _repository.FindByIdAsync(idPago);

            return guardado == null ? null : _mapper.Map<PagoDTO>(guardado);
        }
    }
}
