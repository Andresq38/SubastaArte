using SubastaArte.Application.DTOs;
using SubastaArte.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubastaArte.Application.Services.Interfaces
{
    public interface IServiceEstadoUsuario
    {
        Task<ICollection<EstadoUsuarioDTO>> ListAsync();
        Task<EstadoUsuarioDTO?> FindByIdAsync(int id);
    }
}
