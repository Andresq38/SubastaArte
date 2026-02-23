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
    public class PujaProfile : Profile
    {
        public PujaProfile()
        {
            CreateMap<PujaDTO, Puja>().ReverseMap();

            CreateMap<PujaDTO, Puja>()
            .ForMember(dest => dest.IdUsuarioNavigation, orig => orig.Ignore())
            .ForMember(dest => dest.IdSubastaNavigation, orig => orig.Ignore());
        }
    }
}
