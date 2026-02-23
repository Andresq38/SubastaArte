using SubastaArte.Application.DTOs;
using SubastaArte.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubastaArte.Application.Services.Interfaces
{
    public interface IServiceHistorialUsuario
    {
        Task<ICollection<HistorialUsuarioDTO>> ListAsync();
        Task<HistorialUsuarioDTO?> FindByIdAsync(int id);
    }
}
