using SubastaArte.Infraestructure.Data;
using SubastaArte.Infraestructure.Models;
using SubastaArte.Infraestructure.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubastaArte.Infraestructure.Repository.Implementations
{
    public class RepositoryImagen : IRepositoryImagen
    {
        public Task<Imagen?> FindByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<Imagen>> ListAsync()
        {
            throw new NotImplementedException();
        }
    }
}
