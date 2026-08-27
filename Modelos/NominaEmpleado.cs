using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace NominaPro.Modelos
{
    public class NominaEmpleado
    {
        public string NumeroEmpleado { get; set; } = "";

        public string Nombre { get; set; } = "";

        public decimal HorasTrabajadas { get; set; }

        public decimal SalarioHora { get; set; }

        public decimal SueldoBase { get; set; }

        public int TotalRetardos { get; set; }

        public decimal DescuentoRetardos { get; set; }

        public decimal Propinas { get; set; }

        public decimal PrimaDominical { get; set; }

        public decimal TotalPagar { get; set; }
    }
}
