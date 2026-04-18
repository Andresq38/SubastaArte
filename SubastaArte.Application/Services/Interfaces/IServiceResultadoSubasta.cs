using SubastaArte.Application.DTOs;
using SubastaArte.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubastaArte.Application.Services.Interfaces
{
    public interface IServiceResultadoSubasta
    {
        Task<ICollection<ResultadoSubastaDTO>> ListAsync();
        Task<ResultadoSubastaDTO?> FindByIdAsync(int id);

        Task<ResultadoSubastaDTO?> FindBySubastaIdAsync(int idSubasta);
        Task<ResultadoSubastaDTO?> RegistrarResultadoAsync(int idSubasta, DateTime fechaCierre);
    }
}
