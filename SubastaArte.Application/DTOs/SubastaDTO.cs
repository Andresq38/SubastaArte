using SubastaArte.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubastaArte.Application.DTOs
{
    public record SubastaDTO
    {
        [DisplayName("ID Subasta")]
        public int IdSubasta { get; set; }

        public int IdObjeto { get; set; }

        public DateTime FechaInicio { get; set; }

        public DateTime FechaCierre { get; set; }

        public decimal PrecioBase { get; set; }

        public decimal IncrementoMinimo { get; set; }

        public string Nombre { get; set; } = string.Empty;

        public int IdEstadoSubasta { get; set; }

        public int IdCreador { get; set; }

        public int IdVendedor { get; set; }

        public List<CancelacionSubastaDTO> CancelacionSubasta { get; set; } = new();

        public Usuario IdCreadorNavigation { get; set; } = new();

        [DisplayName("Estado Subasta")]
        public  EstadoSubasta IdEstadoSubastaNavigation { get; set; } = new();

        [DisplayName("Imagen")]
        public Objeto IdObjetoNavigation { get; set; } = new();

        public Usuario IdVendedorNavigation { get; set; } = new();

        public List<PagoDTO> Pago { get; set; } = new();

        //[DisplayName("Cantidad Pujas")]
        //public List<PujaDTO> Puja { get; set; } = new();

        public List<ResultadoSubastaDTO> ResultadoSubasta { get; set; } = new();

        public int PujasSubasta { get; set; }


    }
}
