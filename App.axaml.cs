using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.EntityFrameworkCore;
using NominaPro.Datos;
using NominaPro.UI;
using NominaPro.ViewModels;
using NominaPro.Views;

namespace NominaPro;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            NominaDatabase.EnsureMigrated();

            var mainWindow = new MainWindow();
            var dialogs = new AppDialogService(() => mainWindow);
            var appState = new AppState();

            mainWindow.DataContext = new MainWindowViewModel(dialogs, appState);
            desktop.MainWindow = mainWindow;
        }

        base.OnFrameworkInitializationCompleted();
    }
}
