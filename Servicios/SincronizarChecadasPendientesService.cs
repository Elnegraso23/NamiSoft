using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using NominaPro.Datos;
using NominaPro.Modelos;

namespace NominaPro.Servicios
{
    public class SincronizarChecadasPendientesService
    {
        public string Sincronizar()
        {
            int sincronizadas = 0;

            using (var db = new NominaContext())
            {
                var configuracion = db.ConfiguracionSistema.FirstOrDefault();

                if (configuracion == null)
                    return "Primero configure el sistema.";

                MotorNomina motor = new MotorNomina();

                var pendientes = db.ChecadasPendientes.ToList();

                foreach (var pendiente in pendientes)
                {
                    var empleado = db.Empleados.FirstOrDefault(x =>
                        x.NumeroEmpleado == pendiente.NumeroEmpleado);

                    if (empleado == null)
                        continue;

                    bool existe = db.Checadas.Any(x =>
                        x.NumeroEmpleado == pendiente.NumeroEmpleado &&
                        x.Fecha.Date == pendiente.Fecha.Date);

                    if (existe)
                    {
                        db.ChecadasPendientes.Remove(pendiente);
                        continue;
                    }

                    var horario = motor.ObtenerHorarioEmpleado(empleado,pendiente.Fecha, configuracion, db);


                    decimal horas = motor.CalcularHorasTrabajadas(
                        pendiente.PrimeraChecada,
                        pendiente.UltimaChecada);

                    int retardo = 0;

                    if (horario.AplicaRetardo)
                    {
                        retardo = motor.CalcularRetardo(
                            pendiente.PrimeraChecada,
                            horario.Entrada,
                            horario.Tolerancia);
                    }

                    ChecadaEmpleado nueva = new ChecadaEmpleado
                    {
                        NumeroEmpleado = pendiente.NumeroEmpleado,
                        Nombre = pendiente.Nombre,
                        Fecha = pendiente.Fecha,

                        PrimeraChecada = pendiente.PrimeraChecada,
                        UltimaChecada = pendiente.UltimaChecada,

                        HorasTrabajadas = horas,

                        RetardoMinutos = retardo,

                        TieneRetardo = motor.TieneRetardo(retardo),

                        HorasExtra = 0,

                        ModificadaManual = false
                    };

                    db.Checadas.Add(nueva);

                    db.ChecadasPendientes.Remove(pendiente);

                    sincronizadas++;
                }

                db.SaveChanges();
            }

            return $"Se sincronizaron {sincronizadas} checadas pendientes.";
        }
    }
}
