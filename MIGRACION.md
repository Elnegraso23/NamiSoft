# Informe de migración WinForms → Avalonia

## 1. Funcionamiento del proyecto original

El programa original es una aplicación WinForms monolítica de escritorio para gestionar asistencia y nómina.

### Inicio

`Program.cs` inicializa WinForms, abre `NominaContext`, ejecuta `Database.EnsureCreated()` y muestra `FrmPrincipal`.

### Persistencia

`NominaContext` usa Entity Framework Core con SQLite y la conexión relativa `Data Source=Nomina.db`.

Tablas principales:

- Empleados
- Checadas
- HorariosEmpleado
- ConfiguracionSistema
- ChecadasPendientes

### Empleados

`FrmEmpleados` consultaba directamente EF Core para listar, buscar, filtrar, activar/desactivar y eliminar. `FrmNuevoEmpleado` servía tanto para alta como para edición.

En la migración esas operaciones se concentran en `Servicios/EmpleadoService.cs` y la UI las consume desde `EmployeesViewModel`.

### Importación de checadas

`FrmImportarChecadas` usaba `OpenFileDialog`, `ExcelHelper` y `ChecadasService`.

En Avalonia:

- `AppDialogService` abre el selector de archivos multiplataforma.
- `ImportAttendanceViewModel` carga la vista previa.
- `ChecadasService` sigue realizando la importación real.

### Horario manual

`FrmHorarioManual` cargaba un `DataGridView`, permitía editar Entrada/Salida y enviaba la lista a `HorarioManualService`.

En Avalonia se usa una tabla construida con `ListBox + Grid + TextBox`; no se requiere `DataGrid` de pago.

### Nómina

`FrmNomina` llamaba `NominaService.CalcularNomina` y mostraba los resultados en `DataGridView`. La exportación se realiza con `ExcelNominaService`.

Los servicios se conservaron. Solo cambió la presentación y los diálogos de archivo.

### Configuración

`FrmConfiguracion` gestionaba turnos, tolerancias, descuentos por retardos y prima dominical. La lógica de persistencia se extrajo a `ConfiguracionService`.

## 2. Qué se eliminó

- `System.Windows.Forms`
- `Form`
- `*.Designer.cs`
- `*.resx` de formularios
- `DataGridView`
- `MessageBox`
- `OpenFileDialog` / `SaveFileDialog`
- `FontAwesome.Sharp`
- navegación por formularios embebidos dentro de `Panel`

## 3. Qué se añadió

- Avalonia 12.1.1
- AXAML
- MVVM con CommunityToolkit.Mvvm
- `ViewLocator`
- `AppDialogService`
- `AppState`
- vistas separadas de ViewModels
- servicios adicionales para retirar acceso directo a EF Core de las pantallas

## 4. Qué no se cambió deliberadamente

El motor de cálculo de nómina y la interpretación del Excel no se reescribieron. El objetivo fue cambiar el framework de UI sin alterar las reglas del negocio.

Antes de reemplazar la versión WinForms en producción, compara ambos programas con la misma copia de `NamiSoft.db` (antes `Nomina.db`, se migra sola) y el mismo archivo de checadas.

## 6. Cambios aplicados (2026-09-12)

- Target framework `net8.0` → `net10.0` (SDK 10.0.112 verificado, 0 errores).
- `TextBox.Watermark` → `PlaceholderText` (6 usos, elimina warnings AVLN5001).
- `THIRD_PARTY_LICENSES.md`: inventario de licencias (MIT / Apache-2.0 / OFL).
- Sistema de diseño en `Styles/AppStyles.axaml`: botones 48px, variantes
  primary/ghost/danger, navegación con estado seleccionado, tarjetas,
  tablas, etiquetas de campo, KPIs, campos e indicadores táctiles.
- Navegación principal: botones sueltos → `ListBox` con `SelectedNav`;
  la sección activa queda resaltada y se conserva el refresco por página.
- Importar checadas: corregido layout del estado (fila propia, sin margen fijo).
- E/S pesada fuera del hilo UI (`Task.Run`): lectura Excel, importación,
  cálculo/exportación de nómina, sincronización y horario manual.
- Empleados: barra de acciones con `WrapPanel` (ya no se desborda en 1366px).

Pendiente: rediseño pantalla por pantalla (siguiente: Empleados), ruta de BD a
carpeta de datos de aplicación, backups automáticos, pruebas de `MotorNomina`.

## 7. Capa de persistencia (2026-09-12)

- `Datos/NominaDatabase`: único punto de acceso (ruta + conexión + factory).
  17 `new NominaContext()` dispersos → `NominaDatabase.CreateContext()`.
- `NominaContext`: ctor con opciones + `OnModelCreating` (índice único en
  `NumeroEmpleado`, campos requeridos, precisión en `decimal`). Dinero ya
  estaba en `decimal` en todo el modelo. Sin TXT/JSON/CSV como almacén:
  Excel solo importación/exportación.
- Eliminado `EstadoSistema` estático duplicado (solo queda `AppState`).
- `EmpleadoService.Guardar` ahora es atómico (transacción) y ya no lanza
  excepciones a la UI: `AlternarActivo`/`Eliminar` devuelven `bool`.
- Verificado con captura real del Dashboard en Linux (Resumen, KPIs, flujo).

## 8. Módulo de asistencia interna (2026-09-12)

- La app ya funciona sin Excel: registro interno + importación opcional,
  ambos en la misma tabla `Checadas` con el mismo cálculo.
- `ChecadasService` evolucionado: `RegistrarEntrada/Salida` (antiduplicados,
  valida inactivo/salida>entrada), `AgregarManual`, `EditarChecada`,
  `EliminarChecada`, `ObtenerDelDia/Rango`, `ObtenerIncidencias`,
  `ContarIncompletas`, `ResumenPrevio`, `SemanaDe`. Motor intacto.
- Nueva vista `Asistencia` (pestañas Hoy/Historial/Incidencias/Importar):
  maestro-detalle táctil, botones 64px, captura manual inline, historial
  7 días, incidencias con texto (no solo color), importación embebida.
- Nómina advierte checadas incompletas antes de calcular (no bloquea).
- Horas en `hh:mm` (`EntradaTexto/SalidaTexto`); nombre sin truncar.
- Referencia de nivel: PoloTab (táctil grande, pocos pasos, offline),
  sin copiar diseño ni assets.

## 5. Próximos pasos recomendados

1. Compilar y ejecutar en Pop!_OS.
2. Corregir cualquier incompatibilidad puntual del SDK/paquetes.
3. Hacer una copia de `NamiSoft.db` (antes `Nomina.db`, se migra sola) real y comparar resultados de nómina WinForms vs Avalonia.
4. Terminar el diseño visual final.
5. Añadir pruebas automáticas para `MotorNomina` y `NominaService`.
6. Cambiar la ubicación de la base de datos a una ruta de datos de aplicación antes de distribuir instaladores.
7. Agregar copias de seguridad automáticas de SQLite.
8. Crear publicación `win-x64` y probar en la caja Windows real.

## 9. Migración de SQLite a Microsoft SQL Server (2026-10-28)

Motivo: la materia de Base de Datos exige el motor de Microsoft. La app pasa de
un archivo local `NamiSoft.db` a una instancia SQL Server 2022 (contenedor
`mssql-bd`, `MSSQL_PID=Developer`, puerto 1433).

### Cambios de código

- `NamiSoft.csproj`: `Microsoft.EntityFrameworkCore.Sqlite` 8.0.11 →
  `Microsoft.EntityFrameworkCore.SqlServer` 10.0.14 +
  `Microsoft.EntityFrameworkCore.Design` 10.0.14. Se fija
  `System.Security.Cryptography.Pkcs` 10.0.14 para eliminar NU1902/NU1903.
- `Datos/NominaContext.cs`: `UseSqlite` → `UseSqlServer`.
- `Datos/NominaDatabase.cs`: se eliminó toda la lógica SQLite
  (`SqliteConnection` heredada, `PRAGMA wal_checkpoint`, `sqlite_master`,
  `pragma_table_info`, `INSERT OR IGNORE`). `EnsureMigrated()` ahora es
  `db.Database.Migrate()`.
  La cadena se resuelve en este orden: `NAMISOFT_DB_CONNECTION` →
  `NAMISOFT_DB_PASSWORD` (+ `NAMISOFT_DB_SERVER` / `NAMISOFT_DB_NAME`) →
  archivo `mssql.pass` junto al ejecutable o `~/.config/namisoft/mssql.pass`.
  Ninguna contraseña queda en el repositorio.
- `Datos/Migrations/`: se borraron las migraciones SQLite y se regeneró una
  única `InitialCreate` con el proveedor SQL Server.

### Equivalencias de tipos generadas

| Modelo C# | SQLite anterior | SQL Server actual |
|---|---|---|
| `int Id` (PK) | `INTEGER` + `Sqlite:Autoincrement` | `int` + `SqlServer:Identity(1,1)` |
| `string` con `HasMaxLength` | `TEXT` | `nvarchar(n)` |
| `string` sin límite | `TEXT` | `nvarchar(max)` |
| `DateTime` | `TEXT` | `datetime2` |
| `TimeSpan` / `TimeSpan?` | `TEXT` | `time` |
| `decimal(p,s)` | `TEXT` | `decimal(p,s)` |
| `bool` | `INTEGER` | `bit` |

Índices conservados: `IX_Empleados_NumeroEmpleado` (**único**),
`IX_Checadas_NumeroEmpleado_Fecha`, `IX_ChecadasPendientes_NumeroEmpleado_Fecha`,
`IX_HorariosEmpleado_NumeroEmpleado_DiaSemana`.

### Verificación real (no simulada)

- `dotnet build`: 0 errores, 0 advertencias.
- `dotnet ef database update`: base `NamiSoft` creada con 5 tablas + historial EF.
- Prueba de humo con la misma capa de datos: INSERT/SELECT de un empleado y una
  checada con `TimeSpan` (07:58 / 17:03) y `decimal` (9.08 / 1.50) → round-trip
  correcto.
- Duplicado de `NumeroEmpleado` rechazado por el índice único:
  `SqlException: Cannot insert duplicate key row ... IX_Empleados_NumeroEmpleado`.
- App Avalonia arrancada en display virtual contra SQL Server: Dashboard muestra
  Empleados=1, Checadas=1 (datos insertados vía EF Core), sin excepciones.
- `sql/NamiSoft_SQLServer.sql` ejecutado en una base limpia: 6 tablas, 2
  empleados, 2 checadas y 1 configuración inicial.

### Cómo levantar el entorno

```bash
podman run -d --name mssql-bd --restart=unless-stopped \
  -e ACCEPT_EULA=Y -e "MSSQL_SA_PASSWORD=<password>" -e MSSQL_PID=Developer \
  -p 1433:1433 mcr.microsoft.com/mssql/server:2022-latest
export NAMISOFT_DB_CONNECTION="Server=localhost,1433;Database=NamiSoft;User Id=sa;Password=<password>;Encrypt=True;TrustServerCertificate=True"
dotnet run
```

