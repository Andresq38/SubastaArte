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
    public class ResultadoSubastaProfile : Profile
    {
        public ResultadoSubastaProfile()
        {
            CreateMap<ResultadoSubastaDTO, ResultadoSubasta>().ReverseMap();
            
            CreateMap<ResultadoSubastaDTO, ResultadoSubasta>()
            .ForMember(dest => dest.IdSubastaNavigation, orig => orig.Ignore())
            .ForMember(dest => dest.IdUsuarioGanadorNavigation, orig => orig.Ignore());
        }
    }
}
