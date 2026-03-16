using AutoMapper;
using SubastaArte.Application.DTOs;
using SubastaArte.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubastaArte.Application.Profiles
{
    public class ObjetoProfile : Profile
    {
        public ObjetoProfile()
        {
            CreateMap<Objeto, ObjetoDTO>()
                // Mandar la navegación solo para mostrarla
                .ForMember(d => d.IdVendedorNavigation, o => o.MapFrom(s => s.IdVendedorNavigation))
                // Categorías solo para mostrar
                .ForMember(d => d.IdCategoria, o => o.MapFrom(s => s.IdCategoria))
                .ReverseMap();

            // DTO → ENTIDAD (CREAR / EDITAR)
            CreateMap<ObjetoDTO, Objeto>()
            // BLOQUEAR la navegación en dirección DTO → Entidad
            .ForMember(dest => dest.IdVendedorNavigation, orig => orig.Ignore())
            .ForMember(dest => dest.IdEstadoObjetoNavigation, orig => orig.Ignore())
            .ForMember(dest => dest.Subasta, orig => orig.Ignore())
            .ForMember(dest => dest.IdCategoria, orig => orig.Ignore());
            
            //.ForMember(dest => dest.Foto, orig => orig.Ignore());
        }
    }
}
