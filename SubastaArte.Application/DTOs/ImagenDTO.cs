using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubastaArte.Application.DTOs
{
    public record ImagenDTO
    {
        public int IdImagen { get; set; }

        public int IdObjeto { get; set; }


        [Display(Name = "Imagen")]
        public byte[] Foto { get; set; } = Array.Empty<byte>();

        public ObjetoDTO IdObjetoNavigation { get; set; } = new();
    }
}
