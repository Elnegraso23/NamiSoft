using System.Windows.Forms;
using NominaPro.Formularios;
using NominaPro.Servicios;
using FontAwesome.Sharp;

namespace NominaPro
{
    public partial class FrmPrincipal : Form
    {
        public FrmPrincipal()
        {
            InitializeComponent();

        

        }




        private void ConfigurarBoton(Button boton, IconChar icono)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void panelMenu_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }



        private void label1_Click_2(object sender, EventArgs e)
        {

        }

        private void panelContenedor_Paint(object sender, PaintEventArgs e)
        {

        }

        private void LblEstado_Click(object sender, EventArgs e)
        {

        }

        private void btnEmpleados_Click(object sender, EventArgs e)
        {
            AbrirFormulario(new FrmEmpleados());
        }

        //METODO PARA HACER MAS FACIL ABRIR LAS VENTANAS

        private void AbrirFormulario(Form formulario)
        {
            panelContenedor.Controls.Clear();

            formulario.TopLevel = false;
            formulario.FormBorderStyle = FormBorderStyle.None;
            formulario.Dock = DockStyle.Fill;

            panelContenedor.Controls.Add(formulario);
            panelContenedor.Tag = formulario;

            formulario.Show();

            ActualizarEstadoArchivo();
        }

        private void btnNomina_Click(object sender, EventArgs e)
        {
            AbrirFormulario(new FrmNomina());
        }

        private void btnHorario_Click(object sender, EventArgs e)
        {
            AbrirFormulario(new FrmHorarioManual());

        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void LblFecha_Click(object sender, EventArgs e)
        {

        }
        private void FrmPrincipal_Load(object sender, EventArgs e)
        {
            TimerFecha.Start();
        }

        private void panelMenu_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void TimerFecha_Tick_1(object sender, EventArgs e)
        {
            LblFecha.Text = DateTime.Now.ToString("dd/MM/yyyy\nhh:mm:ss tt");
        }

        private void lblEstadoArchivo_Click(object sender, EventArgs e)
        {

        }

        public void ActualizarEstadoArchivo()
        {
            if (EstadoSistema.ArchivoCargado)
            {
                lblEstado.Text = "ESTADO ●";
                lblEstado.ForeColor = Color.Green;

                lblEstadoTexto.Text = "Archivo cargado";
                lblArchivo.Text = EstadoSistema.NombreArchivo;
            }
            else
            {
                lblEstado.Text = "ESTADO ●";
                lblEstado.ForeColor = Color.Red;

                lblEstadoTexto.Text = "Sin archivo cargado";
                lblArchivo.Text = "Ninguno";
            }
        }

        private void btnChecadas_Click(object sender, EventArgs e)
        {
            AbrirFormulario(new FrmImportarChecadas());
        }

        private void lblArchivo_Click(object sender, EventArgs e)
        {

        }

        private void btnConfiguracion_Click(object sender, EventArgs e)
        {
            AbrirFormulario(new FrmConfiguracion());
        }

        private void ibtnEmpleados_Click(object sender, EventArgs e)
        {

        }
    }
}
