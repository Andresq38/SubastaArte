using SubastaArte.Application.DTOs;
using SubastaArte.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubastaArte.Application.Services.Interfaces
{
    public interface IServiceSubasta
    {
        Task<ICollection<SubastaDTO>> ListAsync(int estadoId);
        Task<SubastaDTO> FindByIdAsync(int id);
        Task DeleteAsync(int id);
        Task<int> AddAsync(SubastaDTO entity, string[] selectedObjetos);
        Task UpdateAsync(int id, SubastaDTO entity, string[] selectedObjetos);
        Task ChangeEstadoAsync(int idSubasta, int idEstadoSubasta);


    }
}
