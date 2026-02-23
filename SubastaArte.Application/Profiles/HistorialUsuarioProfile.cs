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
    public class HistorialUsuarioProfile :Profile
    {
        public HistorialUsuarioProfile()
        {
            CreateMap<HistorialUsuarioDTO, HistorialUsuario>().ReverseMap();
        }
    }
}
