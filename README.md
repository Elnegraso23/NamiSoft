# NamiSoft · migración a Avalonia

Esta carpeta contiene una migración funcional de la aplicación original **C# + WinForms** a **C# + .NET 10 + Avalonia 12 + MVVM**.

## Qué conserva del proyecto original

Se conservaron y reutilizan las capas que no dependían de WinForms:

- `Modelos/`
- `Enumeraciones/`
- `Datos/NominaContext.cs`
- `Servicios/MotorNomina.cs`
- `Servicios/NominaService.cs`
- `Servicios/ChecadasService.cs`
- `Servicios/HorarioManualService.cs`
- `Servicios/SincronizarChecadasPendientesService.cs`
- `Servicios/ExcelNominaService.cs`
- `Utilidades/ExcelHelper.cs`

La interfaz `Form + Designer.cs + DataGridView + MessageBox + FileDialog` fue reemplazada por Avalonia.

## Qué hace NamiSoft

1. **Empleados**: alta, edición, activación/desactivación y eliminación.
2. **Horarios**: turnos mañana/tarde y horario personalizado por día.
3. **Checadas**: importa un reporte Excel (`.xlsx`) y lo transforma a registros de asistencia.
4. **Motor de nómina**: calcula horas trabajadas, retardos y reglas definidas en configuración.
5. **Corrección manual**: permite ajustar entrada/salida de checadas existentes.
6. **Nómina semanal**: calcula sueldo, descuentos, propinas, prima dominical y total.
7. **Excel**: exporta la nómina semanal a `.xlsx` con ClosedXML.
8. **SQLite**: persiste empleados, checadas, horarios y configuración en `NamiSoft.db` (antes `Nomina.db`, se migra sola).

## Nueva estructura

```text
NamiSoft-Avalonia/
├── Datos/                 Entity Framework Core + SQLite
├── Enumeraciones/         Enumeraciones del dominio
├── Modelos/               Entidades y DTOs originales
├── Servicios/             Motor, nómina, checadas, Excel y nuevos servicios UI-free
├── Utilidades/            Lectura de Excel
├── UI/                    Diálogos multiplataforma y estado de aplicación
├── ViewModels/            Lógica de presentación (MVVM)
├── Views/                 Interfaz Avalonia AXAML
├── Styles/                Estilos globales
├── App.axaml
├── Program.cs
└── NamiSoft.csproj
```

## Requisitos

- .NET SDK 10
- Linux: entorno gráfico X11/XWayland compatible con Avalonia
- Windows: Windows 10/11

### Pop!_OS

```bash
sudo apt update
sudo apt install dotnet-sdk-10.0
```

Si tu repositorio de Pop!_OS no ofrece .NET 10, instala el SDK siguiendo el repositorio oficial de Microsoft para la versión de Ubuntu en la que se basa tu Pop!_OS.


### Atajos incluidos

En Pop!_OS puedes ejecutar:

```bash
./run-linux.sh
```

Para generar una publicación Windows x64 desde Linux:

```bash
./publish-windows.sh
```

En Windows PowerShell puedes usar:

```powershell
./run-windows.ps1
```

## Ejecutar en Linux

```bash
cd NamiSoft-Avalonia
dotnet restore
dotnet run
```

Abre la carpeta en Zed:

```bash
zed .
```

Los archivos visuales están en `Views/*.axaml`. Guarda cambios y vuelve a ejecutar `dotnet run` para verlos.

## Ejecutar en Windows

Con .NET 10 SDK instalado:

```powershell
cd C:\ruta\NamiSoft-Avalonia
dotnet restore
dotnet run
```

También puedes abrir `NamiSoft.csproj` en Visual Studio, Rider, VS Code o Zed.

## Publicar

### Windows x64

```bash
dotnet publish -c Release -r win-x64 --self-contained true
```

### Linux x64

```bash
dotnet publish -c Release -r linux-x64 --self-contained true
```

Los archivos aparecen dentro de `bin/Release/net10.0/<runtime>/publish/`.

## Base de datos existente

El archivo local se llama:

```text
NamiSoft.db
```

Si vienes de la versión anterior (`Nomina.db`), la aplicación la detecta
y la copia automáticamente a `NamiSoft.db` en el primer arranque.

Si ya tienes una base real de la versión WinForms, **haz una copia de seguridad primero**. Para probar la migración con datos existentes, copia `NamiSoft.db` (antes `Nomina.db`, se migra sola) al directorio desde el que ejecutas la aplicación.

No borres tu proyecto WinForms hasta haber comparado todos los resultados de nómina con datos reales.

## Dependencias principales

El proyecto evita componentes Avalonia Pro/Plus y controles comerciales.

- Avalonia 12.1.1
- CommunityToolkit.Mvvm 8.4.2
- Entity Framework Core SQLite 10.0.11
- ClosedXML 0.105.1

No se usa `FontAwesome.Sharp` porque era una dependencia específica de la UI WinForms.

## Pantallas migradas

| WinForms original | Avalonia |
|---|---|
| `FrmPrincipal` | `MainWindow` + `DashboardView` |
| `FrmEmpleados` | `EmployeesView` |
| `FrmNuevoEmpleado` | `EmployeeEditorWindow` |
| `FrmHorarioPersonalizado` | `ScheduleEditorWindow` |
| `FrmImportarChecadas` | `ImportAttendanceView` |
| `FrmHorarioManual` | `ManualScheduleView` |
| `FrmNomina` | `PayrollView` |
| `FrmConfiguracion` | `SettingsView` |

`FrmEditarEmpleado` del proyecto original estaba vacío y no necesitó una vista separada; editar y crear usan el mismo editor.

## Nota de validación

La migración fue realizada a nivel de código y estructura. El entorno donde se generó este paquete no dispone del SDK de .NET, por lo que el paso final que debes hacer en tu Pop!_OS es:

```bash
dotnet restore
dotnet build
dotnet run
```

Si `dotnet build` devuelve un error concreto, corrígelo antes de continuar con personalización visual. Conserva el WinForms original como referencia durante esta fase.
