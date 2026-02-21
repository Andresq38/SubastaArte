using SubastaArte.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubastaArte.Infraestructure.Repository.Interfaces
{
    public interface IRepositoryObjeto
    {
        Task<ICollection<Objeto>> ListAsync();
        Task<Objeto> FindByIdAsync(int id);
        Task<int> AddAsync(Objeto entity, string[] selectedCategorias);
        Task UpdateAsync(Objeto entity, string[] selectedCategorias);
        Task<ICollection<Objeto>> GetObjetoByCategoria(int IdCategoria);
        Task<ICollection<Objeto>> FindByNameAsync(string nombre);
        Task DeleteAsync(int id);
    }
}
