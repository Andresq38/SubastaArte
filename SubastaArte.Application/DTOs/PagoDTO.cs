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
    public record PagoDTO
    {
        public int IdPago { get; set; }

        [DisplayName("Estado Pago")]
        public string EstadoPago { get; set; } = string.Empty;

        [DisplayName("Fecha Pago")]
        public DateTime FechaPago { get; set; }

        public int IdSubasta { get; set; }

        [DisplayName("Subasta Objeto")]
        public SubastaDTO IdSubastaNavigation { get; set; } = new();
    }
}
