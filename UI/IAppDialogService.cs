using NamiSoft.Modelos;

namespace NamiSoft.UI;

public interface IAppDialogService
{
    Task ShowInfoAsync(string title, string message);
    Task<bool> ConfirmAsync(string title, string message, string acceptText = "Aceptar");
    Task<string?> PickExcelFileAsync();
    Task<string?> PickSaveExcelFileAsync(string suggestedName);
    Task<bool> EditEmployeeAsync(Empleado? employee);
    Task<List<HorarioEmpleado>?> EditPersonalScheduleAsync(string numeroEmpleado, IReadOnlyList<HorarioEmpleado>? current);
}
