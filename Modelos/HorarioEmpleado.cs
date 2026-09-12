using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NamiSoft.Modelos
{
    public class HorarioEmpleado
    {
        public int Id { get; set; }

        public string NumeroEmpleado { get; set; } = "";

        public string DiaSemana { get; set; } = "";

        public TimeSpan HoraEntrada { get; set; }

        public TimeSpan HoraSalida { get; set; }
    }
}