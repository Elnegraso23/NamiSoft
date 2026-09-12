using NominaPro.Datos;

namespace NominaPro.Servicios;

public sealed record DashboardResumen(int EmpleadosActivos, int EmpleadosTotales, int Checadas, int Pendientes);

public sealed class DashboardService
{
    public DashboardResumen Obtener()
    {
        using var db = new NominaContext();
        return new DashboardResumen(
            db.Empleados.Count(e => e.Activo),
            db.Empleados.Count(),
            db.Checadas.Count(),
            db.ChecadasPendientes.Count());
    }
}
