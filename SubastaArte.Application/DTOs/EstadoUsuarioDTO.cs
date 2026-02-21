using SubastaArte.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubastaArte.Application.DTOs
{
    public record  EstadoUsuarioDTO
    {
        public int IdEstadoUsuario { get; set; }

        [DisplayName("Estado Usuario")]
        public string Nombre { get; set; } = string.Empty;

        public List<UsuarioDTO> Usuario { get; set; } = new();
    }
}
