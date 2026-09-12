using NamiSoft.Datos;
using NamiSoft.Enumeraciones;
using NamiSoft.Modelos;

namespace NamiSoft.Servicios;

public sealed record EstadoDia(ChecadaEmpleado? Checada)
{
    public bool TieneEntrada => Checada?.PrimeraChecada.HasValue == true;
    public bool TieneSalida => Checada?.UltimaChecada.HasValue == true;
    public bool Completo => TieneEntrada && TieneSalida;
}

public sealed record ResultadoRegistro(bool Ok, string Mensaje)
{
    public static ResultadoRegistro Exito(string mensaje) => new(true, mensaje);
    public static ResultadoRegistro Fallo(string mensaje) => new(false, mensaje);
}

public sealed record IncidenciaAsistencia(
    string NumeroEmpleado, string Nombre, DateTime Fecha, string Tipo, string Detalle);

public sealed record ResumenPrevioImportacion(int Validos, int Duplicados, int NoEncontrados);

/// <summary>
/// Punto único de escritura de checadas: registro interno, captura manual,
/// edición e importación terminan en la misma tabla con el mismo cálculo.
/// El motor de nómina solo lee checadas normalizadas.
/// </summary>
public class ChecadasService
{
    public string GuardarChecadas(List<ChecadaImportada> lista)
    {
        int importadas = 0;
        int duplicadas = 0;
        int empleadosNoEncontrados = 0;

        MotorNomina motor = new MotorNomina();

        using (var db = NominaDatabase.CreateContext())
        {
            var configuracion = db.ConfiguracionSistema.FirstOrDefault();

            if (configuracion == null)
            {
                return "Primero debe configurar los cálculos del sistema.";
            }

            foreach (var item in lista)
            {
                var empleado = db.Empleados.FirstOrDefault(x =>
                    x.NumeroEmpleado == item.NumeroEmpleado);

                // EMPLEADO NO REGISTRADO
                if (empleado == null)
                {
                    bool pendienteExiste = db.ChecadasPendientes.Any(x =>
                        x.NumeroEmpleado == item.NumeroEmpleado &&
                        x.Fecha.Date == item.Fecha.Date);

                    if (!pendienteExiste)
                    {
                        ChecadaPendiente pendiente = new ChecadaPendiente
                        {
                            NumeroEmpleado = item.NumeroEmpleado,
                            Nombre = item.Nombre,
                            Fecha = item.Fecha,
                            PrimeraChecada = item.PrimeraChecada,
                            UltimaChecada = item.UltimaChecada,
                            Motivo = "Empleado no registrado"
                        };

                        db.ChecadasPendientes.Add(pendiente);
                    }

                    empleadosNoEncontrados++;
                    continue;
                }

                // YA EXISTE LA CHECADA
                bool existe = db.Checadas.Any(x =>
                    x.NumeroEmpleado == item.NumeroEmpleado &&
                    x.Fecha.Date == item.Fecha.Date);

                if (existe)
                {
                    duplicadas++;
                    continue;
                }

                var horario = motor.ObtenerHorarioEmpleado(
                    empleado,
                    item.Fecha,
                    configuracion,
                    db);

                decimal horasTrabajadas = motor.CalcularHorasTrabajadas(
                    item.PrimeraChecada,
                    item.UltimaChecada);

                int retardo = 0;

                if (horario.AplicaRetardo)
                {
                    retardo = motor.CalcularRetardo(
                        item.PrimeraChecada,
                        horario.Entrada,
                        horario.Tolerancia);
                }

                ChecadaEmpleado nueva = new ChecadaEmpleado
                {
                    NumeroEmpleado = item.NumeroEmpleado,
                    Nombre = item.Nombre,
                    Fecha = item.Fecha,

                    PrimeraChecada = item.PrimeraChecada,
                    UltimaChecada = item.UltimaChecada,

                    HorasTrabajadas = horasTrabajadas,

                    RetardoMinutos = retardo,

                    TieneRetardo = motor.TieneRetardo(retardo),

                    HorasExtra = 0,

                    ModificadaManual = false,

                    Origen = OrigenChecada.ImportacionExcel,
                    CreadaEn = DateTime.Now
                };

                db.Checadas.Add(nueva);

                importadas++;
            }

            db.SaveChanges();
        }

        return $"Importadas: {importadas}\nDuplicadas: {duplicadas}\nNo encontrados: {empleadosNoEncontrados}";
    }

    public ResumenPrevioImportacion? ResumenPrevio(List<ChecadaImportada> lista)
    {
        using var db = NominaDatabase.CreateContext();
        if (db.ConfiguracionSistema.FirstOrDefault() is null)
            return null;

        int validos = 0, duplicados = 0, noEncontrados = 0;
        foreach (var item in lista)
        {
            bool empleadoExiste = db.Empleados.Any(x => x.NumeroEmpleado == item.NumeroEmpleado);
            if (!empleadoExiste)
            {
                noEncontrados++;
                continue;
            }

            bool existe = db.Checadas.Any(x =>
                x.NumeroEmpleado == item.NumeroEmpleado && x.Fecha.Date == item.Fecha.Date);
            if (existe)
                duplicados++;
            else
                validos++;
        }

        return new ResumenPrevioImportacion(validos, duplicados, noEncontrados);
    }

    public List<ChecadaEmpleado> ObtenerDelDia(DateTime fecha)
    {
        using var db = NominaDatabase.CreateContext();
        return db.Checadas
            .Where(c => c.Fecha.Date == fecha.Date)
            .OrderBy(c => c.Nombre)
            .ToList();
    }

    public EstadoDia ObtenerEstadoDia(string numeroEmpleado, DateTime fecha)
    {
        using var db = NominaDatabase.CreateContext();
        var checada = db.Checadas.FirstOrDefault(c =>
            c.NumeroEmpleado == numeroEmpleado && c.Fecha.Date == fecha.Date);
        return new EstadoDia(checada);
    }

    public ResultadoRegistro RegistrarEntrada(string numeroEmpleado, DateTime momento, OrigenChecada origen)
    {
        using var db = NominaDatabase.CreateContext();
        var empleado = db.Empleados.FirstOrDefault(e => e.NumeroEmpleado == numeroEmpleado);
        if (empleado is null)
            return ResultadoRegistro.Fallo("Empleado no encontrado.");
        if (!empleado.Activo)
            return ResultadoRegistro.Fallo($"{empleado.Nombre} está inactivo.");

        var configuracion = db.ConfiguracionSistema.FirstOrDefault();
        if (configuracion is null)
            return ResultadoRegistro.Fallo("Primero debe configurar los cálculos del sistema.");

        var fecha = momento.Date;
        var hora = momento.TimeOfDay;
        var motor = new MotorNomina();

        var checada = db.Checadas.FirstOrDefault(c =>
            c.NumeroEmpleado == numeroEmpleado && c.Fecha.Date == fecha);

        if (checada is not null)
        {
            if (checada.PrimeraChecada.HasValue)
                return ResultadoRegistro.Fallo(
                    $"Ya tiene entrada registrada a las {FormatoHora(checada.PrimeraChecada.Value)}.");

            checada.PrimeraChecada = hora;
            Recalcular(checada, empleado, configuracion, motor, db);
            db.SaveChanges();
            return ResultadoRegistro.Exito(
                $"Entrada de {empleado.Nombre} registrada a las {FormatoHora(hora)}.");
        }

        checada = new ChecadaEmpleado
        {
            NumeroEmpleado = numeroEmpleado,
            Nombre = empleado.Nombre,
            Fecha = fecha,
            PrimeraChecada = hora,
            Origen = origen,
            CreadaEn = DateTime.Now
        };
        Recalcular(checada, empleado, configuracion, motor, db);
        db.Checadas.Add(checada);
        db.SaveChanges();
        return ResultadoRegistro.Exito(
            $"Entrada de {empleado.Nombre} registrada a las {FormatoHora(hora)}.");
    }

    public ResultadoRegistro RegistrarSalida(string numeroEmpleado, DateTime momento, OrigenChecada origen)
    {
        using var db = NominaDatabase.CreateContext();
        var empleado = db.Empleados.FirstOrDefault(e => e.NumeroEmpleado == numeroEmpleado);
        if (empleado is null)
            return ResultadoRegistro.Fallo("Empleado no encontrado.");
        if (!empleado.Activo)
            return ResultadoRegistro.Fallo($"{empleado.Nombre} está inactivo.");

        var configuracion = db.ConfiguracionSistema.FirstOrDefault();
        if (configuracion is null)
            return ResultadoRegistro.Fallo("Primero debe configurar los cálculos del sistema.");

        var fecha = momento.Date;
        var hora = momento.TimeOfDay;

        var checada = db.Checadas.FirstOrDefault(c =>
            c.NumeroEmpleado == numeroEmpleado && c.Fecha.Date == fecha);

        if (checada is null || !checada.PrimeraChecada.HasValue)
            return ResultadoRegistro.Fallo(
                $"{empleado.Nombre} no tiene entrada registrada hoy. Registre la entrada primero.");

        if (checada.UltimaChecada.HasValue)
            return ResultadoRegistro.Fallo(
                $"Ya tiene salida registrada a las {FormatoHora(checada.UltimaChecada.Value)}.");

        if (hora <= checada.PrimeraChecada.Value)
            return ResultadoRegistro.Fallo(
                $"La salida debe ser mayor que la entrada ({FormatoHora(checada.PrimeraChecada.Value)}).");

        checada.UltimaChecada = hora;
        if (checada.Origen == OrigenChecada.Sistema && origen != OrigenChecada.Sistema)
            checada.Origen = origen;
        Recalcular(checada, empleado, configuracion, new MotorNomina(), db);
        db.SaveChanges();
        return ResultadoRegistro.Exito(
            $"Salida de {empleado.Nombre} registrada a las {FormatoHora(hora)}. " +
            $"Horas: {checada.HorasTrabajadas:0.##}.");
    }

    public ResultadoRegistro AgregarManual(
        string numeroEmpleado, DateTime fecha,
        TimeSpan? entrada, TimeSpan? salida,
        string? motivo, OrigenChecada origen)
    {
        using var db = NominaDatabase.CreateContext();
        var empleado = db.Empleados.FirstOrDefault(e => e.NumeroEmpleado == numeroEmpleado);
        if (empleado is null)
            return ResultadoRegistro.Fallo("Empleado no encontrado.");
        if (!empleado.Activo)
            return ResultadoRegistro.Fallo($"{empleado.Nombre} está inactivo.");

        var configuracion = db.ConfiguracionSistema.FirstOrDefault();
        if (configuracion is null)
            return ResultadoRegistro.Fallo("Primero debe configurar los cálculos del sistema.");

        bool existe = db.Checadas.Any(c =>
            c.NumeroEmpleado == numeroEmpleado && c.Fecha.Date == fecha.Date);
        if (existe)
            return ResultadoRegistro.Fallo(
                "Ya existe una checada ese día. Edítela en lugar de crear otra.");

        if (!entrada.HasValue && !salida.HasValue)
            return ResultadoRegistro.Fallo("Capture al menos la entrada o la salida.");
        if (entrada.HasValue && salida.HasValue && salida <= entrada)
            return ResultadoRegistro.Fallo("La salida debe ser mayor que la entrada.");

        var checada = new ChecadaEmpleado
        {
            NumeroEmpleado = numeroEmpleado,
            Nombre = empleado.Nombre,
            Fecha = fecha.Date,
            PrimeraChecada = entrada,
            UltimaChecada = salida,
            ModificadaManual = true,
            Origen = origen,
            CreadaEn = DateTime.Now,
            Motivo = string.IsNullOrWhiteSpace(motivo) ? null : motivo.Trim()
        };
        Recalcular(checada, empleado, configuracion, new MotorNomina(), db);
        db.Checadas.Add(checada);
        db.SaveChanges();

        var mensaje = $"Checada manual guardada para {empleado.Nombre}.";
        if (checada.TieneRetardo)
            mensaje += $" Con retardo de {checada.RetardoMinutos} min.";
        return ResultadoRegistro.Exito(mensaje);
    }

    public string EditarChecada(int id, TimeSpan? entrada, TimeSpan? salida, string? motivo)
    {
        using var db = NominaDatabase.CreateContext();
        var checada = db.Checadas.FirstOrDefault(x => x.Id == id);
        if (checada is null)
            return "No se encontró la checada a editar.";

        var empleado = db.Empleados.FirstOrDefault(x => x.NumeroEmpleado == checada.NumeroEmpleado);
        if (empleado is null)
            return "El empleado de esta checada ya no existe.";

        var configuracion = db.ConfiguracionSistema.FirstOrDefault();
        if (configuracion is null)
            return "Primero configure el sistema.";

        if (entrada.HasValue && salida.HasValue && salida <= entrada)
            return $"La salida debe ser mayor que la entrada.\n\nFecha: {checada.Fecha:dd/MM/yyyy}";

        checada.PrimeraChecada = entrada;
        checada.UltimaChecada = salida;
        Recalcular(checada, empleado, configuracion, new MotorNomina(), db);
        checada.ModificadaManual = true;
        checada.ModificadaEn = DateTime.Now;
        if (!string.IsNullOrWhiteSpace(motivo))
            checada.Motivo = motivo.Trim();
        db.SaveChanges();
        return "OK";
    }

    public List<IncidenciaAsistencia> ObtenerIncidencias(DateTime inicio, DateTime fin)
    {
        using var db = NominaDatabase.CreateContext();
        var lista = new List<IncidenciaAsistencia>();
        var checadas = db.Checadas
            .Where(c => c.Fecha.Date >= inicio.Date && c.Fecha.Date <= fin.Date)
            .OrderBy(c => c.Fecha).ThenBy(c => c.Nombre)
            .ToList();

        foreach (var c in checadas)
        {
            if (!c.PrimeraChecada.HasValue)
                lista.Add(new IncidenciaAsistencia(c.NumeroEmpleado, c.Nombre, c.Fecha,
                    "Falta entrada", "Hay registro del día sin hora de entrada."));
            else if (!c.UltimaChecada.HasValue)
                lista.Add(new IncidenciaAsistencia(c.NumeroEmpleado, c.Nombre, c.Fecha,
                    "Falta salida", $"Entró a las {FormatoHora(c.PrimeraChecada.Value)} y no registró salida."));

            if (c.ModificadaManual)
                lista.Add(new IncidenciaAsistencia(c.NumeroEmpleado, c.Nombre, c.Fecha,
                    "Modificada", string.IsNullOrWhiteSpace(c.Motivo) ? "Editada manualmente." : c.Motivo));
        }

        return lista;
    }

    public int ContarIncompletas(DateTime inicio, DateTime fin)
    {
        using var db = NominaDatabase.CreateContext();
        return db.Checadas.Count(c =>
            c.Fecha.Date >= inicio.Date && c.Fecha.Date <= fin.Date &&
            (!c.PrimeraChecada.HasValue || !c.UltimaChecada.HasValue));
    }

    public List<ChecadaEmpleado> ObtenerRango(string? numeroEmpleado, DateTime inicio, DateTime fin)
    {
        using var db = NominaDatabase.CreateContext();
        var query = db.Checadas
            .Where(c => c.Fecha.Date >= inicio.Date && c.Fecha.Date <= fin.Date);
        if (!string.IsNullOrWhiteSpace(numeroEmpleado))
            query = query.Where(c => c.NumeroEmpleado == numeroEmpleado);
        return query.OrderByDescending(c => c.Fecha).ThenBy(c => c.Nombre).ToList();
    }

    public bool EliminarChecada(int id)
    {
        using var db = NominaDatabase.CreateContext();
        var checada = db.Checadas.FirstOrDefault(c => c.Id == id);
        if (checada is null)
            return false;
        db.Checadas.Remove(checada);
        db.SaveChanges();
        return true;
    }

    public static (DateTime Inicio, DateTime Fin) SemanaDe(DateTime fecha)
    {
        int diferencia = fecha.DayOfWeek == DayOfWeek.Sunday ? 6 : (int)fecha.DayOfWeek - 1;
        var inicio = fecha.Date.AddDays(-diferencia);
        return (inicio, inicio.AddDays(6));
    }

    private static void Recalcular(
        ChecadaEmpleado checada, Empleado empleado,
        ConfiguracionSistema configuracion, MotorNomina motor, NominaContext db)
    {
        var horario = motor.ObtenerHorarioEmpleado(empleado, checada.Fecha, configuracion, db);
        checada.HorasTrabajadas = motor.CalcularHorasTrabajadas(checada.PrimeraChecada, checada.UltimaChecada);
        if (horario.AplicaRetardo)
        {
            checada.RetardoMinutos = motor.CalcularRetardo(checada.PrimeraChecada, horario.Entrada, horario.Tolerancia);
            checada.TieneRetardo = motor.TieneRetardo(checada.RetardoMinutos);
        }
        else
        {
            checada.RetardoMinutos = 0;
            checada.TieneRetardo = false;
        }
    }

    private static string FormatoHora(TimeSpan hora) => hora.ToString(@"hh\:mm");
}
