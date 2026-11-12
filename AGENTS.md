# Reglas para OpenCode / agentes

Este repositorio es la migración de NamiSoft a Avalonia.

## Stack obligatorio

- C#
- .NET 10 LTS
- Avalonia UI OSS
- MVVM
- SQLite + Entity Framework Core

## Licencias

No agregues paquetes comerciales, trialware, Avalonia Plus/Pro/Enterprise ni controles que requieran pago.
Antes de agregar una dependencia, confirma que tiene licencia permisiva y uso comercial gratuito (preferir MIT, Apache-2.0 o BSD).

## Arquitectura

- `Views/`: solo presentación y code-behind mínimo necesario para cerrar ventanas/dialogs.
- `ViewModels/`: estado y comandos de la pantalla.
- `Servicios/`: reglas de negocio, persistencia y operaciones reutilizables.
- `Modelos/`: dominio/DTOs.
- No acceder directamente a EF Core desde AXAML o code-behind.
- No introducir `System.Windows.Forms` ni WPF.

## Migración

El comportamiento del motor de nómina original es la referencia. No cambies fórmulas o reglas de negocio durante un cambio puramente visual.

## UI

- Diseñar para escritorio y uso táctil.
- Controles interactivos con altura mínima aproximada de 42 px.
- Evitar posiciones absolutas; usar `Grid`, `StackPanel`, `WrapPanel`, etc.
- La interfaz debe funcionar en Linux y Windows.
- No depender de `DataGrid` premium; las tablas actuales están construidas con controles base.

## Verificación mínima después de cambios

```bash
dotnet restore
dotnet build
dotnet run
```

Para cambios de lógica de nómina, agregar/ejecutar pruebas antes de aceptar la modificación.

## Instalación en Windows con Smart App Control activo

Si el usuario reporta `[WinError 4551] Una directiva de Control de aplicaciones
bloqueó este archivo`, no es antivirus ni SmartScreen: es **SAC** bloqueando un
binario sin firma. No firmes, no repackees, no renombres y no desactives SAC en
equipos ajenos. La ruta soportada es instalar framework-dependent y lanzar como
DLL bajo `dotnet.exe`:

```powershell
powershell -ExecutionPolicy Bypass -File .\instalador\instalar-namisoft.ps1 -FromRelease -Test
```

Reglas duras para el agente:

- Nunca ejecutes `%LOCALAPPDATA%\NamiSoft\app\NamiSoft.exe`: es el apphost sin
  firma y vuelve a disparar el 4551. El lanzador válido es
  `%LOCALAPPDATA%\NamiSoft\NamiSoft.cmd`.
- No modifiques `instalador/instalar-namisoft.ps1` sin probarlo: el flujo
  completo está descrito en `docs/INSTALACION-SIN-FIRMA.md` (incluye criterios
  de aceptación y tabla de diagnóstico).
- El ZIP que consume el instalador lo publica `.github/workflows/fdd.yml`
  (`NamiSoft-<tag>-win-x64-fdd.zip`). Si falta, el script compila desde fuente
  solo; no hace falta firmar nada.
- La cadena de conexión se resuelve en `Datos/NominaDatabase.cs`:
  `NAMISOFT_DB_CONNECTION` > `NAMISOFT_DB_SERVER`/`NAMISOFT_DB_NAME` +
  `%USERPROFILE%\.config\namisoft\mssql.pass` > `localhost,1433`/`NamiSoft`.
