# Instalación en Windows sin firma digital (SAC / WinError 4551)

Guía determinista para que un agente (o una persona) instale NamiSoft en un
Windows 11 con **Control inteligente de aplicaciones (Smart App Control, SAC)**
activo, sin firmar nada y sin desactivar la protección del equipo.

## El problema

El instalador oficial (`NamiSoft-Setup-<ver>-x64.exe`, Inno Setup) contiene
`NamiSoft.exe`, un apphost generado por `dotnet publish` que **no está firmado**
con Authenticode. SAC bloquea la ejecución de binarios sin firma ni reputación
antes de mostrar cualquier aviso, y el síntoma es:

```
[WinError 4551] Una directiva de Control de aplicaciones bloqueó este archivo
```

Lo mismo ocurre con `dotnet publish -r win-x64 --self-contained true` a mano.

## La solución (por qué funciona)

SAC decide sobre **archivos PE ejecutables**. `dotnet.exe` sí está firmado por
Microsoft y tiene reputación, y una DLL o un `.cmd` **no son PE ejecutables**.

Entonces: se publica **framework-dependent** (sin apphost ejecutable propio) y
la aplicación se lanza como DLL bajo el host firmado:

```
dotnet.exe  app\NamiSoft.dll
```

Cero binarios sin firma en la cadena de ejecución ⇒ no hay 4551.

## Comando único (lo que debe hacer el agente)

```powershell
powershell -ExecutionPolicy Bypass -File .\instalador\instalar-namisoft.ps1 -FromRelease -Test
```

Eso, en orden:

1. Lee el estado de SAC (`HKLM\SYSTEM\CurrentControlSet\Control\CI\Policy`).
2. Verifica si hay runtime **.NET 10**; si no, lo instala en
   `%LOCALAPPDATA%\NamiSoft\dotnet` con `dotnet-install.ps1` de Microsoft
   (sin admin, sin tocar Program Files).
3. Descarga el asset `NamiSoft-<ver>-win-x64-fdd.zip` de la última release y lo
   extrae en `%LOCALAPPDATA%\NamiSoft\app`.
4. Configura la conexión a SQL Server si se pasan los parámetros (ver abajo).
5. Crea `%LOCALAPPDATA%\NamiSoft\NamiSoft.cmd` y los accesos del menú Inicio
   (`NamiSoft` y `NamiSoft - Configurar base de datos`).
6. Con `-Test`, arranca 8 s y confirma que el proceso `dotnet` vive.

No requiere permisos de administrador.

### Con SQL Server ya instalado en el equipo

```powershell
# a) login SQL (usuario sa). Pide la password enmascarada.
powershell -ExecutionPolicy Bypass -File .\instalador\instalar-namisoft.ps1 -FromRelease `
    -DbServer "localhost,1433" -DbName NamiSoft

# b) autenticacion de Windows (sin sa)
powershell -ExecutionPolicy Bypass -File .\instalador\instalar-namisoft.ps1 -FromRelease `
    -DbConnection "Server=localhost;Database=NamiSoft;Trusted_Connection=True;Encrypt=True;TrustServerCertificate=True"

# c) instancia con nombre (Express / LocalDB): sin puerto
... -DbServer "localhost\SQLEXPRESS" -DbName NamiSoft
```

Sin parámetros de BD, el script pide la password de `sa` de forma interactiva;
con `Enter` la deja pendiente y se configura luego desde el menú Inicio.
Para no tocar nada: `-NoDb`.

### Sin conexión a la release (offline o asset ausente)

```powershell
# con un ZIP local
powershell -ExecutionPolicy Bypass -File .\instalador\instalar-namisoft.ps1 -Zip .\NamiSoft-1.0.0-win-x64-fdd.zip

# compilando desde el codigo fuente (instala SDK .NET 10 local)
powershell -ExecutionPolicy Bypass -File .\instalador\instalar-namisoft.ps1 -FromSource
```

## Cómo resuelve la cadena de conexión la aplicación

`Datos/NominaDatabase.cs`, en este orden:

1. `NAMISOFT_DB_CONNECTION` — cadena completa, se usa tal cual.
2. `NAMISOFT_DB_SERVER` / `NAMISOFT_DB_NAME` (variables de usuario) + password en
   `%USERPROFILE%\.config\namisoft\mssql.pass` (o `mssql.pass` junto al exe).
3. Nada configurado ⇒ `localhost,1433` / `NamiSoft` con usuario `sa`.

Al primer arranque se aplican las **migraciones de EF Core**, así que la base y
las tablas se crean solas. El login usado necesita `dbcreator` o `sysadmin`.

Alternativa manual: ejecutar `app\sql\NamiSoft_SQLServer.sql` en SSMS o
`sqlcmd -S localhost,1433 -U sa -C -N -i NamiSoft_SQLServer.sql`
(crea la base, el esquema completo y datos de ejemplo).

## Verificación (criterios de aceptación)

```powershell
# 1. estado de SAC
reg query "HKLM\SYSTEM\CurrentControlSet\Control\CI\Policy" /v VerifiedAndReputablePolicyState
#    0x1 = activo (es el caso normal), 0x2 = evaluacion, 0x0/ausente = apagado

# 2. runtime .NET 10 presente
dotnet --list-runtimes | findstr "Microsoft.NETCore.App 10."

# 3. artefactos instalados
Test-Path "$env:LOCALAPPDATA\NamiSoft\app\NamiSoft.dll"   # True
Test-Path "$env:LOCALAPPDATA\NamiSoft\NamiSoft.cmd"       # True

# 4. base creada
sqlcmd -S localhost,1433 -U sa -C -Q "SELECT name FROM sys.databases WHERE name='NamiSoft'"

# 5. arranque real
& "$env:LOCALAPPDATA\NamiSoft\NamiSoft.cmd"
```

Se considera instalado cuando (3) existe, (4) devuelve `NamiSoft` y la ventana
de la aplicación aparece sin error.

## Diagnóstico rápido

| Síntoma | Causa | Acción |
|---|---|---|
| `WinError 4551` sigue al abrir | se ejecutó `app\NamiSoft.exe` en vez del lanzador | usar `%LOCALAPPDATA%\NamiSoft\NamiSoft.cmd` |
| `You must install .NET to run this application` | falta el runtime 10 | volver a correr el script (lo instala local) |
| `Login failed for user 'sa'` | SQL Server solo con autenticación Windows, o password incorrecta | usar `-DbConnection ...Trusted_Connection=True...` |
| `network-related or instance-specific error` | instancia con nombre o TCP/1433 deshabilitado | `-DbServer "localhost\SQLEXPRESS"` o habilitar TCP en el Administrador de configuración de SQL Server |
| Bloqueo persiste con SAC apagado | directiva WDAC/AppLocker de un equipo administrado (escuela/empresa) | pedir la excepción a TI; el script no puede saltarse una directiva corporativa |
| La app abre y cierra al instante | conexión a BD rechazada | revisar el mensaje en consola: `...\app\Configurar-BD.cmd` y reintentar |

## Lo que NO se debe hacer

- **No** repackear, comprimir o renombrar el instalador: cambia el hash, sigue
  sin firma y SAC lo bloquea igual.
- **No** firmar con certificado autofirmado: SAC solo confía en firmantes con
  reputación.
- **No** desactivar SAC en el equipo de otra persona: es irreversible desde la
  interfaz (solo reinstalando Windows) y baja su protección.
- **No** ejecutar `app\NamiSoft.exe`, aunque el script lo deje en la carpeta: se
  usa para el icono, no para arrancar.

## Publicación del asset (mantenedores)

`.github/workflows/fdd.yml` compila `--no-self-contained` en `windows-latest` y
adjunta `NamiSoft-<tag>-win-x64-fdd.zip` a la release del tag. Para regenerarlo:

```bash
gh workflow run fdd.yml -f tag=v1.0.0
gh release view v1.0.0 --json assets -q '.assets[].name'
```

Si el asset no existe, el script cae automáticamente a compilar desde fuente, así
que la instalación nunca queda bloqueada por un release incompleto.
