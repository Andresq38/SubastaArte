using SubastaArte.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubastaArte.Application.DTOs
{
    public record ResultadoSubastaDTO
    {
        public int IdResultado { get; set; }

        public int IdUsuario { get; set; }

        public int IdSubasta { get; set; }

        [DisplayName("Monto Final")]
        public decimal MontoFinal { get; set; }

        [DisplayName("Fecha Cierre")]
        public DateTime FechaCierre { get; set; }

        [DisplayName("Usuario")]
        public UsuarioDTO IdUsuarioGanadorNavigation { get; set; } = new();

        [DisplayName("Subasta")]
        public UsuarioDTO IdSubastaNavigation { get; set; } = new();
    }
}
