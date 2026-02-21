using SubastaArte.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubastaArte.Application.DTOs
{
    public record  ObjetoDTO
    {
        [DisplayName("ID Objeto")]
        public int IdObjeto { get; set; }

        [Display(Name = "Nombre Objeto")]
        public string Nombre { get; set; } = string.Empty;

        [DisplayName("Descripcion Objeto")]
        public string Descripcion { get; set; } = string.Empty;

        [DisplayName("Condicion Objeto")]
        public string Condicion { get; set; } = string.Empty;

        [DisplayName("ID Estado Objeto")]
        public int IdEstadoObjeto { get; set; }

        [DisplayName("Fecha Registro Objeto")]
        public DateTime FechaRegistro { get; set; }

        [DisplayName("ID Vendedor")]
        public int IdVendedor { get; set; }

        [DisplayName("Estado Objeto")]
        public EstadoObjeto IdEstadoObjetoNavigation { get; set; } = new();

        [DisplayName("Estado Objeto")]
        public Usuario IdVendedorNavigation { get; set; } = new();

        public List<ImagenDTO> Imagen { get; set; } = new();

        [Display(Name = "Categoría")]
        public List<CategoriaDTO> IdCategoria { get; set; } = new();

        [Display(Name = "Subasta")]
        public List<SubastaDTO> Subasta { get; set; } = new();



    }
}
