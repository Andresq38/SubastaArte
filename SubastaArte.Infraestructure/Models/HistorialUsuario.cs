using System;
using System.Collections.Generic;

namespace SubastaArte.Infraestructure.Models;

public partial class HistorialUsuario
{
    public int IdHistorial { get; set; }

    public int IdUsuario { get; set; }

    public string TipoEvento { get; set; } = null!;

    public string Descripcion { get; set; } = null!;

    public DateTime FechaEvento { get; set; }

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
