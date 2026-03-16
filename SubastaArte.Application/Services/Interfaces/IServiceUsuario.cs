using SubastaArte.Application.DTOs;
using SubastaArte.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubastaArte.Application.Services.Interfaces
{
    public interface IServiceUsuario
    {
        Task<ICollection<UsuarioDTO>> ListAsync();
        Task<UsuarioDTO?> FindByIdAsync(int id);

        Task UpdateAsync(int id, UsuarioDTO dto);
        //Metodo para poder modificar el estado del usuario a activo o inactivo(bloqueado)
        Task ChangeEstadoAsync(int idUsuario, int idEstadoUsuario);
    }
}
