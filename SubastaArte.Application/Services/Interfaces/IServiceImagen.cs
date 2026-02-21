using SubastaArte.Application.DTOs;
using SubastaArte.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubastaArte.Application.Services.Interfaces
{
    public interface IServiceImagen
    {
        Task<ICollection<ImagenDTO>> ListAsync();
        Task<ImagenDTO?> FindByIdAsync(int id);
    }
}
