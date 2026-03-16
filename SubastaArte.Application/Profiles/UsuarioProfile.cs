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
    public class UsuarioProfile : Profile
    {
        public UsuarioProfile()
        {
            //Entidad - DTO (Cuando es listar o detallar)
            CreateMap<Usuario, UsuarioDTO>().ReverseMap();

            //DTO - Entidad(Cuando es crear o actualizar)
            CreateMap<UsuarioDTO, Usuario>().ReverseMap();

            CreateMap<Usuario, UsuarioDTO>();
            //.ForMember(dest => dest.IdRolNavigation, orig => orig.Ignore())
            //.ForMember(dest => dest.IdEstadoUsuarioNavigation, orig => orig.Ignore());

            CreateMap<UsuarioDTO, Usuario>();
            //.ForMember(dest => dest.IdRolNavigation, orig => orig.Ignore())
            //.ForMember(dest => dest.IdEstadoUsuarioNavigation, orig => orig.Ignore());

            
        }
    }
}
