using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NominaPro.Datos;
using NominaPro.Modelos;

namespace NominaPro.Servicios
{
    public class HorarioManualService
    {
        public string GuardarCambios(List<HorarioManualItem> lista)
        {
            int modificados = 0;

            MotorNomina motor = new MotorNomina();

            using (var db = new NominaContext())
            {
                var configuracion = db.ConfiguracionSistema.FirstOrDefault();

                if (configuracion == null)
                    return "Primero configure el sistema.";

                foreach (var item in lista)
                {
                    var checada = db.Checadas.FirstOrDefault(x => x.Id == item.Id);

                    if (checada == null)
                        continue;

                    var empleado = db.Empleados.FirstOrDefault(x =>
                        x.NumeroEmpleado == checada.NumeroEmpleado);

                    if (empleado == null)
                        continue;

                    TimeSpan? entrada = null;
                    TimeSpan? salida = null;

                    // Validar entrada
                    if (!string.IsNullOrWhiteSpace(item.Entrada))
                    {
                        if (!TimeSpan.TryParse(item.Entrada, out TimeSpan horaEntrada))
                        {
                            return $"La hora de entrada '{item.Entrada}' del día {item.Fecha:dd/MM/yyyy} no es válida.\n\nUtilice el formato HH:mm\n\nEjemplo: 07:00";
                        }

                        entrada = horaEntrada;
                    }

                    // Validar salida
                    if (!string.IsNullOrWhiteSpace(item.Salida))
                    {
                        if (!TimeSpan.TryParse(item.Salida, out TimeSpan horaSalida))
                        {
                            return $"La hora de salida '{item.Salida}' del día {item.Fecha:dd/MM/yyyy} no es válida.\n\nUtilice el formato HH:mm\n\nEjemplo: 22:00";
                        }

                        salida = horaSalida;
                    }

                    // Validación lógica
                    if (entrada.HasValue && salida.HasValue && salida <= entrada)
                    {
                        return $"La salida debe ser mayor que la entrada.\n\nFecha: {item.Fecha:dd/MM/yyyy}";
                    }

                    var horario = motor.ObtenerHorarioEmpleado(
        empleado,
        checada.Fecha,
        configuracion,
        db);

                    checada.PrimeraChecada = entrada;
                    checada.UltimaChecada = salida;

                    checada.HorasTrabajadas =
                        motor.CalcularHorasTrabajadas(entrada, salida);

                    if (horario.AplicaRetardo)
                    {
                        checada.RetardoMinutos =
                            motor.CalcularRetardo(
                                entrada,
                                horario.Entrada,
                                horario.Tolerancia);

                        checada.TieneRetardo =
                            motor.TieneRetardo(checada.RetardoMinutos);
                    }
                    else
                    {
                        checada.RetardoMinutos = 0;
                        checada.TieneRetardo = false;
                    }

                    checada.ModificadaManual = true;

                    modificados++;
                }

                db.SaveChanges();
            }

            return $"Se actualizaron correctamente {modificados} registros.";
        }
    }
}
