using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NominaPro.Modelos;
using NominaPro.Servicios;
using NominaPro.UI;
using System.Collections.ObjectModel;

namespace NominaPro.ViewModels;

public partial class ManualScheduleViewModel : ViewModelBase, IRefreshable
{
    private readonly IAppDialogService _dialogs;
    private readonly EmpleadoService _employeeService = new();
    private readonly HorarioConsultaService _query = new();

    public ObservableCollection<Empleado> Employees { get; } = new();
    public ObservableCollection<HorarioManualItem> Rows { get; } = new();

    [ObservableProperty] private Empleado? selectedEmployee;
    [ObservableProperty] private DateTimeOffset? weekDate = DateTimeOffset.Now;
    [ObservableProperty] private bool hasRows;

    public ManualScheduleViewModel(IAppDialogService dialogs) => _dialogs = dialogs;

    public void Refresh()
    {
        var selectedId = SelectedEmployee?.Id;
        Employees.Clear();
        foreach (var employee in _employeeService.ObtenerTodos())
            Employees.Add(employee);
        SelectedEmployee = selectedId.HasValue ? Employees.FirstOrDefault(x => x.Id == selectedId) : null;
    }

    [RelayCommand]
    private async Task LoadAsync()
    {
        if (SelectedEmployee is null)
        {
            await _dialogs.ShowInfoAsync("Horario", "Seleccione un empleado.");
            return;
        }

        Rows.Clear();
        var date = (WeekDate ?? DateTimeOffset.Now).DateTime;
        var rows = await Task.Run(() => _query.ObtenerSemana(SelectedEmployee.NumeroEmpleado, date));
        foreach (var item in rows)
            Rows.Add(item);
        HasRows = Rows.Count > 0;
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        if (Rows.Count == 0)
        {
            await _dialogs.ShowInfoAsync("Horario", "No hay checadas cargadas para guardar.");
            return;
        }

        foreach (var row in Rows)
        {
            if ((!string.IsNullOrWhiteSpace(row.Entrada) && !TimeSpan.TryParse(row.Entrada, out _)) ||
                (!string.IsNullOrWhiteSpace(row.Salida) && !TimeSpan.TryParse(row.Salida, out _)))
            {
                await _dialogs.ShowInfoAsync("Hora incorrecta", "Use formato HH:mm. Ejemplos: 07:00, 14:30, 22:00.");
                return;
            }
        }

        var rows = Rows.ToList();
        var result = await Task.Run(() => new HorarioManualService().GuardarCambios(rows));
        await _dialogs.ShowInfoAsync("Horario", result);
        await LoadAsync();
    }
}
