using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using NominaPro.UI;

namespace NominaPro.ViewModels;

public sealed record NavEntry(string Key, string Label);

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly Dictionary<string, ViewModelBase> _pages;

    public AppState AppState { get; }

    public IReadOnlyList<NavEntry> NavItems { get; } =
    [
        new("Dashboard", "Resumen"),
        new("Empleados", "Empleados"),
        new("Checadas", "Importar checadas"),
        new("Horario", "Horario manual"),
        new("Nomina", "Nómina semanal"),
        new("Configuracion", "Configuración"),
    ];

    [ObservableProperty]
    private ViewModelBase currentPage;

    [ObservableProperty]
    private NavEntry selectedNav;

    [ObservableProperty]
    private string pageTitle = "Resumen";

    [ObservableProperty]
    private string currentTime = "";

    public MainWindowViewModel(IAppDialogService dialogs, AppState appState)
    {
        AppState = appState;
        _pages = new Dictionary<string, ViewModelBase>
        {
            ["Dashboard"] = new DashboardViewModel(),
            ["Empleados"] = new EmployeesViewModel(dialogs),
            ["Checadas"] = new ImportAttendanceViewModel(dialogs, appState),
            ["Horario"] = new ManualScheduleViewModel(dialogs),
            ["Nomina"] = new PayrollViewModel(dialogs),
            ["Configuracion"] = new SettingsViewModel(dialogs)
        };

        currentPage = _pages["Dashboard"];
        selectedNav = NavItems[0];
        RefreshCurrentPage();
        UpdateClock();

        var timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
        timer.Tick += (_, _) => UpdateClock();
        timer.Start();
    }

    partial void OnSelectedNavChanged(NavEntry value)
    {
        if (value is null || !_pages.TryGetValue(value.Key, out var page))
            return;

        CurrentPage = page;
        PageTitle = value.Label;
        RefreshCurrentPage();
    }

    private void RefreshCurrentPage()
    {
        if (CurrentPage is IRefreshable refreshable)
            refreshable.Refresh();
    }

    private void UpdateClock() => CurrentTime = DateTime.Now.ToString("dd/MM/yyyy  hh:mm:ss tt");
}

public interface IRefreshable
{
    void Refresh();
}
