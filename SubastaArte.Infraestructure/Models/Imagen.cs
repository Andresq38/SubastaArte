using System;
using System.Collections.Generic;

namespace SubastaArte.Infraestructure.Models;

public partial class Imagen
{
    public int IdImagen { get; set; }

    public int IdObjeto { get; set; }

    public byte[] Foto { get; set; } = null!;

    public virtual Objeto IdObjetoNavigation { get; set; } = null!;
}
