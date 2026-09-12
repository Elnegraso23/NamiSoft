using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NominaPro.Modelos;
using NominaPro.Servicios;
using NominaPro.UI;
using System.Collections.ObjectModel;

namespace NominaPro.ViewModels;

public partial class PayrollViewModel : ViewModelBase
{
    private readonly IAppDialogService _dialogs;
    private readonly NominaService _payroll = new();

    public ObservableCollection<NominaEmpleado> Rows { get; } = new();

    [ObservableProperty] private DateTimeOffset? weekDate = DateTimeOffset.Now;
    [ObservableProperty] private decimal tips;
    [ObservableProperty] private decimal totalPayroll;
    [ObservableProperty] private bool canExport;
    [ObservableProperty] private bool hasRows;

    public PayrollViewModel(IAppDialogService dialogs) => _dialogs = dialogs;

    [RelayCommand]
    private async Task CalculateAsync()
    {
        var date = (WeekDate ?? DateTimeOffset.Now).DateTime;
        var tips = Tips;
        var data = await Task.Run(() => _payroll.CalcularNomina(date, tips));
        Rows.Clear();
        foreach (var row in data)
            Rows.Add(row);
        TotalPayroll = data.Sum(x => x.TotalPagar);
        CanExport = data.Count > 0;
        HasRows = data.Count > 0;
    }

    [RelayCommand]
    private async Task ExportAsync()
    {
        if (!CanExport)
        {
            await _dialogs.ShowInfoAsync("Nómina", "Calcule la nómina antes de exportar.");
            return;
        }

        var date = (WeekDate ?? DateTimeOffset.Now).DateTime;
        var path = await _dialogs.PickSaveExcelFileAsync($"Nomina Casa Red {date:dd-MM-yyyy}.xlsx");
        if (string.IsNullOrWhiteSpace(path))
            return;

        var data = await Task.Run(() => _payroll.CalcularNomina(date, Tips));
        await Task.Run(() => new ExcelNominaService().ExportarNomina(data, date, Tips, path));
        await _dialogs.ShowInfoAsync("Nómina", "La nómina se exportó correctamente.");
    }
}
