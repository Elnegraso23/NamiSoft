using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NominaPro.Datos.Migrations;

/// <summary>
/// Trazabilidad de checadas: origen + fechas de creación/modificación + motivo.
/// En datos existentes, el origen se reconstruye: manual si fue modificada,
/// importación Excel en caso contrario; la creación se aproxima a la fecha.
/// </summary>
[DbContext(typeof(NominaContext))]
[Migration("20260912120000_ChecadaOrigenAuditoria")]
public partial class ChecadaOrigenAuditoria : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<int>(
            name: "Origen",
            table: "Checadas",
            type: "INTEGER",
            nullable: false,
            defaultValue: 1);

        migrationBuilder.AddColumn<DateTime>(
            name: "CreadaEn",
            table: "Checadas",
            type: "TEXT",
            nullable: false,
            defaultValue: new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Unspecified));

        migrationBuilder.AddColumn<DateTime>(
            name: "ModificadaEn",
            table: "Checadas",
            type: "TEXT",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "Motivo",
            table: "Checadas",
            type: "TEXT",
            maxLength: 300,
            nullable: true);

        migrationBuilder.Sql(
            "UPDATE \"Checadas\" SET \"Origen\" = CASE WHEN \"ModificadaManual\" = 1 THEN 0 ELSE 2 END, " +
            "\"CreadaEn\" = \"Fecha\";");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(name: "Origen", table: "Checadas");
        migrationBuilder.DropColumn(name: "CreadaEn", table: "Checadas");
        migrationBuilder.DropColumn(name: "ModificadaEn", table: "Checadas");
        migrationBuilder.DropColumn(name: "Motivo", table: "Checadas");
    }
}
