using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

#nullable disable

namespace NamiSoft.Datos.Migrations;

[DbContext(typeof(NominaContext))]
public partial class NominaContextModelSnapshot : ModelSnapshot
{
    protected override void BuildModel(ModelBuilder modelBuilder)
    {
#pragma warning disable 612, 618
        modelBuilder.HasAnnotation("ProductVersion", "8.0.11");

        modelBuilder.Entity("NamiSoft.Modelos.ChecadaEmpleado", b =>
        {
            b.Property<int>("Id").ValueGeneratedOnAdd().HasColumnType("INTEGER");
            b.Property<bool>("TieneRetardo").HasColumnType("INTEGER");
            b.Property<TimeSpan?>("UltimaChecada").HasColumnType("TEXT");
            b.Property<TimeSpan?>("PrimeraChecada").HasColumnType("TEXT");
            b.Property<int>("RetardoMinutos").HasColumnType("INTEGER");
            b.Property<string>("NumeroEmpleado").IsRequired().HasMaxLength(32).HasColumnType("TEXT");
            b.Property<string>("Nombre").IsRequired().HasColumnType("TEXT");
            b.Property<string>("Motivo").HasMaxLength(300).HasColumnType("TEXT");
            b.Property<DateTime?>("ModificadaEn").HasColumnType("TEXT");
            b.Property<bool>("ModificadaManual").HasColumnType("INTEGER");
            b.Property<decimal>("HorasTrabajadas").HasPrecision(10, 2).HasColumnType("TEXT");
            b.Property<decimal>("HorasExtra").HasPrecision(10, 2).HasColumnType("TEXT");
            b.Property<DateTime>("Fecha").HasColumnType("TEXT");
            b.Property<DateTime>("CreadaEn").HasColumnType("TEXT");
            b.Property<int>("Origen").HasColumnType("INTEGER");
            b.HasKey("Id");
            b.HasIndex("NumeroEmpleado", "Fecha").HasDatabaseName("IX_Checadas_NumeroEmpleado_Fecha");
            b.ToTable("Checadas");
        });

        modelBuilder.Entity("NamiSoft.Modelos.ChecadaPendiente", b =>
        {
            b.Property<int>("Id").ValueGeneratedOnAdd().HasColumnType("INTEGER");
            b.Property<TimeSpan?>("UltimaChecada").HasColumnType("TEXT");
            b.Property<TimeSpan?>("PrimeraChecada").HasColumnType("TEXT");
            b.Property<string>("NumeroEmpleado").IsRequired().HasMaxLength(32).HasColumnType("TEXT");
            b.Property<string>("Nombre").IsRequired().HasColumnType("TEXT");
            b.Property<string>("Motivo").IsRequired().HasColumnType("TEXT");
            b.Property<DateTime>("Fecha").HasColumnType("TEXT");
            b.HasKey("Id");
            b.HasIndex("NumeroEmpleado", "Fecha").HasDatabaseName("IX_ChecadasPendientes_NumeroEmpleado_Fecha");
            b.ToTable("ChecadasPendientes");
        });

        modelBuilder.Entity("NamiSoft.Modelos.ConfiguracionSistema", b =>
        {
            b.Property<int>("Id").ValueGeneratedOnAdd().HasColumnType("INTEGER");
            b.Property<TimeSpan>("SalidaTarde").HasColumnType("TEXT");
            b.Property<TimeSpan>("SalidaManana").HasColumnType("TEXT");
            b.Property<decimal>("PorcentajePrimaDominical").HasPrecision(5, 2).HasColumnType("TEXT");
            b.Property<int>("ToleranciaTarde").HasColumnType("INTEGER");
            b.Property<int>("ToleranciaManana").HasColumnType("INTEGER");
            b.Property<TimeSpan>("EntradaTarde").HasColumnType("TEXT");
            b.Property<TimeSpan>("EntradaManana").HasColumnType("TEXT");
            b.Property<int>("DescuentoRetardosMinutos").HasColumnType("INTEGER");
            b.Property<int>("CadaRetardos").HasColumnType("INTEGER");
            b.Property<bool>("AplicarPrimaDominical").HasColumnType("INTEGER");
            b.HasKey("Id");
            b.ToTable("ConfiguracionSistema");
        });

        modelBuilder.Entity("NamiSoft.Modelos.Empleado", b =>
        {
            b.Property<int>("Id").ValueGeneratedOnAdd().HasColumnType("INTEGER");
            b.Property<string>("Turno").IsRequired().HasMaxLength(32).HasColumnType("TEXT");
            b.Property<int>("TipoHorario").HasColumnType("INTEGER");
            b.Property<decimal>("SalarioHora").HasPrecision(18, 2).HasColumnType("TEXT");
            b.Property<string>("Puesto").IsRequired().HasMaxLength(120).HasColumnType("TEXT");
            b.Property<string>("NumeroEmpleado").IsRequired().HasMaxLength(32).HasColumnType("TEXT");
            b.Property<string>("Nombre").IsRequired().HasMaxLength(200).HasColumnType("TEXT");
            b.Property<string>("Descanso").IsRequired().HasMaxLength(32).HasColumnType("TEXT");
            b.Property<string>("Departamento").IsRequired().HasMaxLength(120).HasColumnType("TEXT");
            b.Property<bool>("Activo").HasColumnType("INTEGER");
            b.HasKey("Id");
            b.HasIndex("NumeroEmpleado").IsUnique().HasDatabaseName("IX_Empleados_NumeroEmpleado");
            b.ToTable("Empleados");
        });

        modelBuilder.Entity("NamiSoft.Modelos.HorarioEmpleado", b =>
        {
            b.Property<int>("Id").ValueGeneratedOnAdd().HasColumnType("INTEGER");
            b.Property<string>("NumeroEmpleado").IsRequired().HasMaxLength(32).HasColumnType("TEXT");
            b.Property<TimeSpan>("HoraSalida").HasColumnType("TEXT");
            b.Property<TimeSpan>("HoraEntrada").HasColumnType("TEXT");
            b.Property<string>("DiaSemana").IsRequired().HasMaxLength(24).HasColumnType("TEXT");
            b.HasKey("Id");
            b.HasIndex("NumeroEmpleado", "DiaSemana").HasDatabaseName("IX_HorariosEmpleado_NumeroEmpleado_DiaSemana");
            b.ToTable("HorariosEmpleado");
        });
#pragma warning restore 612, 618
    }
}
