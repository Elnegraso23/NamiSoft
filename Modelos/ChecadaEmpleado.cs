using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}