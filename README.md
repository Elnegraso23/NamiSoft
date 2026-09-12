# NamiSoft (interno, repo privado)

Sistema de control de asistencia y nómina semanal para operación con pantalla
táctil en Windows, desarrollado y probado en Linux/Pop!_OS.

Stack: C# + .NET 10 + Avalonia UI + MVVM (CommunityToolkit.Mvvm) +
Entity Framework Core + SQLite. Sin componentes de pago
(ver `THIRD_PARTY_LICENSES.md`).

## Estado actual (2026-09-12)

- Migración WinForms → Avalonia completa. Cero `System.Windows.Forms`.
- Módulo **Asistencia** con registro interno (ya no depende de Excel;
  la importación queda como opción en su pestaña).
- `ChecadaEmpleado` con trazabilidad: origen, creación, modificación, motivo.
- Migraciones EF con línea base sin pérdida de datos (`EnsureMigrated`).
- Sistema de diseño táctil (48px+), navegación lateral con sección activa,
  estados vacíos accionables, E/S pesada fuera del hilo UI.
- Compila en `net10.0` con 0 errores. Verificado con capturas reales en Linux.

## Cómo correrlo

```bash
./run-linux.sh        # restore + build + run
```

```bash
dotnet build NamiSoft.csproj
dotnet run
```

Publicar para Windows (plataforma comercial):

```bash
./publish-windows.sh  # → publish/win-x64 (self-contained)
```

En Windows PowerShell: `./run-windows.ps1`.

Requisito: .NET SDK 10 (`dotnet --list-sdks` debe mostrar `10.x`).

## Módulos

| Sección | Qué hace |
|---|---|
| Resumen | KPIs (activos, totales, checadas, pendientes) y flujo del programa |
| Empleados | Alta, edición, activar/desactivar, eliminar (con confirmación), sincronizar pendientes |
| Asistencia → Hoy | Reloj táctil: entrada/salida con antiduplicados, captura manual, registros del día |
| Asistencia → Historial | Últimos 7 días del empleado |
| Asistencia → Incidencias | Falta entrada/salida, modificadas; revisar antes de nómina |
| Asistencia → Importar | Excel opcional con la misma validación y tabla que el registro interno |
| Horario manual | Corrección semanal de entrada/salida (delega el cálculo, no duplica reglas) |
| Nómina semanal | Cálculo + aviso de incompletas (no bloquea) + exportación a Excel |
| Configuración | Turnos, tolerancias, descuentos por retardo, prima dominical |

## Datos y migraciones

- Archivo local: `NamiSoft.db` (gitignored). Si existe `Nomina.db` anterior,
  se copia sola en el primer arranque (con checkpoint WAL para no perder datos).
- Acceso centralizado en `Datos/NominaDatabase.cs`. La UI no conoce rutas ni SQL.
- Migraciones en `Datos/Migrations/`. BDs viejas (sin historial) se registran
  como línea base y solo reciben lo que les falte.
- Dinero siempre en `decimal`. `Guardar` de empleado es atómico (transacción).
- Haz copia de `NamiSoft.db` antes de probar cambios de nómina con datos reales.

## Estructura

```text
├── Datos/          NominaDatabase (factory), NominaContext, Migrations/
├── Enumeraciones/  TipoHorario, OrigenChecada
├── Modelos/        Entidades y DTOs
├── Servicios/      Reglas de negocio (sin UI)
├── Utilidades/     Lectura de Excel
├── UI/             Diálogos multiplataforma, AppState
├── ViewModels/     Estado y comandos por pantalla
├── Views/          AXAML + code-behind solo visual
└── Styles/         Sistema de diseño (AppStyles.axaml)
```

Reglas: Views sin acceso a BD; ViewModels sin SQL; sin excepciones sin manejar
hacia la UI; operaciones pesadas con `Task.Run`; botones táctiles ≥48px.

## Pendientes conocidos

- Subir `Microsoft.EntityFrameworkCore.Sqlite` 8.0.11 → 10.x (aviso NU1903).
- Mover la BD a carpeta de datos de aplicación + respaldos automáticos.
- Pruebas automáticas de `MotorNomina` y `NominaService`.
- Publicar `win-x64` y probar en el equipo Windows real.
- Comparar nómina WinForms vs NamiSoft con la misma BD antes de producción.
- Ver detalle de avance por fase en `MIGRACION.md`.

## Notas de desarrollo

- Los cambios se verifican con `dotnet build` y capturas reales de la app
  corriendo en X11 (capturas vía `xwd`; la navegación sintética con
  `xdotool` falla si la sesión está en uso — en ese caso verificar a mano).
- Sintaxis de bindings a cuidar: `PlaceholderText` (no `Watermark`),
  sin `Padding` en `StackPanel`, negación `!Flag` válida en `IsVisible`.
- `DbContextAttribute` vive en `Microsoft.EntityFrameworkCore.Infrastructure`
  (requerido para que EF descubra las migraciones en runtime).
- Commits en español, rama `main`, remoto `origin → Elnegraso23/NamiSoft`.
