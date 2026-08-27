using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NominaPro.Datos;
using NominaPro.Modelos;

namespace NominaPro.Formularios
{
    public partial class FrmConfiguracion : Form
    {
        public FrmConfiguracion()
        {
            InitializeComponent();

            CargarConfiguracion();
        }

        private void CargarConfiguracion()
        {
            using (var db = new NominaContext())
            {
                var config = db.ConfiguracionSistema.FirstOrDefault();

                if (config == null)
                {
                    config = new ConfiguracionSistema
                    {
                        EntradaManana = new TimeSpan(7, 0, 0),
                        SalidaManana = new TimeSpan(17, 0, 0),
                        ToleranciaManana = 10,

                        EntradaTarde = new TimeSpan(12, 0, 0),
                        SalidaTarde = new TimeSpan(22, 0, 0),
                        ToleranciaTarde = 10,

                        CadaRetardos = 3,
                        DescuentoRetardosMinutos = 30,

                        AplicarPrimaDominical = true,
                        PorcentajePrimaDominical = 25
                    };

                    db.ConfiguracionSistema.Add(config);
                    db.SaveChanges();
                }

                dtpEntradaManana.Value = DateTime.Today + config.EntradaManana;
                dtpSalidaMañana.Value = DateTime.Today + config.SalidaManana;

                nudToleranciaManana.Value = config.ToleranciaManana;

                dtpEntradaTarde.Value = DateTime.Today + config.EntradaTarde;
                dtpSalidaTarde.Value = DateTime.Today + config.SalidaTarde;

                nudToleranciaTarde.Value = config.ToleranciaTarde;

                nudCadaRetardos.Value = config.CadaRetardos;
                nudDescuentoRetardos.Value = config.DescuentoRetardosMinutos;

                chkPrimaDominical.Checked = config.AplicarPrimaDominical;
                nudPrimaDominical.Value = config.PorcentajePrimaDominical;
            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void FrmConfiguracion_Load(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void lblTurnomañana_Click(object sender, EventArgs e)
        {

        }

        private void btnGuardarConfiguracion_Click(object sender, EventArgs e)
        {
            using (var db = new NominaContext())
            {
                var config = db.ConfiguracionSistema.First();

                config.EntradaManana = dtpEntradaManana.Value.TimeOfDay;
                config.SalidaManana = dtpSalidaMañana.Value.TimeOfDay;
                config.ToleranciaManana = (int)nudToleranciaManana.Value;

                config.EntradaTarde = dtpEntradaTarde.Value.TimeOfDay;
                config.SalidaTarde = dtpSalidaTarde.Value.TimeOfDay;
                config.ToleranciaTarde = (int)nudToleranciaTarde.Value;

                config.CadaRetardos = (int)nudCadaRetardos.Value;
                config.DescuentoRetardosMinutos = (int)nudDescuentoRetardos.Value;

                config.AplicarPrimaDominical = chkPrimaDominical.Checked;
                config.PorcentajePrimaDominical = nudPrimaDominical.Value;

                db.SaveChanges();
            }

            MessageBox.Show(
                "La configuración se guardó correctamente.",
                "NominaPro",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }
    }
}
