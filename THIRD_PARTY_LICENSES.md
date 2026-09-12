# Licencias de terceros — NominaPro

Todo el stack debe poder usarse gratuitamente en un producto comercial.
Antes de agregar una dependencia nueva: comprobar su licencia, justificar
por qué es necesaria y preferir controles estándar de Avalonia.

Última revisión: 2026-09-12.

## Dependencias directas (NuGet)

| Paquete | Versión | Licencia | Uso comercial gratuito | Propósito |
|---|---|---|---|---|
| Avalonia | 12.1.1 | MIT | Sí | Framework UI multiplataforma (Linux/Windows) |
| Avalonia.Desktop | 12.1.1 | MIT | Sí | Backend de escritorio (X11/Win32) |
| Avalonia.Themes.Fluent | 12.1.1 | MIT | Sí | Tema base de la aplicación |
| Avalonia.Fonts.Inter | 12.1.1 | OFL 1.1 (fuente) + MIT (código) | Sí | Tipografía incluida |
| CommunityToolkit.Mvvm | 8.4.2 | MIT | Sí | MVVM: ObservableObject, RelayCommand |
| ClosedXML | 0.105.1 | MIT | Sí | Lectura/escritura de Excel (.xlsx) sin Office |
| Microsoft.EntityFrameworkCore.Sqlite | 8.0.11 | Apache-2.0 | Sí | ORM + proveedor SQLite |

## Dependencias transitivas relevantes

| Paquete | Licencia | Notas |
|---|---|---|
| DocumentFormat.OpenXml | MIT | Vía ClosedXML (formato .xlsx) |
| ExcelNumberFormat | Apache-2.0 | Vía ClosedXML |
| Microsoft.Data.Sqlite / SQLitePCLRaw | Apache-2.0 | Vía EF Core Sqlite |
| SkiaSharp / HarfBuzzSharp | MIT | Renderizado, vía Avalonia |
| SixLabors.Fonts | Apache-2.0 | Vía Avalonia |

## Notas

- No se usa Avalonia Pro/Plus, ni controles premium, trial o con regalías.
- `dotnet restore` reporta advertencia NU1903 (vulnerabilidad alta en
  `SQLitePCLRaw.lib.e_sqlite3 2.1.6`, vía EF Core 8.0.11). Pendiente:
  subir a `Microsoft.EntityFrameworkCore.Sqlite` 10.x cuando haya
  ventana de pruebas con datos reales, y revalidar el motor de nómina.
- `Nomina.db` (SQLite) no tiene restricción de licencia para distribución.
