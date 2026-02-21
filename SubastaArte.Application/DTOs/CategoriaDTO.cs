using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubastaArte.Application.DTOs
{
    public record CategoriaDTO
    {
        public int IdCategoria { get; set; }

        public string Nombre { get; set; } = string.Empty;

        public bool Estado { get; set; }

        public List<ObjetoDTO> IdObjeto { get; set; } = new();
    }
}
