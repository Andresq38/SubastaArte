using SubastaArte.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubastaArte.Application.Services.Interfaces
{
    public interface IServicePuja
    {
        Task<ICollection<PujaDTO>> ListAsync();
        Task<PujaDTO?> FindByIdAsync(int id);
        Task<ICollection<PujaDTO>> ListSubastaIdAsync(int idSubasta);

        Task<PujaDTO> RegistrarPujaAsync(int idSubasta, decimal monto, int idUsuarioActual);
        Task<PujaDTO?> GetPujaLiderAsync(int idSubasta);
    }
}
