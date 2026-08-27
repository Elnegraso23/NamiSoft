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
using Microsoft.EntityFrameworkCore;
using NominaPro.Modelos;
using NominaPro.Servicios;

namespace NominaPro.Formularios
{
    public partial class FrmHorarioManual : Form
    {
        public FrmHorarioManual()
        {
            InitializeComponent();

            dvgHorarioManual.EditMode = DataGridViewEditMode.EditOnEnter;

            CargarEmpleados();
        }

        private void CargarEmpleados()
        {
            using (var db = new NominaContext())
            {
                var empleados = db.Empleados
                    .OrderBy(e => e.NumeroEmpleado)
                    .ToList();

                cmbEmpleado.DataSource = empleados;
                cmbEmpleado.DisplayMember = "NombreCompleto";
                cmbEmpleado.ValueMember = "NumeroEmpleado";
                cmbEmpleado.SelectedIndex = -1;
            }
        }

        private void CargarHorario()
        {
            if (cmbEmpleado.SelectedValue == null)
            {
                MessageBox.Show("Seleccione un empleado.");
                return;
            }

            string numeroEmpleado = cmbEmpleado.SelectedValue.ToString()!;

            DateTime fechaSeleccionada = dtpSemana.Value.Date;

            int diferencia = fechaSeleccionada.DayOfWeek == DayOfWeek.Sunday
                ? 6
                : (int)fechaSeleccionada.DayOfWeek - 1;

            DateTime inicioSemana = fechaSeleccionada.AddDays(-diferencia);

            DateTime finSemana = inicioSemana.AddDays(6);

            using (var db = new NominaContext())
            {
                var lista = db.Checadas
                    .Where(c => c.NumeroEmpleado == numeroEmpleado
                             && c.Fecha >= inicioSemana
                             && c.Fecha <= finSemana)
                    .OrderBy(c => c.Fecha)
                    .ToList();

                var datos = lista.Select(c => new HorarioManualItem
                {
                    Id = c.Id,

                    Fecha = c.Fecha,

                    Entrada = c.PrimeraChecada.HasValue
                        ? c.PrimeraChecada.Value.ToString(@"hh\:mm")
                        : "",

                    Salida = c.UltimaChecada.HasValue
                        ? c.UltimaChecada.Value.ToString(@"hh\:mm")
                        : "",

                    HorasTrabajadas = c.HorasTrabajadas,

                    RetardoMinutos = c.RetardoMinutos,

                    TieneRetardo = c.TieneRetardo,

                    Modificado = c.ModificadaManual

                }).ToList();

                dvgHorarioManual.DataSource = datos;

                dvgHorarioManual.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                dvgHorarioManual.AllowUserToAddRows = false;
                dvgHorarioManual.AllowUserToDeleteRows = false;

                dvgHorarioManual.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dvgHorarioManual.MultiSelect = false;

                dvgHorarioManual.Columns["Id"].Visible = false;

                dvgHorarioManual.Columns["Fecha"].HeaderText = "Fecha";
                dvgHorarioManual.Columns["Entrada"].HeaderText = "Entrada";
                dvgHorarioManual.Columns["Salida"].HeaderText = "Salida";
                dvgHorarioManual.Columns["HorasTrabajadas"].HeaderText = "Horas Trabajadas";
                dvgHorarioManual.Columns["RetardoMinutos"].HeaderText = "Retardo (min)";
                dvgHorarioManual.Columns["TieneRetardo"].HeaderText = "Retardo";
                dvgHorarioManual.Columns["Modificado"].HeaderText = "Modificado";

                dvgHorarioManual.Columns["Fecha"].ReadOnly = true;
                dvgHorarioManual.Columns["HorasTrabajadas"].ReadOnly = true;
                dvgHorarioManual.Columns["RetardoMinutos"].ReadOnly = true;
                dvgHorarioManual.Columns["TieneRetardo"].ReadOnly = true;
                dvgHorarioManual.Columns["Modificado"].ReadOnly = true;

                dvgHorarioManual.Columns["Fecha"].FillWeight = 20;
                dvgHorarioManual.Columns["Entrada"].FillWeight = 15;
                dvgHorarioManual.Columns["Salida"].FillWeight = 15;
                dvgHorarioManual.Columns["HorasTrabajadas"].FillWeight = 20;
                dvgHorarioManual.Columns["RetardoMinutos"].FillWeight = 15;
                dvgHorarioManual.Columns["TieneRetardo"].FillWeight = 10;
                dvgHorarioManual.Columns["Modificado"].FillWeight = 10;
            }
        }


        private void FrmHorarioManual_Load(object sender, EventArgs e)
        {

        }

        private void LblTit_Click(object sender, EventArgs e)
        {

        }

        private void LblSemana_Click(object sender, EventArgs e)
        {

        }

        private void BtnBuscar_Click(object sender, EventArgs e)
        {
            using (var db = new NominaContext())
            {
                MessageBox.Show($"Checadas registradas: {db.Checadas.Count()}");
            }

            CargarHorario();
        }

        private void btnGuardarCambios_Click(object sender, EventArgs e)
        {
            HorarioManualService servicio = new HorarioManualService();

            var lista = (List<HorarioManualItem>)dvgHorarioManual.DataSource;

            MessageBox.Show(servicio.GuardarCambios(lista));

            CargarHorario();
        }

        private void dvgHorarioManual_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {


        }

        private void dvgHorarioManual_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            string nombreColumna = dvgHorarioManual.Columns[e.ColumnIndex].Name;

            // Solo validar Entrada y Salida
            if (nombreColumna != "Entrada" && nombreColumna != "Salida")
                return;

            string valor = e.FormattedValue?.ToString()?.Trim() ?? "";

            // Se permite dejar la celda vacía
            if (string.IsNullOrWhiteSpace(valor))
                return;

            if (!TimeSpan.TryParse(valor, out _))
            {
                MessageBox.Show(
                    "La hora no tiene un formato válido.\n\nEjemplos válidos:\n07:00\n14:30\n22:00",
                    "Hora incorrecta",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                e.Cancel = true;
            }
        }

        private void btnActualizarRetardos_Click(object sender, EventArgs e)
        {
            HorarioManualService servicio = new HorarioManualService();

            var lista = (List<HorarioManualItem>)dvgHorarioManual.DataSource;

            MessageBox.Show(servicio.GuardarCambios(lista));

            CargarHorario();
        }
    }
}
