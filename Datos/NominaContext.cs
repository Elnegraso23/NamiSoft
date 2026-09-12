using Microsoft.EntityFrameworkCore;
using NominaPro.Modelos;

namespace NominaPro.Datos;

public class NominaContext : DbContext
{
    public DbSet<Empleado> Empleados { get; set; }

    public DbSet<ChecadaEmpleado> Checadas { get; set; }

    public DbSet<HorarioEmpleado> HorariosEmpleado { get; set; }

    public DbSet<ConfiguracionSistema> ConfiguracionSistema { get; set; }

    public DbSet<ChecadaPendiente> ChecadasPendientes { get; set; }

    public NominaContext()
    {
    }

    public NominaContext(DbContextOptions<NominaContext> options)
        : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
            optionsBuilder.UseSqlite(NominaDatabase.ConnectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Empleado>(e =>
        {
            e.HasIndex(x => x.NumeroEmpleado).IsUnique();
            e.Property(x => x.NumeroEmpleado).IsRequired().HasMaxLength(32);
            e.Property(x => x.Nombre).IsRequired().HasMaxLength(200);
            e.Property(x => x.Puesto).HasMaxLength(120);
            e.Property(x => x.Departamento).HasMaxLength(120);
            e.Property(x => x.Turno).HasMaxLength(32);
            e.Property(x => x.Descanso).HasMaxLength(32);
            e.Property(x => x.SalarioHora).HasPrecision(18, 2);
        });

        modelBuilder.Entity<ChecadaEmpleado>(e =>
        {
            e.HasIndex(x => new { x.NumeroEmpleado, x.Fecha });
            e.Property(x => x.NumeroEmpleado).IsRequired().HasMaxLength(32);
            e.Property(x => x.HorasTrabajadas).HasPrecision(10, 2);
            e.Property(x => x.HorasExtra).HasPrecision(10, 2);
            e.Property(x => x.Motivo).HasMaxLength(300);
        });

        modelBuilder.Entity<ChecadaPendiente>(e =>
        {
            e.HasIndex(x => new { x.NumeroEmpleado, x.Fecha });
            e.Property(x => x.NumeroEmpleado).IsRequired().HasMaxLength(32);
        });

        modelBuilder.Entity<HorarioEmpleado>(e =>
        {
            e.HasIndex(x => new { x.NumeroEmpleado, x.DiaSemana });
            e.Property(x => x.NumeroEmpleado).IsRequired().HasMaxLength(32);
            e.Property(x => x.DiaSemana).IsRequired().HasMaxLength(24);
        });

        modelBuilder.Entity<ConfiguracionSistema>(e =>
        {
            e.Property(x => x.PorcentajePrimaDominical).HasPrecision(5, 2);
        });
    }
}
