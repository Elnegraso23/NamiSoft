using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NominaPro.Modelos;
using NominaPro.Datos;
using NominaPro.Enumeraciones;

namespace NominaPro.Servicios
{
    internal class MotorNomina
    {

        public decimal CalcularHorasTrabajadas(TimeSpan? entrada, TimeSpan? salida)
        {
            if (!entrada.HasValue || !salida.HasValue)
                return 0;

            decimal horas = (decimal)(salida.Value - entrada.Value).TotalHours;

            return Math.Round(horas, 2);
        }

        public int CalcularRetardo(TimeSpan? entradaReal, TimeSpan horaEntradaTurno, int tolerancia)
        {
            if (!entradaReal.HasValue)
                return 0;

            TimeSpan horaLimite = horaEntradaTurno.Add(TimeSpan.FromMinutes(tolerancia));

            if (entradaReal.Value <= horaLimite)
                return 0;

            return (int)(entradaReal.Value - horaLimite).TotalMinutes;
        }

        public bool TieneRetardo(int minutosRetardo)
        {
            return minutosRetardo > 0;
        }

        public decimal CalcularDescuentoRetardos(int totalRetardos,int cadaRetardos,int descuentoMinutos)
        {
            if (cadaRetardos <= 0)
                return 0;

            int grupos = totalRetardos / cadaRetardos;

            return grupos * (descuentoMinutos / 60m);
        }

        public bool EsDomingo(DateTime fecha)
        {
            return fecha.DayOfWeek == DayOfWeek.Sunday;
        }

        public HorarioCalculado ObtenerHorarioEmpleado(
    Empleado empleado,
    DateTime fecha,
    ConfiguracionSistema config,
    NominaContext db)
        {
            HorarioCalculado horario = new HorarioCalculado();

            switch (empleado.TipoHorario)
            {
                case TipoHorario.Mañana:

                    horario.Entrada = config.EntradaManana;
                    horario.Salida = config.SalidaManana;
                    horario.Tolerancia = config.ToleranciaManana;
                    horario.AplicaRetardo = true;
                    break;

                case TipoHorario.Tarde:

                    horario.Entrada = config.EntradaTarde;
                    horario.Salida = config.SalidaTarde;
                    horario.Tolerancia = config.ToleranciaTarde;
                    horario.AplicaRetardo = true;
                    break;

                case TipoHorario.Personalizado:

                    string dia = fecha.DayOfWeek switch
                    {
                        DayOfWeek.Monday => "Lunes",
                        DayOfWeek.Tuesday => "Martes",
                        DayOfWeek.Wednesday => "Miércoles",
                        DayOfWeek.Thursday => "Jueves",
                        DayOfWeek.Friday => "Viernes",
                        DayOfWeek.Saturday => "Sábado",
                        DayOfWeek.Sunday => "Domingo",
                        _ => ""
                    };

                    var horarioDB = db.HorariosEmpleado.FirstOrDefault(x =>
                        x.NumeroEmpleado == empleado.NumeroEmpleado &&
                        x.DiaSemana == dia);

                    if (horarioDB != null)
                    {
                        horario.Entrada = horarioDB.HoraEntrada;
                        horario.Salida = horarioDB.HoraSalida;
                    }

                    horario.Tolerancia = 0;
                    horario.AplicaRetardo = true;

                    break;

                case TipoHorario.Especial:

                    horario.Entrada = TimeSpan.Zero;
                    horario.Salida = TimeSpan.Zero;
                    horario.Tolerancia = 0;
                    horario.AplicaRetardo = false;

                    break;
            }

            return horario;
        }
    }
}
