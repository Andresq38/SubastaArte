using SubastaArte.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubastaArte.Application.DTOs
{
    public record PujaDTO
    {
        public int IdPuja { get; set; }

        [DisplayName("Monto")]
        public decimal Monto { get; set; }

        [DisplayName("Fecha Puja")]
        public DateTime FechaHora { get; set; }

        public int IdUsuario { get; set; }

        public int IdSubasta { get; set; }

        [DisplayName("Usuario")]
        public UsuarioDTO IdUsuarioNavigation { get; set; } = new();

        [DisplayName("Subasta")]
        public UsuarioDTO IdSubastaNavigation { get; set; } = new();
    }
}
