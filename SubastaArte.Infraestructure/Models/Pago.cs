using System;
using System.Collections.Generic;

namespace SubastaArte.Infraestructure.Models;

public partial class Pago
{
    public int IdPago { get; set; }

    public int IdSubasta { get; set; }

    public string EstadoPago { get; set; } = null!;

    public DateTime? FechaPago { get; set; }

    public virtual Subasta IdSubastaNavigation { get; set; } = null!;
}
