using NominaPro.Modelos;

namespace NominaPro.Servicios;

/// <summary>
/// Corrige checadas existentes por semana. Delega el cálculo en ChecadasService
/// para no duplicar reglas.
/// </summary>
public class HorarioManualService
{
    private readonly ChecadasService _checadas = new();

    public string GuardarCambios(List<HorarioManualItem> lista)
    {
        int modificados = 0;

        foreach (var item in lista)
        {
            TimeSpan? entrada = null;
            TimeSpan? salida = null;

            if (!string.IsNullOrWhiteSpace(item.Entrada))
            {
                if (!TimeSpan.TryParse(item.Entrada, out TimeSpan horaEntrada))
                {
                    return $"La hora de entrada '{item.Entrada}' del día {item.Fecha:dd/MM/yyyy} no es válida.\n\nUtilice el formato HH:mm\n\nEjemplo: 07:00";
                }

                entrada = horaEntrada;
            }

            if (!string.IsNullOrWhiteSpace(item.Salida))
            {
                if (!TimeSpan.TryParse(item.Salida, out TimeSpan horaSalida))
                {
                    return $"La hora de salida '{item.Salida}' del día {item.Fecha:dd/MM/yyyy} no es válida.\n\nUtilice el formato HH:mm\n\nEjemplo: 22:00";
                }

                salida = horaSalida;
            }

            var resultado = _checadas.EditarChecada(item.Id, entrada, salida, null);
            if (resultado != "OK")
                return resultado;

            modificados++;
        }

        return $"Se actualizaron correctamente {modificados} registros.";
    }
}
