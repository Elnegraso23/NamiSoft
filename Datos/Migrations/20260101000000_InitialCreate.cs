using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NamiSoft.Datos.Migrations;

/// <summary>
/// Esquema base tal como lo creaba EnsureCreated. En BDs existentes nunca se
/// ejecuta: solo se registra en el historial (línea base sin pérdida de datos).
/// </summary>
[DbContext(typeof(NominaContext))]
[Migration("20260101000000_InitialCreate")]
public partial class InitialCreate : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Checadas",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                NumeroEmpleado = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false),
                Nombre = table.Column<string>(type: "TEXT", nullable: false),
                Fecha = table.Column<DateTime>(type: "TEXT", nullable: false),
                PrimeraChecada = table.Column<TimeSpan>(type: "TEXT", nullable: true),
                UltimaChecada = table.Column<TimeSpan>(type: "TEXT", nullable: true),
                HorasExtra = table.Column<decimal>(type: "TEXT", precision: 10, scale: 2, nullable: false),
                ModificadaManual = table.Column<bool>(type: "INTEGER", nullable: false),
                HorasTrabajadas = table.Column<decimal>(type: "TEXT", precision: 10, scale: 2, nullable: false),
                RetardoMinutos = table.Column<int>(type: "INTEGER", nullable: false),
                TieneRetardo = table.Column<bool>(type: "INTEGER", nullable: false)
            },
            constraints: table => table.PrimaryKey("PK_Checadas", x => x.Id));

        migrationBuilder.CreateTable(
            name: "ChecadasPendientes",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                NumeroEmpleado = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false),
                Nombre = table.Column<string>(type: "TEXT", nullable: false),
                Fecha = table.Column<DateTime>(type: "TEXT", nullable: false),
                PrimeraChecada = table.Column<TimeSpan>(type: "TEXT", nullable: true),
                UltimaChecada = table.Column<TimeSpan>(type: "TEXT", nullable: true),
                Motivo = table.Column<string>(type: "TEXT", nullable: false)
            },
            constraints: table => table.PrimaryKey("PK_ChecadasPendientes", x => x.Id));

        migrationBuilder.CreateTable(
            name: "ConfiguracionSistema",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                EntradaManana = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                SalidaManana = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                ToleranciaManana = table.Column<int>(type: "INTEGER", nullable: false),
                EntradaTarde = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                SalidaTarde = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                ToleranciaTarde = table.Column<int>(type: "INTEGER", nullable: false),
                CadaRetardos = table.Column<int>(type: "INTEGER", nullable: false),
                DescuentoRetardosMinutos = table.Column<int>(type: "INTEGER", nullable: false),
                AplicarPrimaDominical = table.Column<bool>(type: "INTEGER", nullable: false),
                PorcentajePrimaDominical = table.Column<decimal>(type: "TEXT", precision: 5, scale: 2, nullable: false)
            },
            constraints: table => table.PrimaryKey("PK_ConfiguracionSistema", x => x.Id));

        migrationBuilder.CreateTable(
            name: "Empleados",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                NumeroEmpleado = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false),
                Nombre = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                Puesto = table.Column<string>(type: "TEXT", maxLength: 120, nullable: false),
                Departamento = table.Column<string>(type: "TEXT", maxLength: 120, nullable: false),
                SalarioHora = table.Column<decimal>(type: "TEXT", precision: 18, scale: 2, nullable: false),
                Turno = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false),
                TipoHorario = table.Column<int>(type: "INTEGER", nullable: false),
                Descanso = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false),
                Activo = table.Column<bool>(type: "INTEGER", nullable: false)
            },
            constraints: table => table.PrimaryKey("PK_Empleados", x => x.Id));

        migrationBuilder.CreateTable(
            name: "HorariosEmpleado",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                NumeroEmpleado = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false),
                DiaSemana = table.Column<string>(type: "TEXT", maxLength: 24, nullable: false),
                HoraEntrada = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                HoraSalida = table.Column<TimeSpan>(type: "TEXT", nullable: false)
            },
            constraints: table => table.PrimaryKey("PK_HorariosEmpleado", x => x.Id));

        migrationBuilder.CreateIndex(
            name: "IX_Checadas_NumeroEmpleado_Fecha",
            table: "Checadas",
            columns: new[] { "NumeroEmpleado", "Fecha" });

        migrationBuilder.CreateIndex(
            name: "IX_ChecadasPendientes_NumeroEmpleado_Fecha",
            table: "ChecadasPendientes",
            columns: new[] { "NumeroEmpleado", "Fecha" });

        migrationBuilder.CreateIndex(
            name: "IX_Empleados_NumeroEmpleado",
            table: "Empleados",
            column: "NumeroEmpleado",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_HorariosEmpleado_NumeroEmpleado_DiaSemana",
            table: "HorariosEmpleado",
            columns: new[] { "NumeroEmpleado", "DiaSemana" });
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(name: "Checadas");
        migrationBuilder.DropTable(name: "ChecadasPendientes");
        migrationBuilder.DropTable(name: "ConfiguracionSistema");
        migrationBuilder.DropTable(name: "Empleados");
        migrationBuilder.DropTable(name: "HorariosEmpleado");
    }
}
