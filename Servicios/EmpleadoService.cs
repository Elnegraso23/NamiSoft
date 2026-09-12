using Microsoft.EntityFrameworkCore;
using NominaPro.Datos;
using NominaPro.Enumeraciones;
using NominaPro.Modelos;

namespace NominaPro.Servicios;

public sealed class EmpleadoService
{
    public List<Empleado> Obtener(string filtroEstado, string busqueda)
    {
        using var db = new NominaContext();
        var query = db.Empleados.AsNoTracking().AsQueryable();

        query = filtroEstado switch
        {
            "Activos" => query.Where(e => e.Activo),
            "Inactivos" => query.Where(e => !e.Activo),
            _ => query
        };

        if (!string.IsNullOrWhiteSpace(busqueda))
        {
            var texto = busqueda.Trim().ToLower();
            query = query.Where(e =>
                e.Nombre.ToLower().Contains(texto) ||
                e.NumeroEmpleado.ToLower().Contains(texto) ||
                e.Puesto.ToLower().Contains(texto) ||
                e.Departamento.ToLower().Contains(texto));
        }

        return query.OrderBy(e => e.NumeroEmpleado).ToList();
    }

    public Empleado? ObtenerPorId(int id)
    {
        using var db = new NominaContext();
        return db.Empleados.AsNoTracking().FirstOrDefault(e => e.Id == id);
    }

    public List<Empleado> ObtenerTodos()
    {
        using var db = new NominaContext();
        return db.Empleados.AsNoTracking().OrderBy(e => e.NumeroEmpleado).ToList();
    }

    public List<HorarioEmpleado> ObtenerHorarioPersonalizado(string numeroEmpleado)
    {
        using var db = new NominaContext();
        return db.HorariosEmpleado.AsNoTracking()
            .Where(h => h.NumeroEmpleado == numeroEmpleado)
            .ToList();
    }

    public string Guardar(Empleado empleado, IReadOnlyList<HorarioEmpleado>? horariosPersonalizados)
    {
        if (string.IsNullOrWhiteSpace(empleado.NumeroEmpleado))
            return "Ingrese el número de empleado.";
        if (string.IsNullOrWhiteSpace(empleado.Nombre))
            return "Ingrese el nombre.";
        if (string.IsNullOrWhiteSpace(empleado.Puesto))
            return "Ingrese el puesto.";
        if (empleado.SalarioHora < 0)
            return "El salario por hora no puede ser negativo.";

        using var db = new NominaContext();

        var numero = empleado.NumeroEmpleado.Trim();
        bool existe = db.Empleados.Any(e => e.NumeroEmpleado == numero && e.Id != empleado.Id);
        if (existe)
            return "Ese número de empleado ya existe.";

        Empleado destino;
        if (empleado.Id == 0)
        {
            destino = new Empleado();
            db.Empleados.Add(destino);
        }
        else
        {
            destino = db.Empleados.Find(empleado.Id)
                ?? throw new InvalidOperationException("No se encontró el empleado a editar.");
        }

        destino.NumeroEmpleado = numero;
        destino.Nombre = empleado.Nombre.Trim();
        destino.Puesto = empleado.Puesto.Trim();
        destino.Departamento = empleado.Departamento.Trim();
        destino.SalarioHora = empleado.SalarioHora;
        destino.TipoHorario = empleado.TipoHorario;
        destino.Turno = empleado.TipoHorario switch
        {
            TipoHorario.Mañana => "Mañana",
            TipoHorario.Tarde => "Tarde",
            TipoHorario.Personalizado => "Personalizado",
            TipoHorario.Especial => "Especial",
            _ => "Mañana"
        };
        destino.Descanso = empleado.Descanso;
        destino.Activo = empleado.Activo;

        bool tieneHorarioGuardado = db.HorariosEmpleado.Any(h => h.NumeroEmpleado == numero);
        if (destino.TipoHorario == TipoHorario.Personalizado &&
            (horariosPersonalizados is null || horariosPersonalizados.Count == 0) &&
            !tieneHorarioGuardado)
        {
            return "Debe configurar un horario personalizado antes de guardar el empleado.";
        }

        db.SaveChanges();

        var anteriores = db.HorariosEmpleado.Where(h => h.NumeroEmpleado == numero).ToList();
        if (destino.TipoHorario == TipoHorario.Personalizado)
        {
            if (horariosPersonalizados is { Count: > 0 })
            {
                db.HorariosEmpleado.RemoveRange(anteriores);
                foreach (var horario in horariosPersonalizados)
                {
                    db.HorariosEmpleado.Add(new HorarioEmpleado
                    {
                        NumeroEmpleado = numero,
                        DiaSemana = horario.DiaSemana,
                        HoraEntrada = horario.HoraEntrada,
                        HoraSalida = horario.HoraSalida
                    });
                }
            }
        }
        else if (anteriores.Count > 0)
        {
            db.HorariosEmpleado.RemoveRange(anteriores);
        }

        db.SaveChanges();
        return "OK";
    }

    public void AlternarActivo(int id)
    {
        using var db = new NominaContext();
        var empleado = db.Empleados.Find(id)
            ?? throw new InvalidOperationException("Empleado no encontrado.");
        empleado.Activo = !empleado.Activo;
        db.SaveChanges();
    }

    public void Eliminar(int id)
    {
        using var db = new NominaContext();
        var empleado = db.Empleados.Find(id)
            ?? throw new InvalidOperationException("Empleado no encontrado.");

        var horarios = db.HorariosEmpleado.Where(h => h.NumeroEmpleado == empleado.NumeroEmpleado).ToList();
        if (horarios.Count > 0)
            db.HorariosEmpleado.RemoveRange(horarios);

        db.Empleados.Remove(empleado);
        db.SaveChanges();
    }
}
