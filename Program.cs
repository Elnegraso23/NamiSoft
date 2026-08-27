using NominaPro.Datos;

namespace NominaPro
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            using (var db = new NominaContext())
            {
                db.Database.EnsureCreated();
            }

            Application.Run(new FrmPrincipal());
        }
    }
}