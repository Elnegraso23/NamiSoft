using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NominaPro.Enumeraciones;
using NominaPro.Modelos;
using NominaPro.Servicios;
using NominaPro.UI;

namespace NominaPro.ViewModels;

public partial class EmployeeEditorViewModel : ViewModelBase
{
    private readonly IAppDialogService _dialogs;
    private readonly EmpleadoService _service = new();
    private readonly int _id;
    private List<HorarioEmpleado> _personalSchedule = new();

    public IReadOnlyList<string> Shifts { get; } = ["Mañana", "Tarde", "Personalizado", "Especial"];
    public IReadOnlyList<string> DaysOff { get; } = ["Lunes", "Martes", "Miércoles", "Jueves", "Viernes", "Sábado", "Domingo", "Ninguno"];

    [ObservableProperty] private string numeroEmpleado = "";
    [ObservableProperty] private string nombre = "";
    [ObservableProperty] private string puesto = "";
    [ObservableProperty] private string departamento = "";
    [ObservableProperty] private decimal salarioHora;
    [ObservableProperty] private string selectedShift = "Mañana";
    [ObservableProperty] private string selectedDayOff = "Domingo";
    [ObservableProperty] private bool activo = true;
    [ObservableProperty] private bool isPersonalized;
    [ObservableProperty] private string scheduleSummary = "Sin horario personalizado en borrador";

    public string WindowTitle => _id == 0 ? "Nuevo empleado" : "Editar empleado";
    public string SaveText => _id == 0 ? "Guardar empleado" : "Actualizar empleado";

    public EmployeeEditorViewModel(IAppDialogService dialogs, Empleado? employee)
    {
        _dialogs = dialogs;
        _id = employee?.Id ?? 0;

        if (employee is not null)
        {
            NumeroEmpleado = employee.NumeroEmpleado;
            Nombre = employee.Nombre;
            Puesto = employee.Puesto;
            Departamento = employee.Departamento;
            SalarioHora = employee.SalarioHora;
            SelectedShift = employee.TipoHorario switch
            {
                TipoHorario.Mañana => "Mañana",
                TipoHorario.Tarde => "Tarde",
                TipoHorario.Personalizado => "Personalizado",
                TipoHorario.Especial => "Especial",
                _ => "Mañana"
            };
            SelectedDayOff = employee.Descanso;
            Activo = employee.Activo;

            if (employee.TipoHorario == TipoHorario.Personalizado)
                _personalSchedule = _service.ObtenerHorarioPersonalizado(employee.NumeroEmpleado);
        }

        UpdatePersonalizedState();
    }

    partial void OnSelectedShiftChanged(string value) => UpdatePersonalizedState();

    private void UpdatePersonalizedState()
    {
        IsPersonalized = SelectedShift == "Personalizado";
        ScheduleSummary = _personalSchedule.Count > 0
            ? $"Horario personalizado: {_personalSchedule.Count} días configurados"
            : "Sin horario personalizado en borrador";
    }

    [RelayCommand]
    private async Task EditScheduleAsync()
    {
        var result = await _dialogs.EditPersonalScheduleAsync(NumeroEmpleado, _personalSchedule);
        if (result is null)
            return;

        _personalSchedule = result;
        UpdatePersonalizedState();
    }

    public async Task<bool> SaveAsync()
    {
        var employee = new Empleado
        {
            Id = _id,
            NumeroEmpleado = NumeroEmpleado,
            Nombre = Nombre,
            Puesto = Puesto,
            Departamento = Departamento,
            SalarioHora = SalarioHora,
            TipoHorario = SelectedShift switch
            {
                "Mañana" => TipoHorario.Mañana,
                "Tarde" => TipoHorario.Tarde,
                "Personalizado" => TipoHorario.Personalizado,
                "Especial" => TipoHorario.Especial,
                _ => TipoHorario.Mañana
            },
            Turno = SelectedShift,
            Descanso = SelectedDayOff,
            Activo = Activo
        };

        var result = _service.Guardar(employee, _personalSchedule);
        if (result != "OK")
        {
            await _dialogs.ShowInfoAsync("No se pudo guardar", result);
            return false;
        }

        await _dialogs.ShowInfoAsync("Empleados", "Empleado guardado correctamente.");
        return true;
    }
}
