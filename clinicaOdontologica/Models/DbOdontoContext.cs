using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace clinicaOdontologica.Models;

public partial class DbOdontoContext : DbContext
{
    public DbOdontoContext()
    {
    }

    public DbOdontoContext(DbContextOptions<DbOdontoContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Agendamento> Agendamentos { get; set; }

    public virtual DbSet<Paciente> Pacientes { get; set; }

    public virtual DbSet<Procedimento> Procedimentos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConexaoSqlServer");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Agendamento>(entity =>
        {
            entity.HasKey(e => e.Codigo).HasName("PK__Agendame__06370DAD451EF0B4");

            entity.ToTable("Agendamento");

            entity.Property(e => e.DataHora).HasColumnType("datetime");
            entity.Property(e => e.StatusAgendamento)
                .HasMaxLength(30)
                .IsUnicode(false);

            entity.HasOne(d => d.Paciente).WithMany(p => p.Agendamentos)
                .HasForeignKey(d => d.PacienteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Agendamento_Paciente");

            entity.HasOne(d => d.Procedimento).WithMany(p => p.Agendamentos)
                .HasForeignKey(d => d.ProcedimentoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Agendamento_Procedimento");
        });

        modelBuilder.Entity<Paciente>(entity =>
        {
            entity.HasKey(e => e.Codigo).HasName("PK__Paciente__06370DADBEFEA983");

            entity.ToTable("Paciente");

            entity.HasIndex(e => e.Cpf, "UQ__Paciente__C1FF9309F6E366A9").IsUnique();

            entity.Property(e => e.Cpf)
                .HasMaxLength(14)
                .IsUnicode(false);
            entity.Property(e => e.Nome)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Telefone)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Procedimento>(entity =>
        {
            entity.HasKey(e => e.Codigo).HasName("PK__Procedim__06370DAD4B70CA0C");

            entity.ToTable("Procedimento");

            entity.Property(e => e.Descricao)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Valor).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Paciente).WithMany(p => p.Procedimentos)
                .HasForeignKey(d => d.PacienteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Procedimento_Paciente");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
