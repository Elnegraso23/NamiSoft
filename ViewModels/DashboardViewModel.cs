using CommunityToolkit.Mvvm.ComponentModel;
using NamiSoft.Servicios;

namespace NamiSoft.ViewModels;

public partial class DashboardViewModel : ViewModelBase, IRefreshable
{
    private readonly DashboardService _service = new();

    [ObservableProperty] private int empleadosActivos;
    [ObservableProperty] private int empleadosTotales;
    [ObservableProperty] private int checadas;
    [ObservableProperty] private int pendientes;

    public void Refresh()
    {
        var resumen = _service.Obtener();
        EmpleadosActivos = resumen.EmpleadosActivos;
        EmpleadosTotales = resumen.EmpleadosTotales;
        Checadas = resumen.Checadas;
        Pendientes = resumen.Pendientes;
    }
}
