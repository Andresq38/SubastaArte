using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubastaArte.Application.DTOs
{
    public record UsuarioDTO
    {
        public int IdUsuario { get; set; }

        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public string Nombre { get; set; } = string.Empty;

        public string Apellido1 { get; set; } = string.Empty;

        public string Apellido2 { get; set; } = string.Empty;

        public int IdRol { get; set; }

        public int IdEstadoUsuario { get; set; }

        public DateTime FechaRegistro { get; set; }

        public RolDTO IdRolNavigation { get; set; } = new();

        public EstadoUsuarioDTO IdEstadoUsuarioNavigation { get; set; } = new();

    }
}
