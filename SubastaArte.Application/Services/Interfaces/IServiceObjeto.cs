using SubastaArte.Application.DTOs;
using SubastaArte.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubastaArte.Application.Services.Interfaces
{
    public interface IServiceObjeto
    {
        Task<ICollection<ObjetoDTO>> ListAsync();

        Task<ObjetoDTO> FindByIdAsync(int id);
        Task<int> AddAsync(ObjetoDTO dto, string[] selectedCategorias);

        Task UpdateAsync(int id, ObjetoDTO dto, string[] selectedCategorias);

        Task<ICollection<ObjetoDTO>> GetObjetoByCategoria(int IdCategoria);
        Task<ICollection<ObjetoDTO>> FindByNameAsync(string nombre);

        Task DeleteAsync(int id);
    }
}
