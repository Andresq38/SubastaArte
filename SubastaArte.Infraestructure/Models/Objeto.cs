using System;
using System.Collections.Generic;

namespace SubastaArte.Infraestructure.Models;

public partial class Objeto
{
    public int IdObjeto { get; set; }

    public string Nombre { get; set; } = null!;

    public string Descripcion { get; set; } = null!;

    public string Condicion { get; set; } = null!;

    public int IdEstadoObjeto { get; set; }

    public DateTime FechaRegistro { get; set; }

    public int IdVendedor { get; set; }

    public virtual EstadoObjeto IdEstadoObjetoNavigation { get; set; } = null!;

    public virtual Usuario IdVendedorNavigation { get; set; } = null!;

    public virtual ICollection<Imagen> Imagen { get; set; } = new List<Imagen>();

    public virtual ICollection<Subasta> Subasta { get; set; } = new List<Subasta>();

    public virtual ICollection<Categoria> IdCategoria { get; set; } = new List<Categoria>();
}
