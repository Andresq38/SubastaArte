using SubastaArte.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubastaArte.Application.DTOs
{
    public record CancelacionSubastaDTO
    {
        public int IdCancelacion { get; set; }

        public int IdUsuario { get; set; }

        public int IdSubasta { get; set; }

        [DisplayName("Motivo")]
        public string Motivo { get; set; } = string.Empty;

        [DisplayName("Fecha Cancelacion")]
        public DateTime FechaCancelacion { get; set; }

        [DisplayName("Usuario")]
        public UsuarioDTO IdUsuarioNavigation { get; set; } = new();

        [DisplayName("Subasta")]
        public UsuarioDTO IdSubastaNavigation { get; set; } = new();
    }
}
