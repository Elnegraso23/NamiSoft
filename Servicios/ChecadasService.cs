using System;
using System.Collections.Generic;
using System.Linq;
using NominaPro.Datos;
using NominaPro.Modelos;

namespace NominaPro.Servicios
{
    public class ChecadasService
    {
        public string GuardarChecadas(List<ChecadaImportada> lista)
        {
            int importadas = 0;
            int duplicadas = 0;
            int empleadosNoEncontrados = 0;

            MotorNomina motor = new MotorNomina();

            using (var db = new NominaContext())
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

                        ModificadaManual = false
                    };

                    db.Checadas.Add(nueva);

                    importadas++;
                }

                db.SaveChanges();
            }

            return $"Importadas: {importadas}\nDuplicadas: {duplicadas}\nNo encontrados: {empleadosNoEncontrados}";
        }
    }
}