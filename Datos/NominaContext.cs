using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NominaPro.Modelos;

namespace NominaPro.Datos
{
    public class NominaContext : DbContext
    {
        public DbSet<Empleado> Empleados { get; set; }

        public DbSet<ChecadaEmpleado> Checadas { get; set; }

        public DbSet<HorarioEmpleado> HorariosEmpleado { get; set; }

        public DbSet<ConfiguracionSistema> ConfiguracionSistema { get; set; }

        public DbSet<ChecadaPendiente> ChecadasPendientes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=Nomina.db");
        }
    }
}
