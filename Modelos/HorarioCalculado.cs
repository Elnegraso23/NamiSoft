using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NamiSoft.Modelos
{
    public class HorarioCalculado
    {
        public TimeSpan Entrada { get; set; }

        public TimeSpan Salida { get; set; }

        public int Tolerancia { get; set; }

        public bool AplicaRetardo { get; set; } = true;
    }
}