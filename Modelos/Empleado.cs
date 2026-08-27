using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NominaPro.Enumeraciones;

namespace NominaPro.Modelos
{
    public class Empleado
    {
        public int Id { get; set; }

        public string NumeroEmpleado { get; set; } = "";

        public string Nombre { get; set; } = "";

        public string Puesto { get; set; } = "";

        public string Departamento { get; set; } = "";

        public decimal SalarioHora { get; set; }

        // ← SE MANTIENE TEMPORALMENTE
        public string Turno { get; set; } = "";

        // ← NUEVA PROPIEDAD
        public TipoHorario TipoHorario { get; set; } = TipoHorario.Mañana;

        public string Descanso { get; set; } = "";

        public bool Activo { get; set; } = true;

        public string NombreCompleto
        {
            get
            {
                return $"{NumeroEmpleado} - {Nombre}";
            }
        }
    }
}
