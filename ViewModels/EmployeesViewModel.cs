using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NominaPro.Modelos;
using NominaPro.Servicios;
using NominaPro.UI;
using System.Collections.ObjectModel;

namespace NominaPro.ViewModels;

public partial class EmployeesViewModel : ViewModelBase, IRefreshable
{
    private readonly IAppDialogService _dialogs;
    private readonly EmpleadoService _service = new();

    public ObservableCollection<Empleado> Employees { get; } = new();
    public IReadOnlyList<string> StatusFilters { get; } = ["Activos", "Inactivos", "Todos"];

    [ObservableProperty] private string selectedStatusFilter = "Activos";
    [ObservableProperty] private string searchText = "";
    [ObservableProperty] private Empleado? selectedEmployee;
    [ObservableProperty] private bool hasEmployees;

    public EmployeesViewModel(IAppDialogService dialogs) => _dialogs = dialogs;

    partial void OnSelectedStatusFilterChanged(string value) => Refresh();
    partial void OnSearchTextChanged(string value) => Refresh();

    public void Refresh()
    {
        Employees.Clear();
        foreach (var employee in _service.Obtener(SelectedStatusFilter, SearchText))
            Employees.Add(employee);
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
}
