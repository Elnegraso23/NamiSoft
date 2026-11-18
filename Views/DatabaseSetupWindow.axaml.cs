using Avalonia.Controls;
using Avalonia.Interactivity;
using NamiSoft.Datos;

namespace NamiSoft.Views;

public partial class DatabaseSetupWindow : Window
{
    private readonly Action _onConnected;

    public DatabaseSetupWindow() : this("No se encontró configuración de SQL Server.", () => { })
    {
    }

    public DatabaseSetupWindow(string initialError, Action onConnected)
    {
        _onConnected = onConnected;
        InitializeComponent();

        if (string.IsNullOrWhiteSpace(initialError))
        {
            ErrorBanner.IsVisible = false;
        }
        else
        {
            ErrorText.Text = initialError;
            ErrorBanner.IsVisible = true;
        }
    }

    private void OnAuthModeChanged(object? sender, RoutedEventArgs e)
    {
        var winAuth = WindowsAuthCheck.IsChecked == true;
        PasswordPanel.IsVisible = !winAuth;
    }

    private async void OnConnectClick(object? sender, RoutedEventArgs e)
    {
        var servidor = ServerBox.Text ?? string.Empty;
        var baseDatos = DatabaseBox.Text ?? string.Empty;
        var password = PasswordBox.Text;
        var winAuth = WindowsAuthCheck.IsChecked == true;

        ConnectButton.IsEnabled = false;
        ErrorBanner.IsVisible = false;
        StatusText.Text = "Probando conexión y aplicando migraciones...";
        StatusText.IsVisible = true;

        try
        {
            await Task.Run(() => NominaDatabase.ConfigurarYProbar(servidor, baseDatos, password, winAuth));
            _onConnected();
            Close();
        }
        catch (Exception ex)
        {
            ErrorText.Text = ex.Message;
            ErrorBanner.IsVisible = true;
            StatusText.IsVisible = false;
            ConnectButton.IsEnabled = true;
        }
    }

    private void OnExitClick(object? sender, RoutedEventArgs e)
    {
        Close();
    }
}
