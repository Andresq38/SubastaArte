using System;
using System.Collections.Generic;

namespace SubastaArte.Infraestructure.Models;

public partial class Usuario
{
    public int IdUsuario { get; set; }

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public string Apellido1 { get; set; } = null!;

    public string? Apellido2 { get; set; }

    public int IdRol { get; set; }

    public int IdEstadoUsuario { get; set; }

    public DateTime FechaRegistro { get; set; }

    public virtual ICollection<CancelacionSubasta> CancelacionSubasta { get; set; } = new List<CancelacionSubasta>();

    public virtual ICollection<HistorialUsuario> HistorialUsuario { get; set; } = new List<HistorialUsuario>();

    public virtual EstadoUsuario IdEstadoUsuarioNavigation { get; set; } = null!;

    public virtual Rol IdRolNavigation { get; set; } = null!;

    public virtual ICollection<Objeto> Objeto { get; set; } = new List<Objeto>();

    public virtual ICollection<Puja> Puja { get; set; } = new List<Puja>();

    public virtual ICollection<ResultadoSubasta> ResultadoSubasta { get; set; } = new List<ResultadoSubasta>();

    public virtual ICollection<Subasta> SubastaIdCreadorNavigation { get; set; } = new List<Subasta>();

    public virtual ICollection<Subasta> SubastaIdVendedorNavigation { get; set; } = new List<Subasta>();
}
