using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Api.Models;

public partial class EscuelaContext : DbContext
{
    public EscuelaContext()
    {
    }

    public EscuelaContext(DbContextOptions<EscuelaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Server=10.120.2.217;Database=escuela;Username=administrador;Password=Pass123!");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("uuid-ossp");

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("roles_pkey");

            entity.ToTable("roles");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("id");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("usuarios_pkey");

            entity.ToTable("usuarios");

            entity.HasIndex(e => e.Email, "usuarios_email_key").IsUnique();

            entity.HasIndex(e => e.NombreUsuario, "usuarios_nombre_usuario_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Contraseña)
                .HasMaxLength(100)
                .HasColumnName("contraseña");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .HasColumnName("nombre");
            entity.Property(e => e.NombreUsuario)
                .HasMaxLength(50)
                .HasColumnName("nombre_usuario");

            entity.HasMany(d => d.Rols).WithMany(p => p.Usuarios)
                .UsingEntity<Dictionary<string, object>>(
                    "UsuariosRole",
                    r => r.HasOne<Role>().WithMany()
                        .HasForeignKey("RolId")
                        .HasConstraintName("usuarios_roles_rol_id_fkey"),
                    l => l.HasOne<Usuario>().WithMany()
                        .HasForeignKey("UsuarioId")
                        .HasConstraintName("usuarios_roles_usuario_id_fkey"),
                    j =>
                    {
                        j.HasKey("UsuarioId", "RolId").HasName("usuarios_roles_pkey");
                        j.ToTable("usuarios_roles");
                        j.IndexerProperty<int>("UsuarioId").HasColumnName("usuario_id");
                        j.IndexerProperty<Guid>("RolId").HasColumnName("rol_id");
                    });
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
