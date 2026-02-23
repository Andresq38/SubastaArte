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
            CreateMap<SubastaDTO, Subasta>().ReverseMap();
            
            CreateMap<SubastaDTO, Subasta>()
            .ForMember(dest => dest.IdObjetoNavigation, orig => orig.Ignore())
            .ForMember(dest => dest.Puja, orig => orig.Ignore());
        }
    }
}
