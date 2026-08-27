using ClosedXML.Excel;
using NominaPro.Modelos;
using System;
using System.Collections.Generic;

namespace NominaPro.Servicios
{
    /// Servicio encargado de generar y exportar la nómina semanal
    /// en formato Microsoft Excel (.xlsx).
    ///
    /// El archivo incluye:
    /// - Encabezado de la empresa.
    /// - Periodo de la nómina.
    /// - Tabla con empleados.
    /// - Totales por empleado.
    /// - Resumen general.
    /// - Formato profesional listo para impresión.
    /// </summary>
    public class ExcelNominaService
    {
        /// <summary>
        /// Exporta la nómina semanal a un archivo Excel.
        /// </summary>
        /// <param name="lista">
        /// Lista de empleados con la nómina ya calculada.
        /// </param>
        /// <param name="fechaSemana">
        /// Cualquier fecha perteneciente a la semana que se desea exportar.
        /// El sistema calcula automáticamente lunes y domingo.
        /// </param>
        /// <param name="propinasSemana">
        /// Total de propinas capturadas para la semana.
        /// (Actualmente solo se utiliza como dato de referencia.)
        /// </param>
        /// <param name="rutaArchivo">
        /// Ruta donde se guardará el archivo Excel.
        /// </param>
        public void ExportarNomina(
            List<NominaEmpleado> lista,
            DateTime fechaSemana,
            decimal propinasSemana,
            string rutaArchivo)
        {

            //====================================================
            // Crear libro y hoja de trabajo
            //====================================================

            using (var libro = new XLWorkbook())
            {
                var hoja = libro.Worksheets.Add("Nómina");

                //====================================================
                // Calcular rango de la semana (Lunes - Domingo)
                //====================================================

                int diferencia = fechaSemana.DayOfWeek == DayOfWeek.Sunday
                    ? 6
                    : (int)fechaSemana.DayOfWeek - 1;

                DateTime inicioSemana = fechaSemana.AddDays(-diferencia);

                DateTime finSemana = inicioSemana.AddDays(6);

                //====================================================
                // Encabezado principal
                //====================================================

                hoja.Cell("A1").Value = "CASA RED";

                hoja.Cell("A2").Value = "NÓMINA SEMANAL";

                hoja.Cell("A4").Value =
                    $"Semana: {inicioSemana:dd/MM/yyyy} al {finSemana:dd/MM/yyyy}";

                hoja.Cell("A5").Value =
                    $"Fecha de generación: {DateTime.Now:dd/MM/yyyy}";

                //====================================================
                // Encabezados de la tabla
                //====================================================

                int fila = 7;

                hoja.Cell(fila, 1).Value = "Empleado";
                hoja.Cell(fila, 2).Value = "Horas";
                hoja.Cell(fila, 3).Value = "$ Hora";
                hoja.Cell(fila, 4).Value = "Sueldo";
                hoja.Cell(fila, 5).Value = "Retardos";
                hoja.Cell(fila, 6).Value = "Descuento";
                hoja.Cell(fila, 7).Value = "Propinas";
                hoja.Cell(fila, 8).Value = "Prima Dom.";
                hoja.Cell(fila, 9).Value = "Total";

                //====================================================
                // Aplicar formato al encabezado
                //====================================================

                var encabezado = hoja.Range(fila, 1, fila, 9);

                encabezado.Style.Font.Bold = true;

                encabezado.Style.Font.FontColor = XLColor.White;

                encabezado.Style.Fill.BackgroundColor = XLColor.SteelBlue;

                encabezado.Style.Alignment.Horizontal =
                    XLAlignmentHorizontalValues.Center;

                encabezado.Style.Border.OutsideBorder =
                    XLBorderStyleValues.Thin;

                encabezado.Style.Border.InsideBorder =
                    XLBorderStyleValues.Thin;

                hoja.Row(7).Height = 25;

                hoja.Row(7).Style.Alignment.Vertical =
                    XLAlignmentVerticalValues.Center;

                fila++;

                //====================================================
                // Escribir información de cada empleado
                //====================================================

                foreach (var empleado in lista)
                {
                    hoja.Cell(fila, 1).Value = empleado.Nombre;

                    hoja.Cell(fila, 2).Value = empleado.HorasTrabajadas;

                    hoja.Cell(fila, 3).Value = empleado.SalarioHora;

                    hoja.Cell(fila, 4).Value = empleado.SueldoBase;

                    hoja.Cell(fila, 5).Value = empleado.TotalRetardos;

                    hoja.Cell(fila, 6).Value = empleado.DescuentoRetardos;

                    hoja.Cell(fila, 7).Value = empleado.Propinas;

                    hoja.Cell(fila, 8).Value = empleado.PrimaDominical;

                    hoja.Cell(fila, 9).Value = empleado.TotalPagar;

                    fila++;
                }

                //====================================================
                // Calcular resumen de la nómina
                //====================================================

                fila += 2;

                decimal totalHoras = lista.Sum(x => x.HorasTrabajadas);

                decimal totalSueldos = lista.Sum(x => x.SueldoBase);

                decimal totalPropinas = lista.Sum(x => x.Propinas);

                decimal totalPrima = lista.Sum(x => x.PrimaDominical);

                decimal totalDescuentos = lista.Sum(x => x.DescuentoRetardos);

                decimal totalNomina = lista.Sum(x => x.TotalPagar);

                //====================================================
                // Mostrar resumen
                //====================================================

                hoja.Cell(fila, 1).Value = "RESUMEN";
                hoja.Cell(fila, 1).Style.Font.Bold = true;
                hoja.Cell(fila, 1).Style.Font.FontSize = 14;

                fila++;

                hoja.Cell(fila, 1).Value = "Total empleados";
                hoja.Cell(fila, 2).Value = lista.Count;

                fila++;

                hoja.Cell(fila, 1).Value = "Horas trabajadas";
                hoja.Cell(fila, 2).Value = totalHoras;

                fila++;

                hoja.Cell(fila, 1).Value = "Sueldos";
                hoja.Cell(fila, 2).Value = totalSueldos;

                fila++;

                hoja.Cell(fila, 1).Value = "Propinas";
                hoja.Cell(fila, 2).Value = totalPropinas;

                fila++;

                hoja.Cell(fila, 1).Value = "Prima Dominical";
                hoja.Cell(fila, 2).Value = totalPrima;

                fila++;

                hoja.Cell(fila, 1).Value = "Descuentos";
                hoja.Cell(fila, 2).Value = totalDescuentos;

                fila++;

                hoja.Cell(fila, 1).Value = "TOTAL NÓMINA";
                hoja.Cell(fila, 2).Value = totalNomina;

                //====================================================
                // Dar formato al resumen
                //====================================================

                hoja.Range(fila - 7, 1, fila, 2)
                    .Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                hoja.Range(fila - 7, 1, fila, 2)
                    .Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                hoja.Cell(fila, 1).Style.Font.Bold = true;
                hoja.Cell(fila, 2).Style.Font.Bold = true;

                hoja.Range(fila - 5, 2, fila, 2)
                    .Style.NumberFormat.Format = "$#,##0.00";

                hoja.Cell(fila - 6, 2)
                    .Style.NumberFormat.Format = "0.00";

                //====================================================
                // Formato general de la hoja
                //====================================================

                hoja.Column(2).Style.NumberFormat.Format = "0.00";

                hoja.Columns(3, 9).Style.NumberFormat.Format = "$#,##0.00";

                hoja.Range("A1:I1").Merge();

                hoja.Range("A2:I2").Merge();

                hoja.Range("A4:I4").Merge();

                hoja.Range("A5:I5").Merge();

                hoja.Cell("A1").Style.Font.Bold = true;
                hoja.Cell("A1").Style.Font.FontSize = 18;
                hoja.Cell("A1").Style.Alignment.Horizontal =
                    XLAlignmentHorizontalValues.Center;

                hoja.Cell("A2").Style.Font.Bold = true;
                hoja.Cell("A2").Style.Font.FontSize = 14;
                hoja.Cell("A2").Style.Alignment.Horizontal =
                    XLAlignmentHorizontalValues.Center;

                //====================================================
                // Bordes de la tabla
                //====================================================

                var rangoTabla = hoja.Range(7, 1, 7 + lista.Count, 9);

                rangoTabla.Style.Border.OutsideBorder =
                    XLBorderStyleValues.Thin;

                rangoTabla.Style.Border.InsideBorder =
                    XLBorderStyleValues.Thin;

                //====================================================
                // Configuración de impresión
                //====================================================

                hoja.PageSetup.PageOrientation =
                    XLPageOrientation.Landscape;

                hoja.PageSetup.PaperSize =
                    XLPaperSize.LetterPaper;

                hoja.PageSetup.FitToPages(1, 1);

                hoja.PageSetup.CenterHorizontally = true;

                //====================================================
                // Congelar encabezado
                //====================================================

                hoja.SheetView.FreezeRows(7);

                //====================================================
                // Ajustar automáticamente el ancho de columnas
                //====================================================

                hoja.Columns().AdjustToContents();

                //====================================================
                // Guardar archivo
                //====================================================

                libro.SaveAs(rutaArchivo);
            }
        }
    }
}
