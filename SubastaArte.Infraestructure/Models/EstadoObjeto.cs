using System;
using System.Collections.Generic;

namespace SubastaArte.Infraestructure.Models;

public partial class EstadoObjeto
{
    public int IdEstadoObjeto { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Objeto> Objeto { get; set; } = new List<Objeto>();
}
