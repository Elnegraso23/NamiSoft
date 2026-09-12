using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NominaPro.Enumeraciones;
using NominaPro.Modelos;
using NominaPro.Servicios;
using NominaPro.UI;
using System.Collections.ObjectModel;

namespace NominaPro.ViewModels;

/// <summary>
/// Asistencia: registro interno del día (reloj), historial, incidencias e
/// importación opcional. Toda escritura pasa por ChecadasService.
/// </summary>
public partial class AttendanceViewModel : ViewModelBase, IRefreshable
{
    private readonly IAppDialogService _dialogs;
    private readonly ChecadasService _checadas = new();
    private readonly EmpleadoService _empleados = new();

    public ImportAttendanceViewModel ImportVM { get; }

    public ObservableCollection<Empleado> Employees { get; } = new();
    public ObservableCollection<ChecadaEmpleado> DayRecords { get; } = new();
    public ObservableCollection<ChecadaEmpleado> History { get; } = new();
    public ObservableCollection<IncidenciaAsistencia> Issues { get; } = new();

    [ObservableProperty] private Empleado? selectedEmployee;
    [ObservableProperty] private string searchText = "";
    [ObservableProperty] private DateTimeOffset? selectedDate = DateTimeOffset.Now;
    [ObservableProperty] private EstadoDia? estadoHoy;
    [ObservableProperty] private bool canEnter;
    [ObservableProperty] private bool canExit;
    [ObservableProperty] private string daySummary = "";
    [ObservableProperty] private string issuesSummary = "";
    [ObservableProperty] private bool hasIssues;
    [ObservableProperty] private string manualEntrada = "";
    [ObservableProperty] private string manualSalida = "";
    [ObservableProperty] private string manualMotivo = "";
    [ObservableProperty] private ChecadaEmpleado? selectedRecord;

    public AttendanceViewModel(IAppDialogService dialogs, AppState appState)
    {
        _dialogs = dialogs;
        ImportVM = new ImportAttendanceViewModel(dialogs, appState);
    }

    public void Refresh()
    {
        var selectedId = SelectedEmployee?.Id;
        var texto = SearchText.Trim().ToLower();
        Employees.Clear();
        foreach (var employee in _empleados.ObtenerTodos().Where(e =>
            e.Activo && (texto.Length == 0 ||
                e.Nombre.ToLower().Contains(texto) ||
                e.NumeroEmpleado.ToLower().Contains(texto))))
            Employees.Add(employee);
        SelectedEmployee = selectedId.HasValue
            ? Employees.FirstOrDefault(x => x.Id == selectedId)
            : Employees.FirstOrDefault();
        _ = RecargarAsync();
    }

    partial void OnSelectedEmployeeChanged(Empleado? value) => _ = RecargarAsync();
    partial void OnSelectedDateChanged(DateTimeOffset? value) => _ = RecargarAsync();
    partial void OnSearchTextChanged(string value) => Refresh();

    private async Task RecargarAsync()
    {
        try
        {
            var fecha = (SelectedDate ?? DateTimeOffset.Now).Date;
            var numero = SelectedEmployee?.NumeroEmpleado;

            var dia = await Task.Run(() => _checadas.ObtenerDelDia(fecha));
            DayRecords.Clear();
            foreach (var row in dia)
                DayRecords.Add(row);

            var presentes = dia.Count(c => c.PrimeraChecada.HasValue);
            DaySummary = $"Hoy: {presentes} con entrada · {fecha:dd/MM/yyyy}";

            var estado = await Task.Run(() => numero is null
                ? new EstadoDia(null)
                : _checadas.ObtenerEstadoDia(numero, fecha));
            EstadoHoy = estado;
            CanEnter = SelectedEmployee is not null && !estado.TieneEntrada;
            CanExit = estado.TieneEntrada && !estado.TieneSalida;

            var historial = await Task.Run(() =>
                _checadas.ObtenerRango(numero, fecha.AddDays(-6), fecha));
            History.Clear();
            foreach (var row in historial)
                History.Add(row);

            var (inicioSemana, finSemana) = ChecadasService.SemanaDe(fecha);
            var incidencias = await Task.Run(() =>
                _checadas.ObtenerIncidencias(inicioSemana, finSemana));
            Issues.Clear();
            foreach (var row in incidencias)
                Issues.Add(row);
            HasIssues = Issues.Count > 0;
            IssuesSummary = HasIssues
                ? $"{Issues.Count} incidencias esta semana ({inicioSemana:dd/MM}–{finSemana:dd/MM}). Revíselas antes de calcular nómina."
                : "Sin incidencias esta semana.";
        }
        catch (Exception ex)
        {
            await _dialogs.ShowInfoAsync("Asistencia", $"No se pudo cargar: {ex.Message}");
        }
    }

    [RelayCommand]
    private async Task RegistrarEntradaAsync()
    {
        if (SelectedEmployee is null)
        {
            await _dialogs.ShowInfoAsync("Asistencia", "Seleccione un empleado.");
            return;
        }

        var fecha = (SelectedDate ?? DateTimeOffset.Now).Date;
        if (fecha != DateTime.Today)
        {
            await _dialogs.ShowInfoAsync("Asistencia",
                "Para días pasados use la captura manual con la hora exacta.");
            return;
        }

        var resultado = await Task.Run(() => _checadas.RegistrarEntrada(
            SelectedEmployee.NumeroEmpleado, DateTime.Now, OrigenChecada.Dispositivo));
        await _dialogs.ShowInfoAsync("Asistencia", resultado.Mensaje);
        await RecargarAsync();
    }

    [RelayCommand]
    private async Task RegistrarSalidaAsync()
    {
        if (SelectedEmployee is null)
        {
            await _dialogs.ShowInfoAsync("Asistencia", "Seleccione un empleado.");
            return;
        }

        var fecha = (SelectedDate ?? DateTimeOffset.Now).Date;
        if (fecha != DateTime.Today)
        {
            await _dialogs.ShowInfoAsync("Asistencia",
                "Para días pasados use la captura manual con la hora exacta.");
            return;
        }

        var resultado = await Task.Run(() => _checadas.RegistrarSalida(
            SelectedEmployee.NumeroEmpleado, DateTime.Now, OrigenChecada.Dispositivo));
        await _dialogs.ShowInfoAsync("Asistencia", resultado.Mensaje);
        await RecargarAsync();
    }

    [RelayCommand]
    private async Task GuardarManualAsync()
    {
        if (SelectedEmployee is null)
        {
            await _dialogs.ShowInfoAsync("Asistencia", "Seleccione un empleado.");
            return;
        }

        TimeSpan? entrada = null, salida = null;
        if (!string.IsNullOrWhiteSpace(ManualEntrada))
        {
            if (!TimeSpan.TryParse(ManualEntrada.Trim(), out var horaEntrada))
            {
                await _dialogs.ShowInfoAsync("Hora incorrecta", "Use formato HH:mm. Ejemplo: 07:00.");
                return;
            }
            entrada = horaEntrada;
        }
        if (!string.IsNullOrWhiteSpace(ManualSalida))
        {
            if (!TimeSpan.TryParse(ManualSalida.Trim(), out var horaSalida))
            {
                await _dialogs.ShowInfoAsync("Hora incorrecta", "Use formato HH:mm. Ejemplo: 17:00.");
                return;
            }
            salida = horaSalida;
        }

        var fecha = (SelectedDate ?? DateTimeOffset.Now).Date;
        var resultado = await Task.Run(() => _checadas.AgregarManual(
            SelectedEmployee.NumeroEmpleado, fecha, entrada, salida, ManualMotivo, OrigenChecada.Manual));

        await _dialogs.ShowInfoAsync("Asistencia", resultado.Mensaje);
        if (resultado.Ok)
        {
            ManualEntrada = "";
            ManualSalida = "";
            ManualMotivo = "";
            await RecargarAsync();
        }
    }

    [RelayCommand]
    private async Task EliminarRegistroAsync()
    {
        if (SelectedRecord is null)
        {
            await _dialogs.ShowInfoAsync("Asistencia", "Seleccione el registro a eliminar.");
            return;
        }

        var accepted = await _dialogs.ConfirmAsync(
            "Eliminar checada",
            $"Se eliminará la checada de {SelectedRecord.Nombre} del {SelectedRecord.Fecha:dd/MM/yyyy}. Esta acción no se puede deshacer.",
            "Eliminar");
        if (!accepted)
            return;

        var ok = await Task.Run(() => _checadas.EliminarChecada(SelectedRecord.Id));
        await _dialogs.ShowInfoAsync("Asistencia",
            ok ? "Checada eliminada." : "No se encontró la checada. Recargue la vista.");
        await RecargarAsync();
    }
}
