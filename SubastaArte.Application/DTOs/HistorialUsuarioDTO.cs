using SubastaArte.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubastaArte.Application.DTOs
{
    public record HistorialUsuarioDTO
    {
        public int IdHistorial { get; set; }

        [DisplayName("Descripcion")]
        public string Descripcion { get; set; } = string.Empty;

        [DisplayName("Tipo Evento")]
        public string TipoEvento { get; set; } = string.Empty;

        [DisplayName("Fecha Evento")]
        public DateTime FechaEvento { get; set; }

        public int IdUsuario { get; set; }

        [DisplayName("Usuario")]
        public UsuarioDTO IdUsuarioNavigation { get; set; } = new();
    }
}
