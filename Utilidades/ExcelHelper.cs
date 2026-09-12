using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClosedXML.Excel;
using System.Data;
using NamiSoft.Modelos;
using System.Globalization;
using System.Text.RegularExpressions;

namespace NamiSoft.Utilidades
{
    public class ExcelHelper
    {
        public List<ChecadaImportada> LeerReporteAsistencia(string rutaArchivo)
        {
            List<ChecadaImportada> lista = new();

            using (var workbook = new XLWorkbook(rutaArchivo))
            {
                var hoja = workbook.Worksheet("Reporte de Asistencia");

                // Leer el periodo
                string periodo = hoja.Cell(3, 3).GetString();

                DateTime fechaInicio = DateTime.Parse(
                    periodo.Split('~')[0].Trim(),
                    CultureInfo.InvariantCulture);

                // Días (13 al 19)
                List<DateTime> fechas = new();

                for (int c = 1; c <= 7; c++)
                {
                    int dia = hoja.Cell(4, c).GetValue<int>();

                    fechas.Add(new DateTime(
                        fechaInicio.Year,
                        fechaInicio.Month,
                        dia));
                }

                int fila = 5;

                while (true)
                {
                    string texto = hoja.Cell(fila, 1).GetString();

                    if (string.IsNullOrWhiteSpace(texto))
                        break;

                    if (texto != "ID:")
                    {
                        fila++;
                        continue;
                    }

                    string numeroEmpleado = hoja.Cell(fila, 3).GetString();
                    string nombre = hoja.Cell(fila, 11).GetString();

                    // La siguiente fila contiene las checadas
                    int filaChecadas = fila + 1;

                    for (int columna = 1; columna <= 7; columna++)
                    {
                        string contenido = hoja.Cell(filaChecadas, columna).GetString();

                        if (string.IsNullOrWhiteSpace(contenido))
                            continue;

                        List<string> horas = ObtenerHoras(contenido);

                        if (horas.Count == 0)
                            continue;

                        ChecadaImportada registro = new ChecadaImportada();

                        registro.NumeroEmpleado = numeroEmpleado;
                        registro.Nombre = nombre;
                        registro.Fecha = fechas[columna - 1];
                        registro.PrimeraChecada = TimeSpan.Parse(horas.First());
                        registro.UltimaChecada = TimeSpan.Parse(horas.Last());

                        lista.Add(registro);
                    }

                    fila += 2;
                }
            }

            return lista;
        }

        private List<string> ObtenerHoras(string texto)
        {
            List<string> horas = new();

            MatchCollection matches = Regex.Matches(texto, @"\d{2}:\d{2}");

            foreach (Match match in matches)
            {
                horas.Add(match.Value);
            }

            return horas;
        }
    }
}