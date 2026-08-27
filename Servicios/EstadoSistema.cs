using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace NominaPro.Servicios
{
    internal class EstadoSistema
    {
        public static bool ArchivoCargado { get; set; } = false;

        public static string NombreArchivo { get; set; } = "Ninguno";
    }
}
