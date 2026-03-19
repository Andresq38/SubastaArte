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
    public class SubastaProfile : Profile
    {
        public SubastaProfile()
        {
            // ENTIDAD → DTO (MOSTRAR)
            CreateMap<Subasta, SubastaDTO>()
                // Incluir navegaciones para mostrar
                .ForMember(d => d.IdCreadorNavigation, o => o.MapFrom(s => s.IdCreadorNavigation))
                .ForMember(d => d.IdVendedorNavigation, o => o.MapFrom(s => s.IdVendedorNavigation))
                .ForMember(d => d.IdObjetoNavigation, o => o.MapFrom(s => s.IdObjetoNavigation))
                .ForMember(d => d.IdEstadoSubastaNavigation, o => o.MapFrom(s => s.IdEstadoSubastaNavigation));

            // DTO → ENTIDAD (CREAR / EDITAR)  
            CreateMap<SubastaDTO, Subasta>()
                // BLOQUEAR navegaciones en dirección DTO → Entidad
                .ForMember(dest => dest.IdCreadorNavigation, orig => orig.Ignore())
                .ForMember(dest => dest.IdVendedorNavigation, orig => orig.Ignore())
                .ForMember(dest => dest.IdObjetoNavigation, orig => orig.Ignore())
                .ForMember(dest => dest.IdEstadoSubastaNavigation, orig => orig.Ignore())
                .ForMember(dest => dest.CancelacionSubasta, orig => orig.Ignore())
                .ForMember(dest => dest.Pago, orig => orig.Ignore())
                .ForMember(dest => dest.Puja, orig => orig.Ignore())
                .ForMember(dest => dest.ResultadoSubasta, orig => orig.Ignore());
        }
    }
    
}
