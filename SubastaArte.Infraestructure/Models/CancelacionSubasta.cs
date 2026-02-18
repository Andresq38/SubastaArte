using System;
using System.Collections.Generic;

namespace SubastaArte.Infraestructure.Models;

public partial class CancelacionSubasta
{
    public int IdCancelacion { get; set; }

    public int IdSubasta { get; set; }

    public string Motivo { get; set; } = null!;

    public DateTime FechaCancelacion { get; set; }

    public int IdUsuario { get; set; }

    public virtual Subasta IdSubastaNavigation { get; set; } = null!;

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
