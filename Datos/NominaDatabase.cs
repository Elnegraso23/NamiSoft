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
}
