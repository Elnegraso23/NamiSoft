using Avalonia.Controls;
using Avalonia.Interactivity;
using NominaPro.ViewModels;

namespace NominaPro.Views;

public partial class EmployeeEditorWindow : Window
{
    public EmployeeEditorWindow() => InitializeComponent();

    private void OnCancelClick(object? sender, RoutedEventArgs e) => Close(false);

    private async void OnSaveClick(object? sender, RoutedEventArgs e)
    {
        if (DataContext is EmployeeEditorViewModel vm && await vm.SaveAsync())
            Close(true);
    }
}
