using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NamiSoft.Modelos;
using NamiSoft.Servicios;
using NamiSoft.UI;
using System.Collections.ObjectModel;

namespace NamiSoft.ViewModels;

public partial class EmployeesViewModel : ViewModelBase, IRefreshable
{
    private readonly IAppDialogService _dialogs;
    private readonly EmpleadoService _service = new();

    public ObservableCollection<Empleado> Employees { get; } = new();
    public ObservableCollection<string> DepartmentFilters { get; } = new() { "Todos" };
    public IReadOnlyList<string> StatusFilters { get; } = ["Activos", "Inactivos", "Todos"];
    public IReadOnlyList<string> ShiftFilters { get; } = ["Todos", "Mañana", "Tarde", "Personalizado", "Especial"];
    public IReadOnlyList<string> SortOptions { get; } =
    [
        "Número",
        "Nombre (A-Z)",
        "Salario (mayor a menor)",
        "Salario (menor a mayor)",
        "Departamento"
    ];

    [ObservableProperty] private string selectedStatusFilter = "Activos";
    [ObservableProperty] private string selectedDepartmentFilter = "Todos";
    [ObservableProperty] private string selectedShiftFilter = "Todos";
    [ObservableProperty] private string selectedSortOption = "Número";
    [ObservableProperty] private string searchText = "";
    [ObservableProperty] private Empleado? selectedEmployee;
    [ObservableProperty] private bool hasEmployees;

    public EmployeesViewModel(IAppDialogService dialogs) => _dialogs = dialogs;

    partial void OnSelectedStatusFilterChanged(string value) => Refresh();
    partial void OnSelectedDepartmentFilterChanged(string value) => Refresh();
    partial void OnSelectedShiftFilterChanged(string value) => Refresh();
    partial void OnSelectedSortOptionChanged(string value) => Refresh();
    partial void OnSearchTextChanged(string value) => Refresh();

    public void Refresh()
    {
        var depts = _service.ObtenerDepartamentos();
        var currentDept = SelectedDepartmentFilter;
        DepartmentFilters.Clear();
        DepartmentFilters.Add("Todos");
        foreach (var d in depts)
            DepartmentFilters.Add(d);
        SelectedDepartmentFilter = DepartmentFilters.Contains(currentDept) ? currentDept : "Todos";

        Employees.Clear();
        foreach (var employee in _service.Obtener(
            SelectedStatusFilter,
            SearchText,
            SelectedDepartmentFilter,
            SelectedShiftFilter,
            SelectedSortOption))
        {
            Employees.Add(employee);
        }
        HasEmployees = Employees.Count > 0;
    }

    [RelayCommand]
    private async Task NewEmployeeAsync()
    {
        if (await _dialogs.EditEmployeeAsync(null))
            Refresh();
    }

    [RelayCommand]
    private async Task EditEmployeeAsync()
    {
        if (SelectedEmployee is null)
        {
            await _dialogs.ShowInfoAsync("Empleados", "Seleccione un empleado.");
            return;
        }

        var employee = _service.ObtenerPorId(SelectedEmployee.Id);
        if (employee is not null && await _dialogs.EditEmployeeAsync(employee))
            Refresh();
    }

    [RelayCommand]
    private async Task ToggleActiveAsync()
    {
        if (SelectedEmployee is null)
        {
            await _dialogs.ShowInfoAsync("Empleados", "Seleccione un empleado.");
            return;
        }

        if (!await Task.Run(() => _service.AlternarActivo(SelectedEmployee.Id)))
        {
            await _dialogs.ShowInfoAsync("Empleados", "No se encontró el empleado. Recargue la lista.");
            return;
        }

        Refresh();
    }

    [RelayCommand]
    private async Task DeleteEmployeeAsync()
    {
        if (SelectedEmployee is null)
        {
            await _dialogs.ShowInfoAsync("Empleados", "Seleccione un empleado.");
            return;
        }

        var accepted = await _dialogs.ConfirmAsync(
            "Eliminar empleado",
            $"¿Eliminar a {SelectedEmployee.Nombre}? Esta acción no se puede deshacer.",
            "Eliminar");

        if (!accepted)
            return;

        if (!await Task.Run(() => _service.Eliminar(SelectedEmployee.Id)))
        {
            await _dialogs.ShowInfoAsync("Empleados", "No se encontró el empleado. Recargue la lista.");
            return;
        }

        Refresh();
    }

    [RelayCommand]
    private async Task SyncPendingAsync()
    {
        var accepted = await _dialogs.ConfirmAsync(
            "Actualizar historial",
            "Se buscarán checadas pendientes para todos los empleados registrados. ¿Desea continuar?",
            "Actualizar");

        if (!accepted)
            return;

        var result = await Task.Run(() => new SincronizarChecadasPendientesService().Sincronizar());
        await _dialogs.ShowInfoAsync("Proceso terminado", result);
        Refresh();
    }

    [RelayCommand]
    private async Task SeedSampleDataAsync()
    {
        var accepted = await _dialogs.ConfirmAsync(
            "Cargar datos de prueba",
            "Se agregarán empleados y checadas de ejemplo (sin duplicar los ya existentes). ¿Continuar?",
            "Cargar datos");

        if (!accepted)
            return;

        var result = await Task.Run(() => _service.GenerarDatosPrueba());
        await _dialogs.ShowInfoAsync("Datos de prueba", result);
        Refresh();
    }
}
