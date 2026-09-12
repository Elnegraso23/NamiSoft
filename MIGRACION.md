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

Antes de reemplazar la versión WinForms en producción, compara ambos programas con la misma copia de `Nomina.db` y el mismo archivo de checadas.

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

Pendiente: subir EF Core Sqlite 8.0.11 → 10.x (NU1903), rediseño
pantalla por pantalla (siguiente: Empleados), ruta de BD a carpeta de
datos de aplicación, backups automáticos, pruebas de `MotorNomina`.

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
3. Hacer una copia de `Nomina.db` real y comparar resultados de nómina WinForms vs Avalonia.
4. Terminar el diseño visual final.
5. Añadir pruebas automáticas para `MotorNomina` y `NominaService`.
6. Cambiar la ubicación de la base de datos a una ruta de datos de aplicación antes de distribuir instaladores.
7. Agregar copias de seguridad automáticas de SQLite.
8. Crear publicación `win-x64` y probar en la caja Windows real.
