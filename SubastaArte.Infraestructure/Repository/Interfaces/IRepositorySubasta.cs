using SubastaArte.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubastaArte.Infraestructure.Repository.Interfaces
{
    public interface IRepositorySubasta
    {
        Task<ICollection<Subasta>> ListAsync(int estadoId);
        Task<Subasta> FindByIdAsync(int id);
        Task DeleteAsync(int id);

        Task<int> AddAsync(Subasta entity, string[] selectedObjetos);
        Task UpdateAsync(Subasta entity, string[] selectedObjetos);
        Task ChangeEstadoAsync(int idSubasta, int idEstadoSubasta);


        //Task<ICollection<Subasta>> GetObjetoByCategoria(int IdCategoria);
        //Task<ICollection<Subasta>> FindByNameAsync(string nombre);
    }
}
