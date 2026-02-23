using SubastaArte.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubastaArte.Application.DTOs
{
    public record EstadoSubastaDTO
    {
        public int IdEstadoSubasta { get; set; }

        [DisplayName("Estado Subasta")]
        public string Nombre { get; set; } = string.Empty;

        public List<SubastaDTO> Subasta { get; set; } = new();
    }
}
