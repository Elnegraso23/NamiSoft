using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NominaPro.Enumeraciones;

namespace NominaPro.Modelos
{
    public class ChecadaEmpleado
    {
        public int Id { get; set; }

        public string NumeroEmpleado { get; set; } = "";

        public string Nombre { get; set; } = "";

        public DateTime Fecha { get; set; }

        public TimeSpan? PrimeraChecada { get; set; }

        public TimeSpan? UltimaChecada { get; set; }

        public decimal HorasExtra { get; set; } = 0;

        public bool ModificadaManual { get; set; } = false;

        public decimal HorasTrabajadas { get; set; }

        public int RetardoMinutos { get; set; }

        public bool TieneRetardo { get; set; }

        public OrigenChecada Origen { get; set; } = OrigenChecada.Sistema;

        public DateTime CreadaEn { get; set; }

        public DateTime? ModificadaEn { get; set; }

        public string? Motivo { get; set; }

        public bool FaltaEntrada => !PrimeraChecada.HasValue;

        public bool FaltaSalida => PrimeraChecada.HasValue && !UltimaChecada.HasValue;

        public bool Completa => PrimeraChecada.HasValue && UltimaChecada.HasValue;

        public string EstadoTexto
        {
            get
            {
                string estado = Completa ? "Completo" : FaltaSalida ? "Falta salida" : "Falta entrada";
                return ModificadaManual ? estado + " · Modificada" : estado;
            }
        }

        public string EntradaTexto => PrimeraChecada.HasValue ? FormatoCorto(PrimeraChecada.Value) : "--:--";

        public string SalidaTexto => UltimaChecada.HasValue ? FormatoCorto(UltimaChecada.Value) : "--:--";

        private static string FormatoCorto(TimeSpan hora) => hora.ToString(@"hh\:mm");
    }
}