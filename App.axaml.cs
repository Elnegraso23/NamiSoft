using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.EntityFrameworkCore;
using NamiSoft.Datos;
using NamiSoft.UI;
using NamiSoft.ViewModels;
using NamiSoft.Views;

namespace NamiSoft;

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
