using CommunityToolkit.Mvvm.ComponentModel;
using NamiSoft.Modelos;
using System.Collections.ObjectModel;

namespace NamiSoft.ViewModels;

public partial class ScheduleDayViewModel : ViewModelBase
{
    public string Day { get; }
    [ObservableProperty] private string entry = "00:00";
    [ObservableProperty] private string exit = "00:00";

    public ScheduleDayViewModel(string day) => Day = day;
}

public sealed class ScheduleEditorViewModel : ViewModelBase
{
    public ObservableCollection<ScheduleDayViewModel> Days { get; } = new();

    public ScheduleEditorViewModel(IReadOnlyList<HorarioEmpleado>? current)
    {
        string[] names = ["Lunes", "Martes", "Miércoles", "Jueves", "Viernes", "Sábado", "Domingo"];
        foreach (var name in names)
        {
            var existing = current?.FirstOrDefault(x => x.DiaSemana == name);
            Days.Add(new ScheduleDayViewModel(name)
            {
                Entry = existing?.HoraEntrada.ToString(@"hh\:mm") ?? "00:00",
                Exit = existing?.HoraSalida.ToString(@"hh\:mm") ?? "00:00"
            });
        }
    }

    public bool TryBuild(out List<HorarioEmpleado> result, out string error)
    {
        result = new List<HorarioEmpleado>();
        error = "";

        foreach (var day in Days)
        {
            if (!TimeSpan.TryParse(day.Entry, out var entry) || !TimeSpan.TryParse(day.Exit, out var exit))
            {
                error = $"El horario de {day.Day} no es válido. Use HH:mm, por ejemplo 07:00 o 18:30.";
                result.Clear();
                return false;
            }

            result.Add(new HorarioEmpleado
            {
                DiaSemana = day.Day,
                HoraEntrada = entry,
                HoraSalida = exit
            });
        }

        return true;
    }
}
