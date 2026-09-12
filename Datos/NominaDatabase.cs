using Microsoft.EntityFrameworkCore;

namespace NominaPro.Datos;

/// <summary>
/// Único punto de acceso a la base de datos. Ninguna otra capa conoce
/// rutas, cadenas de conexión ni detalles del proveedor.
/// Para cambiar de proveedor (p. ej. PostgreSQL) se modifica solo este archivo.
/// </summary>
public static class NominaDatabase
{
    /// <summary>
    /// Ruta actual: archivo local junto al ejecutable (una sola terminal, offline).
    /// Futuro: mover a carpeta de datos de aplicación copiando el archivo existente.
    /// </summary>
    public static string DatabasePath { get; set; } = "Nomina.db";

    public static string ConnectionString => $"Data Source={DatabasePath}";

    public static NominaContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<NominaContext>()
            .UseSqlite(ConnectionString)
            .Options;
        return new NominaContext(options);
    }

    public static void EnsureCreated()
    {
        using var db = CreateContext();
        db.Database.EnsureCreated();
    }

    private const string InitialMigrationId = "20260101000000_InitialCreate";
    private const string AuditMigrationId = "20260912120000_ChecadaOrigenAuditoria";
    private const string ProductVersion = "8.0.11";

    /// <summary>
    /// Aplica migraciones pendientes sin destruir datos. Las BDs creadas con
    /// EnsureCreated (sin historial) se registran como línea base y solo
    /// reciben las migraciones que les falten.
    /// </summary>
    public static void EnsureMigrated()
    {
        using var db = CreateContext();
        var connection = db.Database.GetDbConnection();
        connection.Open();
        try
        {
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT count(*) FROM sqlite_master WHERE type='table' AND name='__EFMigrationsHistory'";
            bool hasHistory = Convert.ToInt32(cmd.ExecuteScalar()!) > 0;
            if (hasHistory)
            {
                connection.Close();
                db.Database.Migrate();
                return;
            }

            cmd.CommandText = "SELECT count(*) FROM sqlite_master WHERE type='table' AND name='Checadas'";
            bool hasTables = Convert.ToInt32(cmd.ExecuteScalar()!) > 0;
            if (!hasTables)
            {
                connection.Close();
                db.Database.Migrate();
                return;
            }

            cmd.CommandText = "SELECT count(*) FROM pragma_table_info('Checadas') WHERE name='Origen'";
            bool hasAuditColumns = Convert.ToInt32(cmd.ExecuteScalar()!) > 0;
            connection.Close();

            db.Database.ExecuteSqlRaw(
                "CREATE TABLE IF NOT EXISTS \"__EFMigrationsHistory\" " +
                "(\"MigrationId\" TEXT NOT NULL CONSTRAINT \"PK___EFMigrationsHistory\" PRIMARY KEY, " +
                "\"ProductVersion\" TEXT NOT NULL)");
            db.Database.ExecuteSqlRaw(
                "INSERT OR IGNORE INTO \"__EFMigrationsHistory\" (\"MigrationId\", \"ProductVersion\") VALUES ('" +
                InitialMigrationId + "', '" + ProductVersion + "')");
            if (hasAuditColumns)
                db.Database.ExecuteSqlRaw(
                    "INSERT OR IGNORE INTO \"__EFMigrationsHistory\" (\"MigrationId\", \"ProductVersion\") VALUES ('" +
                    AuditMigrationId + "', '" + ProductVersion + "')");

            db.Database.Migrate();
        }
        finally
        {
            if (connection.State == System.Data.ConnectionState.Open)
                connection.Close();
        }
    }
}
