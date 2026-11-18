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
    /// 2) Servidor/base por defecto (o db.config) + password de NAMISOFT_DB_PASSWORD.
    /// 3) Servidor/base por defecto (o db.config) + password leída de un archivo local
    ///    (mssql.pass junto al ejecutable o ~/.config/namisoft/mssql.pass),
    ///    o autenticación integrada de Windows si db.config indica auth=windows.
    /// La password nunca se escribe en el repositorio.
    /// </summary>
    private static string ResolverCadenaConexion()
    {
        var completa = Environment.GetEnvironmentVariable("NAMISOFT_DB_CONNECTION");
        if (!string.IsNullOrWhiteSpace(completa))
            return completa.Trim();

        var (cfgServidor, cfgBase, cfgAuth) = LeerConfigLocal();
        var servidor = Environment.GetEnvironmentVariable("NAMISOFT_DB_SERVER") ?? cfgServidor ?? ServidorPorDefecto;
        var baseDatos = Environment.GetEnvironmentVariable("NAMISOFT_DB_NAME") ?? cfgBase ?? BasePorDefecto;

        if (string.Equals(cfgAuth, "windows", StringComparison.OrdinalIgnoreCase))
            return ConstruirCadena(servidor, baseDatos, null, autenticacionWindows: true);

        var password = Environment.GetEnvironmentVariable("NAMISOFT_DB_PASSWORD");
        if (string.IsNullOrWhiteSpace(password))
            password = LeerPasswordDeArchivo();

        if (string.IsNullOrWhiteSpace(password))
            throw new InvalidOperationException(
                "No se encontró la contraseña de SQL Server. Defina NAMISOFT_DB_CONNECTION " +
                "o NAMISOFT_DB_PASSWORD, o cree ~/.config/namisoft/mssql.pass.");

        return ConstruirCadena(servidor, baseDatos, password, autenticacionWindows: false);
    }

    public static void ConfigurarYProbar(string servidor, string baseDatos, string? password, bool autenticacionWindows)
    {
        var srv = string.IsNullOrWhiteSpace(servidor) ? ServidorPorDefecto : servidor.Trim();
        var db = string.IsNullOrWhiteSpace(baseDatos) ? BasePorDefecto : baseDatos.Trim();

        if (!autenticacionWindows && string.IsNullOrWhiteSpace(password))
            throw new InvalidOperationException("Ingrese la contraseña de SQL Server o active la autenticación de Windows.");

        var anterior = _connectionString;
        _connectionString = ConstruirCadena(srv, db, password, autenticacionWindows);

        try
        {
            EnsureMigrated();
        }
        catch
        {
            _connectionString = anterior;
            throw;
        }

        GuardarConfigLocal(srv, db, password, autenticacionWindows);
    }

    private static string ConstruirCadena(string servidor, string baseDatos, string? password, bool autenticacionWindows)
    {
        if (autenticacionWindows)
        {
            return $"Server={servidor};Database={baseDatos};Trusted_Connection=True;" +
                   "Encrypt=True;TrustServerCertificate=True;MultipleActiveResultSets=True;Connect Timeout=15";
        }

        return $"Server={servidor};Database={baseDatos};User Id=sa;Password={password!.Trim()};" +
               "Encrypt=True;TrustServerCertificate=True;MultipleActiveResultSets=True;Connect Timeout=15";
    }

    private static string? CarpetaConfigUsuario()
    {
        var home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        return string.IsNullOrWhiteSpace(home) ? null : Path.Combine(home, ".config", "namisoft");
    }

    private static (string? Servidor, string? BaseDatos, string? Auth) LeerConfigLocal()
    {
        var carpeta = CarpetaConfigUsuario();
        if (carpeta is null)
            return (null, null, null);

        var ruta = Path.Combine(carpeta, "db.config");
        if (!File.Exists(ruta))
            return (null, null, null);

        string? servidor = null, baseDatos = null, auth = null;
        try
        {
            foreach (var linea in File.ReadAllLines(ruta))
            {
                var idx = linea.IndexOf('=');
                if (idx <= 0) continue;
                var clave = linea[..idx].Trim();
                var valor = linea[(idx + 1)..].Trim();
                if (string.IsNullOrEmpty(valor)) continue;

                switch (clave)
                {
                    case "server": servidor = valor; break;
                    case "database": baseDatos = valor; break;
                    case "auth": auth = valor; break;
                }
            }
        }
        catch (IOException)
        {
            // Se usan los valores por defecto si el archivo no se puede leer.
        }

        return (servidor, baseDatos, auth);
    }

    private static void GuardarConfigLocal(string servidor, string baseDatos, string? password, bool autenticacionWindows)
    {
        var carpeta = CarpetaConfigUsuario();
        if (carpeta is null)
            return;

        Directory.CreateDirectory(carpeta);

        if (!autenticacionWindows && !string.IsNullOrWhiteSpace(password))
        {
            var rutaPass = Path.Combine(carpeta, "mssql.pass");
            File.WriteAllText(rutaPass, password.Trim());
            if (OperatingSystem.IsLinux() || OperatingSystem.IsMacOS())
            {
                File.SetUnixFileMode(rutaPass, UnixFileMode.UserRead | UnixFileMode.UserWrite);
            }
        }

        var rutaConfig = Path.Combine(carpeta, "db.config");
        var lineas = new[]
        {
            $"server={servidor}",
            $"database={baseDatos}",
            $"auth={(autenticacionWindows ? "windows" : "sa")}"
        };
        File.WriteAllLines(rutaConfig, lineas);
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
