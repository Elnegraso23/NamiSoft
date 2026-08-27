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

namespace NominaPro.Formularios
{
    public partial class FrmNomina : Form
    {
        public FrmNomina()
        {
            InitializeComponent();
        }

        private void btnCalcularNomina_Click(object sender, EventArgs e)
        {
            NominaService servicio = new NominaService();

            dvgNomina.DataSource = servicio.CalcularNomina(dtpSemana.Value, nudPropinas.Value);

            dvgNomina.Columns["NumeroEmpleado"].Visible = false;

            dvgNomina.Columns["Nombre"].HeaderText = "Empleado";
            dvgNomina.Columns["HorasTrabajadas"].HeaderText = "Horas";
            dvgNomina.Columns["SalarioHora"].HeaderText = "$ Hora";
            dvgNomina.Columns["SueldoBase"].HeaderText = "Sueldo";
            dvgNomina.Columns["TotalRetardos"].HeaderText = "Retardos";
            dvgNomina.Columns["DescuentoRetardos"].HeaderText = "Desc.";
            dvgNomina.Columns["PrimaDominical"].HeaderText = "Prima Dom.";
            dvgNomina.Columns["TotalPagar"].HeaderText = "Total";

            dvgNomina.Columns["SalarioHora"].DefaultCellStyle.Format = "C2";
            dvgNomina.Columns["SueldoBase"].DefaultCellStyle.Format = "C2";
            dvgNomina.Columns["DescuentoRetardos"].DefaultCellStyle.Format = "C2";
            dvgNomina.Columns["Propinas"].DefaultCellStyle.Format = "C2";
            dvgNomina.Columns["PrimaDominical"].DefaultCellStyle.Format = "C2";
            dvgNomina.Columns["TotalPagar"].DefaultCellStyle.Format = "C2";

            btnExportarExcel.Enabled = true;
            dvgNomina.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dvgNomina.AllowUserToAddRows = false;
            dvgNomina.AllowUserToDeleteRows = false;
            dvgNomina.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dvgNomina.MultiSelect = false;
        }

        private void btnExportarExcel_Click(object sender, EventArgs e)
        {
            SaveFileDialog guardar = new SaveFileDialog();

            guardar.Filter = "Archivo Excel|*.xlsx";

            guardar.FileName = $"Nomina Casa Red {dtpSemana.Value:dd-MM-yyyy}.xlsx";

            if (guardar.ShowDialog() != DialogResult.OK)
                return;

            NominaService nominaService = new NominaService();

            var lista = nominaService.CalcularNomina(
                dtpSemana.Value,
                nudPropinas.Value);

            ExcelNominaService excel = new ExcelNominaService();

            excel.ExportarNomina(
                lista,
                dtpSemana.Value,
                nudPropinas.Value,
                guardar.FileName);

            MessageBox.Show("La nómina se exportó correctamente.");
        }
    }
}
