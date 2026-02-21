using SubastaArte.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubastaArte.Application.DTOs
{
    public record RolDTO
    {
        public int IdRol { get; set; }

        [DisplayName("Rol Uuario")]
        public string Descripcion { get; set; } = string.Empty;

        public List<UsuarioDTO> Usuario { get; set; } = new();
    }
}
