using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NominaPro.Servicios;
using NominaPro.Utilidades;
using System.IO;


namespace NominaPro.Formularios
{
    public partial class FrmImportarChecadas : Form
    {
        public FrmImportarChecadas()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnSeleccionar_Click(object sender, EventArgs e)
        {
            openFileDialog1.Title = "Seleccionar archivo de checadas";

            openFileDialog1.Filter = "Archivos de Excel|*.xlsx";

            openFileDialog1.Multiselect = false;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                txtArchivo.Text = openFileDialog1.FileName;

                lblEstado.Text = "Archivo seleccionado correctamente.";

                btnImportar.Enabled = true;
            }
        }

        private void lblEstado_Click(object sender, EventArgs e)
        {

        }

        private void btnImportar_Click(object sender, EventArgs e)
        {
            try
            {
                ExcelHelper helper = new ExcelHelper();

                var lista = helper.LeerReporteAsistencia(txtArchivo.Text);

                dgvVistaPrevia.DataSource = lista;

                ChecadasService servicio = new ChecadasService();

                MessageBox.Show(servicio.GuardarChecadas(lista));

                EstadoSistema.ArchivoCargado = true;
                EstadoSistema.NombreArchivo = Path.GetFileName(txtArchivo.Text);

                var principal = Application.OpenForms["FrmPrincipal"] as FrmPrincipal;

                if (principal != null)
                {
                    principal.ActualizarEstadoArchivo();
                }
            }
            catch (IOException)
            {
                MessageBox.Show(
                    "El archivo de Excel está abierto.\n\nCierre el archivo y vuelva a intentarlo.",
                    "Archivo en uso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Ocurrió un error al importar el archivo.\n\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }



        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
