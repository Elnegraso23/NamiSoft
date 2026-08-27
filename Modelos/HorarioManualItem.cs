using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NominaPro.Modelos
{
    public class HorarioManualItem
    {
        public int Id { get; set; }

        public DateTime Fecha { get; set; }

        public string Entrada { get; set; } = "";

        public string Salida { get; set; } = "";

        public decimal HorasTrabajadas { get; set; }

        public int RetardoMinutos { get; set; }

        public bool TieneRetardo { get; set; }

        public bool Modificado { get; set; }
    }
}
