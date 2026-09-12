using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NominaPro.Datos;
using NominaPro.Modelos;

namespace NominaPro.Servicios
{
    public class NominaService
    {
        public List<NominaEmpleado> CalcularNomina(DateTime fecha, decimal propinasSemana)
        {
            List<NominaEmpleado> lista = new();

            using (var db = NominaDatabase.CreateContext())
            {
                var configuracion = db.ConfiguracionSistema.FirstOrDefault();

                if (configuracion == null)
                    return lista;

                MotorNomina motor = new MotorNomina();

                int diferencia = fecha.DayOfWeek == DayOfWeek.Sunday
                    ? 6
                    : (int)fecha.DayOfWeek - 1;

                DateTime inicioSemana = fecha.Date.AddDays(-diferencia);

                DateTime finSemana = inicioSemana.AddDays(6);

                var empleados = db.Empleados
                    .Where(e => e.Activo)
                    .OrderBy(e => e.Nombre)
                    .ToList();

                var checadas = db.Checadas
                    .Where(c => c.Fecha >= inicioSemana &&
                                c.Fecha <= finSemana)
                    .ToList();

                decimal totalHorasEmpresa = checadas.Sum(c => c.HorasTrabajadas);

                decimal propinaPorHora = 0;

                if (totalHorasEmpresa > 0)
                    propinaPorHora = propinasSemana / totalHorasEmpresa;

                foreach (var empleado in empleados)
                {
                    var semanaEmpleado = checadas
                        .Where(c => c.NumeroEmpleado == empleado.NumeroEmpleado)
                        .ToList();

                    decimal horasTrabajadas = semanaEmpleado.Sum(c => c.HorasTrabajadas);

                    int totalRetardos = semanaEmpleado.Count(c => c.TieneRetardo);

                    decimal sueldoBase = horasTrabajadas * empleado.SalarioHora;

                    //==========================
                    // DESCUENTO POR RETARDOS
                    //==========================

                    decimal horasDescuento = motor.CalcularDescuentoRetardos(
                        totalRetardos,
                        configuracion.CadaRetardos,
                        configuracion.DescuentoRetardosMinutos);

                    decimal descuentoRetardos =
                        Math.Round(horasDescuento * empleado.SalarioHora, 2);

                    //==========================
                    // PROPINAS
                    //==========================

                    decimal propinas =
                        Math.Round(horasTrabajadas * propinaPorHora, 2);

                    //==========================
                    // PRIMA DOMINICAL
                    //==========================

                    decimal primaDominical = 0;

                    if (configuracion.AplicarPrimaDominical)
                    {
                        bool trabajoDomingo = semanaEmpleado.Any(c =>
                            c.Fecha.DayOfWeek == DayOfWeek.Sunday &&
                            c.PrimeraChecada.HasValue);

                        if (trabajoDomingo)
                        {
                            decimal sueldoDomingo = 8m * empleado.SalarioHora;

                            primaDominical = Math.Round(
                                sueldoDomingo *
                                configuracion.PorcentajePrimaDominical / 100m,
                                2);
                        }
                    }

                    decimal total =
                        sueldoBase
                        + propinas
                        + primaDominical
                        - descuentoRetardos;

                    lista.Add(new NominaEmpleado
                    {
                        NumeroEmpleado = empleado.NumeroEmpleado,

                        Nombre = empleado.Nombre,

                        HorasTrabajadas = horasTrabajadas,

                        SalarioHora = empleado.SalarioHora,

                        SueldoBase = sueldoBase,

                        TotalRetardos = totalRetardos,

                        DescuentoRetardos = descuentoRetardos,

                        Propinas = propinas,

                        PrimaDominical = primaDominical,

                        TotalPagar = total
                    });
                }
            }

            return lista;
        }
    }
}