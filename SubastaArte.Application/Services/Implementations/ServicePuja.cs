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
    public class ServicePuja : IServicePuja
    {
        private readonly IRepositoryPuja _repositoryPuja;
        private readonly IRepositorySubasta _repositorySubasta;
        private readonly IMapper _mapper;

        public ServicePuja(IRepositoryPuja repositoryPuja, IRepositorySubasta repositorySubasta, IMapper mapper)
        {
            _repositoryPuja = repositoryPuja;
            _repositorySubasta = repositorySubasta;
            _mapper = mapper;
        }

        public async Task<PujaDTO?> FindByIdAsync(int id)
        {
            var @object = await _repositoryPuja.FindByIdAsync(id);
            var objectMapped = _mapper.Map<PujaDTO>(@object);
            return objectMapped;
        }

        public async Task<PujaDTO?> GetPujaLiderAsync(int idSubasta)
        {
            var pujaLider = await _repositoryPuja.GetHighestBidBySubastaAsync(idSubasta);
            if (pujaLider == null)
            {
                return null;
            }

            return _mapper.Map<PujaDTO>(pujaLider);
        }

        public async Task<ICollection<PujaDTO>> ListAsync()
        {
            var list = await _repositoryPuja.ListAsync();
            var collection = _mapper.Map<ICollection<PujaDTO>>(list);
            return collection;
        }

        public async Task<ICollection<PujaDTO>> ListSubastaIdAsync(int idSubasta)
        {
            var list = await _repositoryPuja.ListSubastaIdAsync(idSubasta);
            var collection = _mapper.Map<ICollection<PujaDTO>>(list);
            return collection;
        }

        public async Task<PujaDTO> RegistrarPujaAsync(int idSubasta, decimal monto, int idUsuarioActual)
        {
            var subasta = await _repositorySubasta.FindByIdAsync(idSubasta);

            if (subasta == null)
            {
                throw new InvalidOperationException("La subasta no existe.");
            }

            if (subasta.IdEstadoSubasta != 1)
            {
                throw new InvalidOperationException("No se puede pujar porque la subasta no está activa.");
            }

            if (DateTime.Now >= subasta.FechaCierre)
            {
                throw new InvalidOperationException("No se puede pujar porque la subasta ya cerró.");
            }

            if (subasta.IdVendedor == idUsuarioActual)
            {
                throw new InvalidOperationException("No puede pujar en su propia subasta.");
            }

            var pujaLider = subasta.Puja
                .OrderByDescending(x => x.Monto)
                .ThenByDescending(x => x.FechaHora)
                .FirstOrDefault();

            decimal montoMinimoPermitido = pujaLider == null
                ? subasta.PrecioBase
                : pujaLider.Monto + subasta.IncrementoMinimo;

            if (monto < montoMinimoPermitido)
            {
                throw new InvalidOperationException(
                    $"La puja es inválida. El monto mínimo permitido es {montoMinimoPermitido:#,##0.00}."
                );
            }

            var entity = new Puja
            {
                IdSubasta = idSubasta,
                IdUsuario = idUsuarioActual,
                Monto = monto,
                FechaHora = DateTime.Now
            };

            var pujaGuardada = await _repositoryPuja.AddAsync(entity);
            return _mapper.Map<PujaDTO>(pujaGuardada);
        }
    }
}