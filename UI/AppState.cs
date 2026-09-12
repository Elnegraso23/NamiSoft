using CommunityToolkit.Mvvm.ComponentModel;

namespace NamiSoft.UI;

public partial class AppState : ObservableObject
{
    [ObservableProperty]
    private bool archivoCargado;

    [ObservableProperty]
    private string nombreArchivo = "Ninguno";

    public string EstadoArchivo => ArchivoCargado
        ? $"Archivo cargado: {NombreArchivo}"
        : "Sin archivo de checadas cargado";

    partial void OnArchivoCargadoChanged(bool value) => OnPropertyChanged(nameof(EstadoArchivo));
    partial void OnNombreArchivoChanged(string value) => OnPropertyChanged(nameof(EstadoArchivo));
}
