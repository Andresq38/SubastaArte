using System.ComponentModel.DataAnnotations;

namespace SubastaArte.web.ViewModels
{
    public class PagoListadoItemViewModel
    {
        public int IdSubasta { get; set; }
        public string NombreSubasta { get; set; } = string.Empty;
        public string NombreGanador { get; set; } = string.Empty;
        public decimal MontoFinal { get; set; }
        public DateTime FechaCierre { get; set; }
        public string EstadoPago { get; set; } = string.Empty;
    }
}
