using System;
using System.Collections.Generic;

namespace SubastaArte.Infraestructure.Models;

public partial class Subasta
{
    public int IdSubasta { get; set; }

    public int IdObjeto { get; set; }

    public DateTime FechaInicio { get; set; }

    public DateTime FechaCierre { get; set; }

    public decimal PrecioBase { get; set; }

    public decimal IncrementoMinimo { get; set; }

    public string Nombre { get; set; } = null!;

    public int IdEstadoSubasta { get; set; }

    public int IdCreador { get; set; }

    public int IdVendedor { get; set; }

    public virtual ICollection<CancelacionSubasta> CancelacionSubasta { get; set; } = new List<CancelacionSubasta>();

    public virtual Usuario IdCreadorNavigation { get; set; } = null!;

    public virtual EstadoSubasta IdEstadoSubastaNavigation { get; set; } = null!;

    public virtual Objeto IdObjetoNavigation { get; set; } = null!;

    public virtual Usuario IdVendedorNavigation { get; set; } = null!;

    public virtual ICollection<Pago> Pago { get; set; } = new List<Pago>();

    public virtual ICollection<Puja> Puja { get; set; } = new List<Puja>();

    public virtual ICollection<ResultadoSubasta> ResultadoSubasta { get; set; } = new List<ResultadoSubasta>();
}
