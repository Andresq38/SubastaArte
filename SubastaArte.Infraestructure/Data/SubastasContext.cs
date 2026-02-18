using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using SubastaArte.Infraestructure.Models;

namespace SubastaArte.Infraestructure.Data;

public partial class SubastasContext : DbContext
{
    public SubastasContext(DbContextOptions<SubastasContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CancelacionSubasta> CancelacionSubasta { get; set; }

    public virtual DbSet<Categoria> Categoria { get; set; }

    public virtual DbSet<EstadoObjeto> EstadoObjeto { get; set; }

    public virtual DbSet<EstadoSubasta> EstadoSubasta { get; set; }

    public virtual DbSet<EstadoUsuario> EstadoUsuario { get; set; }

    public virtual DbSet<HistorialUsuario> HistorialUsuario { get; set; }

    public virtual DbSet<Imagen> Imagen { get; set; }

    public virtual DbSet<Objeto> Objeto { get; set; }

    public virtual DbSet<Pago> Pago { get; set; }

    public virtual DbSet<Puja> Puja { get; set; }

    public virtual DbSet<ResultadoSubasta> ResultadoSubasta { get; set; }

    public virtual DbSet<Rol> Rol { get; set; }

    public virtual DbSet<Subasta> Subasta { get; set; }

    public virtual DbSet<Usuario> Usuario { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CancelacionSubasta>(entity =>
        {
            entity.HasKey(e => e.IdCancelacion);

            entity.Property(e => e.FechaCancelacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Motivo)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.IdSubastaNavigation).WithMany(p => p.CancelacionSubasta)
                .HasForeignKey(d => d.IdSubasta)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cancelacion_Subasta");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.CancelacionSubasta)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cancelacion_Usuario");
        });

        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.HasKey(e => e.IdCategoria);

            entity.Property(e => e.Estado).HasDefaultValue(true);
            entity.Property(e => e.Nombre)
                .HasMaxLength(80)
                .IsUnicode(false);
        });

        modelBuilder.Entity<EstadoObjeto>(entity =>
        {
            entity.HasKey(e => e.IdEstadoObjeto);

            entity.Property(e => e.Nombre)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<EstadoSubasta>(entity =>
        {
            entity.HasKey(e => e.IdEstadoSubasta);

            entity.Property(e => e.Nombre)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<EstadoUsuario>(entity =>
        {
            entity.HasKey(e => e.IdEstadoUsuario);

            entity.Property(e => e.Nombre)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<HistorialUsuario>(entity =>
        {
            entity.HasKey(e => e.IdHistorial);

            entity.Property(e => e.Descripcion)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.FechaEvento)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.TipoEvento)
                .HasMaxLength(30)
                .IsUnicode(false);

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.HistorialUsuario)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Historial_Usuario");
        });

        modelBuilder.Entity<Imagen>(entity =>
        {
            entity.HasKey(e => e.IdImagen);

            entity.Property(e => e.RutaImagen)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.IdObjetoNavigation).WithMany(p => p.Imagen)
                .HasForeignKey(d => d.IdObjeto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Imagen_Objeto");
        });

        modelBuilder.Entity<Objeto>(entity =>
        {
            entity.HasKey(e => e.IdObjeto);

            entity.Property(e => e.Condicion)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Descripcion)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.FechaRegistro)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Nombre)
                .HasMaxLength(150)
                .IsUnicode(false);

            entity.HasOne(d => d.IdEstadoObjetoNavigation).WithMany(p => p.Objeto)
                .HasForeignKey(d => d.IdEstadoObjeto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Objeto_Estado");

            entity.HasOne(d => d.IdVendedorNavigation).WithMany(p => p.Objeto)
                .HasForeignKey(d => d.IdVendedor)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Objeto_Usuario");

            entity.HasMany(d => d.IdCategoria).WithMany(p => p.IdObjeto)
                .UsingEntity<Dictionary<string, object>>(
                    "ObjetoCategoria",
                    r => r.HasOne<Categoria>().WithMany()
                        .HasForeignKey("IdCategoria")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_ObjetoCategoria_Categoria"),
                    l => l.HasOne<Objeto>().WithMany()
                        .HasForeignKey("IdObjeto")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_ObjetoCategoria_Objeto"),
                    j =>
                    {
                        j.HasKey("IdObjeto", "IdCategoria").IsClustered(false);
                    });
        });

        modelBuilder.Entity<Pago>(entity =>
        {
            entity.HasKey(e => e.IdPago);

            entity.Property(e => e.EstadoPago)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.FechaPago).HasColumnType("datetime");

            entity.HasOne(d => d.IdSubastaNavigation).WithMany(p => p.Pago)
                .HasForeignKey(d => d.IdSubasta)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Pago_Subasta");
        });

        modelBuilder.Entity<Puja>(entity =>
        {
            entity.HasKey(e => e.IdPuja);

            entity.Property(e => e.FechaHora)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Monto).HasColumnType("numeric(18, 2)");

            entity.HasOne(d => d.IdSubastaNavigation).WithMany(p => p.Puja)
                .HasForeignKey(d => d.IdSubasta)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Puja_Subasta");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Puja)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Puja_Usuario");
        });

        modelBuilder.Entity<ResultadoSubasta>(entity =>
        {
            entity.HasKey(e => e.IdResultado);

            entity.Property(e => e.FechaCierre).HasColumnType("datetime");
            entity.Property(e => e.MontoFinal).HasColumnType("numeric(18, 2)");

            entity.HasOne(d => d.IdSubastaNavigation).WithMany(p => p.ResultadoSubasta)
                .HasForeignKey(d => d.IdSubasta)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Resultado_Subasta");

            entity.HasOne(d => d.IdUsuarioGanadorNavigation).WithMany(p => p.ResultadoSubasta)
                .HasForeignKey(d => d.IdUsuarioGanador)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Resultado_Usuario");
        });

        modelBuilder.Entity<Rol>(entity =>
        {
            entity.HasKey(e => e.IdRol);

            entity.Property(e => e.Descripcion)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Subasta>(entity =>
        {
            entity.HasKey(e => e.IdSubasta);

            entity.Property(e => e.FechaCierre).HasColumnType("datetime");
            entity.Property(e => e.FechaInicio).HasColumnType("datetime");
            entity.Property(e => e.IncrementoMinimo).HasColumnType("numeric(18, 2)");
            entity.Property(e => e.PrecioBase).HasColumnType("numeric(18, 2)");

            entity.HasOne(d => d.IdCreadorNavigation).WithMany(p => p.SubastaIdCreadorNavigation)
                .HasForeignKey(d => d.IdCreador)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Subasta_Creador");

            entity.HasOne(d => d.IdEstadoSubastaNavigation).WithMany(p => p.Subasta)
                .HasForeignKey(d => d.IdEstadoSubasta)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Subasta_Estado");

            entity.HasOne(d => d.IdObjetoNavigation).WithMany(p => p.Subasta)
                .HasForeignKey(d => d.IdObjeto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Subasta_Objeto");

            entity.HasOne(d => d.IdVendedorNavigation).WithMany(p => p.SubastaIdVendedorNavigation)
                .HasForeignKey(d => d.IdVendedor)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Subasta_Vendedor");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario);

            entity.HasIndex(e => e.Email, "UQ_Usuario_Email").IsUnique();

            entity.Property(e => e.Email)
                .HasMaxLength(80)
                .IsUnicode(false);
            entity.Property(e => e.FechaRegistro)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.NombreCompleto)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(200)
                .IsUnicode(false);

            entity.HasOne(d => d.IdEstadoUsuarioNavigation).WithMany(p => p.Usuario)
                .HasForeignKey(d => d.IdEstadoUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Usuario_Estado");

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.Usuario)
                .HasForeignKey(d => d.IdRol)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Usuario_Rol");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
