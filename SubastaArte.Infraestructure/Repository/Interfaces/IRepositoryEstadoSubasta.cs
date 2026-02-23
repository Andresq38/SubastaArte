using SubastaArte.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubastaArte.Infraestructure.Repository.Interfaces
{
    public interface IRepositoryEstadoSubasta
    {
        Task<ICollection<EstadoSubasta>> ListAsync();
        Task<EstadoSubasta> FindByIdAsync(int id);
    }
}
