using Microsoft.EntityFrameworkCore;
using NominaPro.Datos;
using NominaPro.Modelos;

namespace NominaPro.Servicios;

public sealed class HorarioConsultaService
{
    public List<HorarioManualItem> ObtenerSemana(string numeroEmpleado, DateTime fecha)
    {
        int diferencia = fecha.DayOfWeek == DayOfWeek.Sunday ? 6 : (int)fecha.DayOfWeek - 1;
        DateTime inicio = fecha.Date.AddDays(-diferencia);
        DateTime fin = inicio.AddDays(6);

        using var db = NominaDatabase.CreateContext();
        return db.Checadas.AsNoTracking()
            .Where(c => c.NumeroEmpleado == numeroEmpleado && c.Fecha >= inicio && c.Fecha <= fin)
            .OrderBy(c => c.Fecha)
            .ToList()
            .Select(c => new HorarioManualItem
            {
                Id = c.Id,
                Fecha = c.Fecha,
                Entrada = c.PrimeraChecada.HasValue ? c.PrimeraChecada.Value.ToString(@"hh\:mm") : "",
                Salida = c.UltimaChecada.HasValue ? c.UltimaChecada.Value.ToString(@"hh\:mm") : "",
                HorasTrabajadas = c.HorasTrabajadas,
                RetardoMinutos = c.RetardoMinutos,
                TieneRetardo = c.TieneRetardo,
                Modificado = c.ModificadaManual
            })
            .ToList();
    }

    public int ContarChecadas()
    {
        using var db = NominaDatabase.CreateContext();
        return db.Checadas.Count();
    }
}
