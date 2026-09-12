using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Platform.Storage;
using NamiSoft.Modelos;
using NamiSoft.ViewModels;
using NamiSoft.Views;

namespace NamiSoft.UI;

public sealed class AppDialogService : IAppDialogService
{
    private readonly Func<Window> _owner;

    public AppDialogService(Func<Window> owner) => _owner = owner;

    public async Task ShowInfoAsync(string title, string message)
    {
        var dialog = CreateMessageDialog(title, message, false, "Aceptar");
        await dialog.ShowDialog<bool>(_owner());
    }

    public async Task<bool> ConfirmAsync(string title, string message, string acceptText = "Aceptar")
    {
        var dialog = CreateMessageDialog(title, message, true, acceptText);
        return await dialog.ShowDialog<bool>(_owner());
    }

    public async Task<string?> PickExcelFileAsync()
    {
        var files = await _owner().StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Seleccionar archivo de checadas",
            AllowMultiple = false,
            FileTypeFilter =
            [
                new FilePickerFileType("Archivos de Excel") { Patterns = ["*.xlsx"] }
            ]
        });

        return files.FirstOrDefault()?.TryGetLocalPath();
    }

    public async Task<string?> PickSaveExcelFileAsync(string suggestedName)
    {
        var file = await _owner().StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
        {
            Title = "Guardar nómina",
            SuggestedFileName = suggestedName,
            DefaultExtension = "xlsx",
            FileTypeChoices =
            [
                new FilePickerFileType("Archivo Excel") { Patterns = ["*.xlsx"] }
            ]
        });

        return file?.TryGetLocalPath();
    }

    public async Task<bool> EditEmployeeAsync(Empleado? employee)
    {
        var vm = new EmployeeEditorViewModel(this, employee);
        var window = new EmployeeEditorWindow { DataContext = vm };
        return await window.ShowDialog<bool>(_owner());
    }

    public async Task<List<HorarioEmpleado>?> EditPersonalScheduleAsync(
        string numeroEmpleado,
        IReadOnlyList<HorarioEmpleado>? current)
    {
        IReadOnlyList<HorarioEmpleado>? source = current;
        if ((source is null || source.Count == 0) && !string.IsNullOrWhiteSpace(numeroEmpleado))
        {
            source = new Servicios.EmpleadoService().ObtenerHorarioPersonalizado(numeroEmpleado);
        }

        var vm = new ScheduleEditorViewModel(source);
        var window = new ScheduleEditorWindow { DataContext = vm };
        return await window.ShowDialog<List<HorarioEmpleado>?>(_owner());
    }

    private static Window CreateMessageDialog(string title, string message, bool confirm, string acceptText)
    {
        var dialog = new Window
        {
            Title = title,
            Width = 460,
            MinHeight = 210,
            SizeToContent = SizeToContent.Height,
            CanResize = false,
            WindowStartupLocation = WindowStartupLocation.CenterOwner
        };

        var messageBlock = new TextBlock
        {
            Text = message,
            TextWrapping = TextWrapping.Wrap,
            FontSize = 15
        };

        var buttons = new StackPanel
        {
            Orientation = Orientation.Horizontal,
            HorizontalAlignment = HorizontalAlignment.Right,
            Spacing = 10
        };

        if (confirm)
        {
            var cancel = new Button { Content = "Cancelar", MinWidth = 100 };
            cancel.Click += (_, _) => dialog.Close(false);
            buttons.Children.Add(cancel);
        }

        var accept = new Button
        {
            Content = acceptText,
            MinWidth = 110,
            Background = new SolidColorBrush(Color.Parse(confirm && acceptText == "Eliminar" ? "#B91C1C" : "#2563EB")),
            Foreground = Brushes.White
        };
        accept.Click += (_, _) => dialog.Close(true);
        buttons.Children.Add(accept);

        dialog.Content = new Border
        {
            Padding = new Avalonia.Thickness(24),
            Child = new StackPanel
            {
                Spacing = 24,
                Children = { messageBlock, buttons }
            }
        };

        return dialog;
    }
}
