using Microsoft.EntityFrameworkCore;
using NamiSoft.Datos;
using NamiSoft.Enumeraciones;
using NamiSoft.Modelos;

namespace NamiSoft.Servicios;

public sealed class EmpleadoService
{
    public List<Empleado> Obtener(
        string filtroEstado,
        string busqueda,
        string? filtroDepartamento = null,
        string? filtroTurno = null,
        string orden = "Número")
    {
        using var db = NominaDatabase.CreateContext();
        var query = db.Empleados.AsNoTracking().AsQueryable();

        query = filtroEstado switch
        {
            "Activos" => query.Where(e => e.Activo),
            "Inactivos" => query.Where(e => !e.Activo),
            _ => query
        };

        if (!string.IsNullOrWhiteSpace(filtroDepartamento) && filtroDepartamento != "Todos")
        {
            query = query.Where(e => e.Departamento == filtroDepartamento);
        }

        if (!string.IsNullOrWhiteSpace(filtroTurno) && filtroTurno != "Todos"
            && Enum.TryParse<TipoHorario>(filtroTurno, out var tipoHorario))
        {
            query = query.Where(e => e.TipoHorario == tipoHorario);
        }

        if (!string.IsNullOrWhiteSpace(busqueda))
        {
            var texto = busqueda.Trim().ToLower();
            query = query.Where(e =>
                e.Nombre.ToLower().Contains(texto) ||
                e.NumeroEmpleado.ToLower().Contains(texto) ||
                e.Puesto.ToLower().Contains(texto) ||
                e.Departamento.ToLower().Contains(texto));
        }

        query = orden switch
        {
            "Nombre (A-Z)" => query.OrderBy(e => e.Nombre),
            "Salario (mayor a menor)" => query.OrderByDescending(e => e.SalarioHora).ThenBy(e => e.Nombre),
            "Salario (menor a mayor)" => query.OrderBy(e => e.SalarioHora).ThenBy(e => e.Nombre),
            "Departamento" => query.OrderBy(e => e.Departamento).ThenBy(e => e.Nombre),
            _ => query.OrderBy(e => e.NumeroEmpleado)
        };

        return query.ToList();
    }

    public List<string> ObtenerDepartamentos()
    {
        using var db = NominaDatabase.CreateContext();
        return db.Empleados.AsNoTracking()
            .Select(e => e.Departamento)
            .Where(d => d != "")
            .Distinct()
            .OrderBy(d => d)
            .ToList();
    }

    public string GenerarDatosPrueba()
    {
        using var db = NominaDatabase.CreateContext();

        var muestra = new List<Empleado>
        {
            new() { NumeroEmpleado = "101", Nombre = "Ana Sofía Mendoza", Puesto = "Gerente de Turno", Departamento = "Administración", SalarioHora = 165m, TipoHorario = TipoHorario.Mañana, Turno = "Mañana", Descanso = "Domingo", Activo = true },
            new() { NumeroEmpleado = "102", Nombre = "Carlos Eduardo Ruiz", Puesto = "Chef Ejecutivo", Departamento = "Cocina", SalarioHora = 150m, TipoHorario = TipoHorario.Mañana, Turno = "Mañana", Descanso = "Lunes", Activo = true },
            new() { NumeroEmpleado = "103", Nombre = "Valeria Torres Vega", Puesto = "Cajera Principal", Departamento = "Caja", SalarioHora = 85m, TipoHorario = TipoHorario.Mañana, Turno = "Mañana", Descanso = "Martes", Activo = true },
            new() { NumeroEmpleado = "104", Nombre = "Diego Armando López", Puesto = "Mesero", Departamento = "Piso", SalarioHora = 65m, TipoHorario = TipoHorario.Tarde, Turno = "Tarde", Descanso = "Miércoles", Activo = true },
            new() { NumeroEmpleado = "105", Nombre = "Fernanda Castillo", Puesto = "Barista", Departamento = "Barra", SalarioHora = 78m, TipoHorario = TipoHorario.Mañana, Turno = "Mañana", Descanso = "Jueves", Activo = true },
            new() { NumeroEmpleado = "106", Nombre = "Jorge Luis Ramírez", Puesto = "Cocinero A", Departamento = "Cocina", SalarioHora = 95m, TipoHorario = TipoHorario.Tarde, Turno = "Tarde", Descanso = "Martes", Activo = true },
            new() { NumeroEmpleado = "107", Nombre = "Mariana Paredes", Puesto = "Hostess", Departamento = "Piso", SalarioHora = 72m, TipoHorario = TipoHorario.Tarde, Turno = "Tarde", Descanso = "Lunes", Activo = true },
            new() { NumeroEmpleado = "108", Nombre = "Ricardo Morales", Puesto = "Bartender", Departamento = "Barra", SalarioHora = 90m, TipoHorario = TipoHorario.Especial, Turno = "Especial", Descanso = "Miércoles", Activo = true },
            new() { NumeroEmpleado = "109", Nombre = "Lucía Hernández", Puesto = "Auxiliar de Limpieza", Departamento = "Limpieza", SalarioHora = 58m, TipoHorario = TipoHorario.Mañana, Turno = "Mañana", Descanso = "Viernes", Activo = true },
            new() { NumeroEmpleado = "110", Nombre = "Héctor Navarro (Baja)", Puesto = "Lavaloza", Departamento = "Cocina", SalarioHora = 55m, TipoHorario = TipoHorario.Tarde, Turno = "Tarde", Descanso = "Jueves", Activo = false }
        };

        var existentes = db.Empleados.Select(e => e.NumeroEmpleado).ToHashSet();
        var agregados = 0;
        foreach (var emp in muestra)
        {
            if (existentes.Contains(emp.NumeroEmpleado))
                continue;
            db.Empleados.Add(emp);
            agregados++;
        }

        db.SaveChanges();

        var hoy = DateTime.Today;
        var checadasAgregadas = 0;
        var empleadosActivos = db.Empleados.AsNoTracking()
            .Where(e => e.Activo && muestra.Select(m => m.NumeroEmpleado).Contains(e.NumeroEmpleado))
            .ToList();

        for (int diasAtras = 4; diasAtras >= 1; diasAtras--)
        {
            var fecha = hoy.AddDays(-diasAtras);
            foreach (var emp in empleadosActivos)
            {
                bool yaExiste = db.Checadas.Any(c => c.NumeroEmpleado == emp.NumeroEmpleado && c.Fecha == fecha);
                if (yaExiste)
                    continue;

                var entradaBase = emp.TipoHorario == TipoHorario.Tarde
                    ? new TimeSpan(14, 0, 0)
                    : new TimeSpan(8, 0, 0);

                // Simular algunos retardos y una salida faltante para probar Incidencias
                int retardoMin = (emp.NumeroEmpleado == "104" && diasAtras == 2) ? 18
                    : (emp.NumeroEmpleado == "107" && diasAtras == 1) ? 12
                    : 0;

                var entrada = entradaBase.Add(TimeSpan.FromMinutes(retardoMin));
                TimeSpan? salida = (emp.NumeroEmpleado == "106" && diasAtras == 1)
                    ? null
                    : entradaBase.Add(TimeSpan.FromHours(8));

                decimal horas = salida.HasValue
                    ? Math.Round((decimal)(salida.Value - entrada).TotalHours, 2)
                    : 0m;

                db.Checadas.Add(new ChecadaEmpleado
                {
                    NumeroEmpleado = emp.NumeroEmpleado,
                    Nombre = emp.Nombre,
                    Fecha = fecha,
                    PrimeraChecada = entrada,
                    UltimaChecada = salida,
                    HorasTrabajadas = horas,
                    RetardoMinutos = retardoMin,
                    TieneRetardo = retardoMin > 0,
                    Origen = OrigenChecada.Sistema,
                    CreadaEn = DateTime.Now,
                    Motivo = "Datos de prueba"
                });
                checadasAgregadas++;
            }
        }

        db.SaveChanges();
        return $"Se agregaron {agregados} empleados y {checadasAgregadas} checadas de prueba.";
    }

    public Empleado? ObtenerPorId(int id)
    {
        using var db = NominaDatabase.CreateContext();
        return db.Empleados.AsNoTracking().FirstOrDefault(e => e.Id == id);
    }

    public List<Empleado> ObtenerTodos()
    {
        using var db = NominaDatabase.CreateContext();
        return db.Empleados.AsNoTracking().OrderBy(e => e.NumeroEmpleado).ToList();
    }

    public List<HorarioEmpleado> ObtenerHorarioPersonalizado(string numeroEmpleado)
    {
        using var db = NominaDatabase.CreateContext();
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

        using var db = NominaDatabase.CreateContext();

        var numero = empleado.NumeroEmpleado.Trim();
        bool existe = db.Empleados.Any(e => e.NumeroEmpleado == numero && e.Id != empleado.Id);
        if (existe)
            return "Ese número de empleado ya existe.";

        Empleado? destino;
        if (empleado.Id == 0)
        {
            destino = new Empleado();
            db.Empleados.Add(destino);
        }
        else
        {
            destino = db.Empleados.Find(empleado.Id);
            if (destino is null)
                return "No se encontró el empleado a editar. Recargue la lista e intente de nuevo.";
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

        using var transaction = db.Database.BeginTransaction();

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
        transaction.Commit();
        return "OK";
    }

    public bool AlternarActivo(int id)
    {
        using var db = NominaDatabase.CreateContext();
        var empleado = db.Empleados.Find(id);
        if (empleado is null)
            return false;
        empleado.Activo = !empleado.Activo;
        db.SaveChanges();
        return true;
    }

    public bool Eliminar(int id)
    {
        using var db = NominaDatabase.CreateContext();
        var empleado = db.Empleados.Find(id);
        if (empleado is null)
            return false;

        var horarios = db.HorariosEmpleado.Where(h => h.NumeroEmpleado == empleado.NumeroEmpleado).ToList();
        if (horarios.Count > 0)
            db.HorariosEmpleado.RemoveRange(horarios);

        db.Empleados.Remove(empleado);
        db.SaveChanges();
        return true;
    }
}
