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
    public class ServiceResultadoSubasta : IServiceResultadoSubasta
    {
        private readonly IRepositoryResultadoSubasta _repositoryResultado;
        private readonly IRepositoryPuja _repositoryPuja;
        private readonly IMapper _mapper;

        public ServiceResultadoSubasta(
            IRepositoryResultadoSubasta repositoryResultado,
            IRepositoryPuja repositoryPuja,
            IMapper mapper)
        {
            _repositoryResultado = repositoryResultado;
            _repositoryPuja = repositoryPuja;
            _mapper = mapper;
        }

        public async Task<ResultadoSubastaDTO?> FindByIdAsync(int id)
        {
            var @object = await _repositoryResultado.FindByIdAsync(id);
            if (@object == null)
            {
                return null;
            }

            return MapResultado(@object);
        }

        public async Task<ResultadoSubastaDTO?> FindBySubastaIdAsync(int idSubasta)
        {
            var resultado = await _repositoryResultado.FindBySubastaIdAsync(idSubasta);
            if (resultado == null)
            {
                return null;
            }

            return MapResultado(resultado);
        }

        public async Task<ICollection<ResultadoSubastaDTO>> ListAsync()
        {
            var list = await _repositoryResultado.ListAsync();
            var collection = list.Select(MapResultado).ToList();
            return collection;
        }

        public async Task<ResultadoSubastaDTO?> RegistrarResultadoAsync(int idSubasta, DateTime fechaCierre)
        {
            var existente = await _repositoryResultado.FindBySubastaIdAsync(idSubasta);
            if (existente != null)
            {
                return MapResultado(existente);
            }

            var pujas = await _repositoryPuja.ListSubastaIdAsync(idSubasta);
            if (pujas == null || !pujas.Any())
            {
                return null; // Escenario sin pujas: no hay ganador
            }

            var pujaGanadora = pujas
                .OrderByDescending(x => x.Monto)
                .ThenByDescending(x => x.FechaHora)
                .First();

            var entity = new ResultadoSubasta
            {
                IdSubasta = idSubasta,
                IdUsuarioGanador = pujaGanadora.IdUsuario,
                MontoFinal = pujaGanadora.Monto,
                FechaCierre = fechaCierre
            };

            var idResultado = await _repositoryResultado.AddAsync(entity);
            var guardado = await _repositoryResultado.FindByIdAsync(idResultado);

            return guardado == null ? null : MapResultado(guardado);
        }

        private ResultadoSubastaDTO MapResultado(ResultadoSubasta entity)
        {
            var dto = _mapper.Map<ResultadoSubastaDTO>(entity);

            // Ajuste por diferencia de nombres DTO vs Modelo
            dto.IdUsuario = entity.IdUsuarioGanador;

            return dto;
        }
    }
}
