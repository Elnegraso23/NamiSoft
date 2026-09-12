using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NominaPro.Modelos;
using NominaPro.Servicios;
using NominaPro.UI;

namespace NominaPro.ViewModels;

public partial class SettingsViewModel : ViewModelBase, IRefreshable
{
    private readonly IAppDialogService _dialogs;
    private readonly ConfiguracionService _service = new();

    [ObservableProperty] private TimeSpan? morningEntry;
    [ObservableProperty] private TimeSpan? morningExit;
    [ObservableProperty] private decimal morningTolerance;
    [ObservableProperty] private TimeSpan? afternoonEntry;
    [ObservableProperty] private TimeSpan? afternoonExit;
    [ObservableProperty] private decimal afternoonTolerance;
    [ObservableProperty] private decimal everyLateCount;
    [ObservableProperty] private decimal lateDiscountMinutes;
    [ObservableProperty] private bool applySundayPremium;
    [ObservableProperty] private decimal sundayPremiumPercent;

    public SettingsViewModel(IAppDialogService dialogs) => _dialogs = dialogs;

    public void Refresh()
    {
        var config = _service.ObtenerOCrear();
        MorningEntry = config.EntradaManana;
        MorningExit = config.SalidaManana;
        MorningTolerance = config.ToleranciaManana;
        AfternoonEntry = config.EntradaTarde;
        AfternoonExit = config.SalidaTarde;
        AfternoonTolerance = config.ToleranciaTarde;
        EveryLateCount = config.CadaRetardos;
        LateDiscountMinutes = config.DescuentoRetardosMinutos;
        ApplySundayPremium = config.AplicarPrimaDominical;
        SundayPremiumPercent = config.PorcentajePrimaDominical;
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        var config = new ConfiguracionSistema
        {
            EntradaManana = MorningEntry ?? new TimeSpan(7, 0, 0),
            SalidaManana = MorningExit ?? new TimeSpan(17, 0, 0),
            ToleranciaManana = (int)MorningTolerance,
            EntradaTarde = AfternoonEntry ?? new TimeSpan(12, 0, 0),
            SalidaTarde = AfternoonExit ?? new TimeSpan(22, 0, 0),
            ToleranciaTarde = (int)AfternoonTolerance,
            CadaRetardos = (int)EveryLateCount,
            DescuentoRetardosMinutos = (int)LateDiscountMinutes,
            AplicarPrimaDominical = ApplySundayPremium,
            PorcentajePrimaDominical = SundayPremiumPercent
        };

        _service.Guardar(config);
        await _dialogs.ShowInfoAsync("Configuración", "La configuración se guardó correctamente.");
    }
}
