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
            
            CreateMap<ObjetoDTO, Objeto>().ReverseMap();
            CreateMap<ObjetoDTO, Objeto>()
            .ForMember(dest => dest.IdVendedorNavigation, orig => orig.Ignore())
            .ForMember(dest => dest.IdEstadoObjetoNavigation, orig => orig.Ignore())
            .ForMember(dest => dest.Subasta, orig => orig.Ignore())
            .ForMember(dest => dest.IdCategoria, orig => orig.Ignore())
            .ForMember(dest => dest.Imagen, orig => orig.Ignore());
        }
    }
}
