using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NominaPro.Modelos
{
    public class ConfiguracionSistema
    {
        public int Id { get; set; }

        // Turno mañana
        public TimeSpan EntradaManana { get; set; }
        public TimeSpan SalidaManana { get; set; }
        public int ToleranciaManana { get; set; }

        // Turno tarde
        public TimeSpan EntradaTarde { get; set; }
        public TimeSpan SalidaTarde { get; set; }
        public int ToleranciaTarde { get; set; }

        // Retardos
        public int CadaRetardos { get; set; }
        public int DescuentoRetardosMinutos { get; set; }

        // Prima dominical
        public bool AplicarPrimaDominical { get; set; }
        public decimal PorcentajePrimaDominical { get; set; }
    }
}
