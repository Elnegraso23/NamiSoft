using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NominaPro.Modelos
{
    public class ChecadaImportada
    {
        public string NumeroEmpleado { get; set; } = "";

        public string Nombre { get; set; } = "";

        public DateTime Fecha { get; set; }

        public TimeSpan? PrimeraChecada { get; set; }

        public TimeSpan? UltimaChecada { get; set; }

        public string EntradaTexto => PrimeraChecada.HasValue ? PrimeraChecada.Value.ToString(@"hh\:mm") : "--:--";

        public string SalidaTexto => UltimaChecada.HasValue ? UltimaChecada.Value.ToString(@"hh\:mm") : "--:--";

    }
}
