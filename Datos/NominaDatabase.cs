using Microsoft.EntityFrameworkCore;

namespace NamiSoft.Datos;

/// <summary>
/// Único punto de acceso a la base de datos. Ninguna otra capa conoce
/// cadenas de conexión ni detalles del proveedor.
/// Motor: Microsoft SQL Server 2022 (instancia local en contenedor).
/// Para cambiar de servidor o de base basta con definir NAMISOFT_DB_CONNECTION
/// o las variables NAMISOFT_DB_SERVER / NAMISOFT_DB_NAME / NAMISOFT_DB_PASSWORD.
/// </summary>
public static class NominaDatabase
{
    private const string ServidorPorDefecto = "localhost,1433";
    private const string BasePorDefecto = "NamiSoft";

    private static string? _connectionString;

    public static string ConnectionString => _connectionString ??= ResolverCadenaConexion();

    public static NominaContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<NominaContext>()
            .UseSqlServer(ConnectionString)
            .Options;
        return new NominaContext(options);
    }

    /// <summary>
    /// Aplica las migraciones pendientes. Crea la base si no existe y actualiza
    /// el esquema sin destruir datos.
    /// </summary>
    public static void EnsureMigrated()
    {
        using var db = CreateContext();
        db.Database.Migrate();
    }

    /// <summary>
    /// Resuelve la cadena de conexión en este orden:
    /// 1) NAMISOFT_DB_CONNECTION (cadena completa).
    /// 2) Servidor/base por defecto + password de NAMISOFT_DB_PASSWORD.
    /// 3) Servidor/base por defecto + password leída de un archivo local
    ///    (mssql.pass junto al ejecutable o ~/.config/namisoft/mssql.pass).
    /// La password nunca se escribe en el repositorio.
    /// </summary>
    private static string ResolverCadenaConexion()
    {
        var completa = Environment.GetEnvironmentVariable("NAMISOFT_DB_CONNECTION");
        if (!string.IsNullOrWhiteSpace(completa))
            return completa.Trim();

        var password = Environment.GetEnvironmentVariable("NAMISOFT_DB_PASSWORD");
        if (string.IsNullOrWhiteSpace(password))
            password = LeerPasswordDeArchivo();

        if (string.IsNullOrWhiteSpace(password))
            throw new InvalidOperationException(
                "No se encontró la contraseña de SQL Server. Defina NAMISOFT_DB_CONNECTION " +
                "o NAMISOFT_DB_PASSWORD, o cree ~/.config/namisoft/mssql.pass.");

        var servidor = Environment.GetEnvironmentVariable("NAMISOFT_DB_SERVER") ?? ServidorPorDefecto;
        var baseDatos = Environment.GetEnvironmentVariable("NAMISOFT_DB_NAME") ?? BasePorDefecto;

        return $"Server={servidor};Database={baseDatos};User Id=sa;Password={password.Trim()};" +
               "Encrypt=True;TrustServerCertificate=True;MultipleActiveResultSets=True;Connect Timeout=15";
    }

    private static string? LeerPasswordDeArchivo()
    {
        foreach (var ruta in RutasPassword())
        {
            try
            {
                if (File.Exists(ruta))
                {
                    var contenido = File.ReadAllText(ruta).Trim();
                    if (contenido.Length > 0)
                        return contenido;
                }
            }
            catch (IOException)
            {
                // Archivo ilegible: se prueba la siguiente ubicación.
            }
        }

        return null;
    }

    private static IEnumerable<string> RutasPassword()
    {
        yield return Path.Combine(AppContext.BaseDirectory, "mssql.pass");

        var home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        if (!string.IsNullOrWhiteSpace(home))
            yield return Path.Combine(home, ".config", "namisoft", "mssql.pass");
    }
}
