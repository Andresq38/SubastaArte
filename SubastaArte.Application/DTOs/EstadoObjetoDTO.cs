using SubastaArte.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubastaArte.Application.DTOs
{
    public record EstadoObjetoDTO
    {
        public int IdEstadoUsuario { get; set; }

        [DisplayName("Estado Objeto")]
        public string Nombre { get; set; } = string.Empty;

        public List<ObjetoDTO> Objeto { get; set; } = new();

    }
}
