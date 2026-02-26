using SubastaArte.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubastaArte.Infraestructure.Repository.Interfaces
{
    public interface IRepositoryPuja
    {
        Task<ICollection<Puja>> ListAsync();
        Task<Puja> FindByIdAsync(int id);

        Task<ICollection<Puja>> ListSubastaIdAsync(int idSubasta);

    }
}
