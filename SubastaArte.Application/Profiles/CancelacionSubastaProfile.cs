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
    public class CancelacionSubastaProfile : Profile
    {
        public CancelacionSubastaProfile()
        {
            CreateMap<CancelacionSubastaDTO, CancelacionSubasta>().ReverseMap();
            CreateMap<CancelacionSubastaDTO, CancelacionSubasta>()
            .ForMember(dest => dest.IdSubastaNavigation, orig => orig.Ignore())
            .ForMember(dest => dest.IdUsuarioNavigation, orig => orig.Ignore());
        }
    }
}
