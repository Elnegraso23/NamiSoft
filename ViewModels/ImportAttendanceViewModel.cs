using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NominaPro.Modelos;
using NominaPro.Servicios;
using NominaPro.UI;
using NominaPro.Utilidades;
using System.Collections.ObjectModel;
using System.IO;

namespace NominaPro.ViewModels;

public partial class ImportAttendanceViewModel : ViewModelBase
{
    private readonly IAppDialogService _dialogs;
    private readonly AppState _appState;

    public ObservableCollection<ChecadaImportada> Preview { get; } = new();

    [ObservableProperty] private string filePath = "";
    [ObservableProperty] private string status = "Seleccione un archivo .xlsx de checadas.";
    [ObservableProperty] private bool canImport;

    public ImportAttendanceViewModel(IAppDialogService dialogs, AppState appState)
    {
        _dialogs = dialogs;
        _appState = appState;
    }

    [RelayCommand]
    private async Task SelectFileAsync()
    {
        var path = await _dialogs.PickExcelFileAsync();
        if (string.IsNullOrWhiteSpace(path))
            return;

        FilePath = path;
        await LoadPreviewAsync();
    }

    private async Task LoadPreviewAsync()
    {
        try
        {
            Status = "Leyendo archivo…";
            var rows = await Task.Run(() => new ExcelHelper().LeerReporteAsistencia(FilePath));
            Preview.Clear();
            foreach (var row in rows.Take(100))
                Preview.Add(row);
            CanImport = rows.Count > 0;
            Status = $"Archivo listo: {Path.GetFileName(FilePath)} · {rows.Count} registros detectados";
        }
        catch (IOException)
        {
            CanImport = false;
            Status = "El archivo está abierto o en uso.";
            await _dialogs.ShowInfoAsync("Archivo en uso", "Cierre el archivo de Excel y vuelva a intentarlo.");
        }
        catch (Exception ex)
        {
            CanImport = false;
            Status = "No se pudo leer el archivo.";
            await _dialogs.ShowInfoAsync("Error", ex.Message);
        }
    }

    [RelayCommand]
    private async Task ImportAsync()
    {
        if (!CanImport || string.IsNullOrWhiteSpace(FilePath))
            return;

        try
        {
            Status = "Importando checadas…";
            var rows = await Task.Run(() => new ExcelHelper().LeerReporteAsistencia(FilePath));
            var result = await Task.Run(() => new ChecadasService().GuardarChecadas(rows));

            EstadoSistema.ArchivoCargado = true;
            EstadoSistema.NombreArchivo = Path.GetFileName(FilePath);
            _appState.ArchivoCargado = true;
            _appState.NombreArchivo = Path.GetFileName(FilePath);

            Status = result;
            await _dialogs.ShowInfoAsync("Importación terminada", result);
        }
        catch (IOException)
        {
            await _dialogs.ShowInfoAsync("Archivo en uso", "Cierre el archivo de Excel y vuelva a intentarlo.");
        }
        catch (Exception ex)
        {
            await _dialogs.ShowInfoAsync("Error al importar", ex.Message);
        }
    }
}
