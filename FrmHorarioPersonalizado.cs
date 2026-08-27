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

namespace NominaPro
{
    public partial class FrmHorarioPersonalizado : Form
    {
        public string NumeroEmpleado { get; set; } = "";

        public List<HorarioEmpleado> Horarios { get; private set; } = new();

        public FrmHorarioPersonalizado()
        {
            InitializeComponent();

            ConfigurarGrid();

            Load += FrmHorarioPersonalizado_Load;
        }

        private void FrmHorarioPersonalizado_Load(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NumeroEmpleado))
                return;

            using (var db = new NominaContext())
            {
                var horarios = db.HorariosEmpleado
                    .Where(x => x.NumeroEmpleado == NumeroEmpleado)
                    .ToList();

                foreach (DataGridViewRow fila in dgvHorario.Rows)
                {
                    string dia = fila.Cells["Dia"].Value?.ToString() ?? "";

                    var horario = horarios.FirstOrDefault(x => x.DiaSemana == dia);

                    if (horario == null)
                        continue;

                    fila.Cells["Entrada"].Value =
                        horario.HoraEntrada.ToString(@"hh\:mm");

                    fila.Cells["Salida"].Value =
                        horario.HoraSalida.ToString(@"hh\:mm");
                }
            }
        }

        private void ConfigurarGrid()
        {
            dgvHorario.Columns.Clear();

            dgvHorario.Columns.Add("Dia", "Día");
            dgvHorario.Columns.Add("Entrada", "Entrada");
            dgvHorario.Columns.Add("Salida", "Salida");

            dgvHorario.Rows.Add("Lunes", "00:00", "00:00");
            dgvHorario.Rows.Add("Martes", "00:00", "00:00");
            dgvHorario.Rows.Add("Miércoles", "00:00", "00:00");
            dgvHorario.Rows.Add("Jueves", "00:00", "00:00");
            dgvHorario.Rows.Add("Viernes", "00:00", "00:00");
            dgvHorario.Rows.Add("Sábado", "00:00", "00:00");
            dgvHorario.Rows.Add("Domingo", "00:00", "00:00");

            dgvHorario.Columns["Dia"].ReadOnly = true;

            dgvHorario.AllowUserToAddRows = false;
            dgvHorario.AllowUserToDeleteRows = false;
            dgvHorario.RowHeadersVisible = false;

            dgvHorario.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgvHorario.SelectionMode = DataGridViewSelectionMode.CellSelect;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            Horarios.Clear();

            foreach (DataGridViewRow fila in dgvHorario.Rows)
            {
                HorarioEmpleado horario = new HorarioEmpleado
                {
                    DiaSemana = fila.Cells["Dia"].Value?.ToString() ?? "",
                    HoraEntrada = TimeSpan.Parse(fila.Cells["Entrada"].Value?.ToString() ?? "00:00"),
                    HoraSalida = TimeSpan.Parse(fila.Cells["Salida"].Value?.ToString() ?? "00:00")
                };

                Horarios.Add(horario);
            }

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
