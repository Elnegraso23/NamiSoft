using Avalonia.Controls;
using Avalonia.Interactivity;
using NamiSoft.Modelos;
using NamiSoft.UI;
using NamiSoft.ViewModels;

namespace NamiSoft.Views;

public partial class ScheduleEditorWindow : Window
{
    public ScheduleEditorWindow() => InitializeComponent();

    private void OnCancelClick(object? sender, RoutedEventArgs e) => Close((List<HorarioEmpleado>?)null);

    private async void OnSaveClick(object? sender, RoutedEventArgs e)
    {
        if (DataContext is not ScheduleEditorViewModel vm)
            return;

        if (!vm.TryBuild(out var result, out var error))
        {
            var owner = Owner as Window ?? this;
            var service = new AppDialogService(() => owner);
            await service.ShowInfoAsync("Horario inválido", error);
            return;
        }

        Close(result);
    }
}
