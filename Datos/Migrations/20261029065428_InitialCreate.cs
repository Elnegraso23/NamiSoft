using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NamiSoft.Datos.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Checadas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NumeroEmpleado = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PrimeraChecada = table.Column<TimeSpan>(type: "time", nullable: true),
                    UltimaChecada = table.Column<TimeSpan>(type: "time", nullable: true),
                    HorasExtra = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    ModificadaManual = table.Column<bool>(type: "bit", nullable: false),
                    HorasTrabajadas = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    RetardoMinutos = table.Column<int>(type: "int", nullable: false),
                    TieneRetardo = table.Column<bool>(type: "bit", nullable: false),
                    Origen = table.Column<int>(type: "int", nullable: false),
                    CreadaEn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModificadaEn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Motivo = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Checadas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ChecadasPendientes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NumeroEmpleado = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PrimeraChecada = table.Column<TimeSpan>(type: "time", nullable: true),
                    UltimaChecada = table.Column<TimeSpan>(type: "time", nullable: true),
                    Motivo = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChecadasPendientes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ConfiguracionSistema",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EntradaManana = table.Column<TimeSpan>(type: "time", nullable: false),
                    SalidaManana = table.Column<TimeSpan>(type: "time", nullable: false),
                    ToleranciaManana = table.Column<int>(type: "int", nullable: false),
                    EntradaTarde = table.Column<TimeSpan>(type: "time", nullable: false),
                    SalidaTarde = table.Column<TimeSpan>(type: "time", nullable: false),
                    ToleranciaTarde = table.Column<int>(type: "int", nullable: false),
                    CadaRetardos = table.Column<int>(type: "int", nullable: false),
                    DescuentoRetardosMinutos = table.Column<int>(type: "int", nullable: false),
                    AplicarPrimaDominical = table.Column<bool>(type: "bit", nullable: false),
                    PorcentajePrimaDominical = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfiguracionSistema", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Empleados",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NumeroEmpleado = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Puesto = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    Departamento = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    SalarioHora = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Turno = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    TipoHorario = table.Column<int>(type: "int", nullable: false),
                    Descanso = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Empleados", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HorariosEmpleado",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NumeroEmpleado = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    DiaSemana = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: false),
                    HoraEntrada = table.Column<TimeSpan>(type: "time", nullable: false),
                    HoraSalida = table.Column<TimeSpan>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HorariosEmpleado", x => x.Id);
                });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Checadas");

            migrationBuilder.DropTable(
                name: "ChecadasPendientes");

            migrationBuilder.DropTable(
                name: "ConfiguracionSistema");

            migrationBuilder.DropTable(
                name: "Empleados");

            migrationBuilder.DropTable(
                name: "HorariosEmpleado");
        }
    }
}
