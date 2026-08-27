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
using NominaPro.Enumeraciones;

namespace NominaPro.Formularios
{
    public partial class FrmNuevoEmpleado : Form
    {
        public Empleado? EmpleadoEditar { get; set; }

        private List<HorarioEmpleado> horariosPersonalizados = new();

        public FrmNuevoEmpleado()
        {
            InitializeComponent();

            checkEstado.Checked = true;

            btnHorarioPersonalizado.Visible = false;
        }

        private void FrmNuevoEmpleado_Load(object sender, EventArgs e)
        {
            if (EmpleadoEditar != null)
            {
                txtNumero.Text = EmpleadoEditar.NumeroEmpleado;
                txtNombre.Text = EmpleadoEditar.Nombre;
                txtPuesto.Text = EmpleadoEditar.Puesto;
                txtDepartamento.Text = EmpleadoEditar.Departamento;
                txtSalario.Text = EmpleadoEditar.SalarioHora.ToString();
                cmbTurno.Text = EmpleadoEditar.TipoHorario switch
                {
                    TipoHorario.Mañana => "Mañana",
                    TipoHorario.Tarde => "Tarde",
                    TipoHorario.Personalizado => "Personalizado",
                    TipoHorario.Especial => "Especial",
                    _ => "Mañana"
                };
                cmbDescanso.Text = EmpleadoEditar.Descanso;
                checkEstado.Checked = EmpleadoEditar.Activo;

                this.Text = "Editar Empleado";
                btnGuardar.Text = "Actualizar";

                if (EmpleadoEditar.TipoHorario == TipoHorario.Personalizado)
                {
                    using (var db = new NominaContext())
                    {
                        horariosPersonalizados = db.HorariosEmpleado
                            .Where(x => x.NumeroEmpleado == EmpleadoEditar.NumeroEmpleado)
                            .ToList();
                    }
                }
            }
        }

        private void cmbTurno_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnHorarioPersonalizado.Visible =
     cmbTurno.Text == "Personalizado";

            if (cmbTurno.Text != "Personalizado")
            {
                horariosPersonalizados.Clear();
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (txtNumero.Text.Trim() == "")
            {
                MessageBox.Show("Ingrese el número de empleado.");
                txtNumero.Focus();
                return;
            }

            if (txtNombre.Text.Trim() == "")
            {
                MessageBox.Show("Ingrese el nombre.");
                txtNombre.Focus();
                return;
            }

            if (txtPuesto.Text.Trim() == "")
            {
                MessageBox.Show("Ingrese el puesto.");
                txtPuesto.Focus();
                return;
            }

            if (!decimal.TryParse(txtSalario.Text, out decimal salario))
            {
                MessageBox.Show("Salario inválido.");
                txtSalario.Focus();
                return;
            }

            using (var db = new NominaContext())
            {
                bool existe = db.Empleados.Any(e => e.NumeroEmpleado == txtNumero.Text && (EmpleadoEditar == null || e.Id != EmpleadoEditar.Id));

                if (existe)
                {
                    MessageBox.Show("Ese número de empleado ya existe.");

                    return;
                }

                Empleado empleado;

                if (EmpleadoEditar == null)
                {
                    empleado = new Empleado();
                    db.Empleados.Add(empleado);
                }
                else
                {
                    empleado = db.Empleados.Find(EmpleadoEditar.Id)!;
                }

                empleado.NumeroEmpleado = txtNumero.Text;
                empleado.Nombre = txtNombre.Text;
                empleado.Puesto = txtPuesto.Text;
                empleado.Departamento = txtDepartamento.Text;
                empleado.SalarioHora = Convert.ToDecimal(txtSalario.Text);

                // Compatibilidad con la versión 1.0
                empleado.Turno = cmbTurno.Text;

                // Nuevo sistema de horarios
                empleado.TipoHorario = cmbTurno.Text switch
                {
                    "Mañana" => TipoHorario.Mañana,
                    "Tarde" => TipoHorario.Tarde,
                    "Personalizado" => TipoHorario.Personalizado,
                    "Especial" => TipoHorario.Especial,
                    _ => TipoHorario.Mañana
                };

                empleado.Descanso = cmbDescanso.Text;
                empleado.Activo = checkEstado.Checked;

                // Verificar si ya tiene un horario guardado
                bool tieneHorarioGuardado = db.HorariosEmpleado.Any(x =>
                    x.NumeroEmpleado == empleado.NumeroEmpleado);

                // Validar horario personalizado
                if (empleado.TipoHorario == TipoHorario.Personalizado &&
                    horariosPersonalizados.Count == 0 &&
                    !tieneHorarioGuardado)
                {
                    MessageBox.Show(
                        "Debe configurar un horario personalizado antes de guardar el empleado.",
                        "Horario requerido",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return;
                }

                // Ahora sí guardar el empleado
                db.SaveChanges();

                // Administrar horarios personalizados
                if (empleado.TipoHorario == TipoHorario.Personalizado)
                {
                    // Solo reemplazar si el usuario configuró un horario nuevo
                    if (horariosPersonalizados.Count > 0)
                    {
                        var horariosViejos = db.HorariosEmpleado
                            .Where(x => x.NumeroEmpleado == empleado.NumeroEmpleado)
                            .ToList();

                        db.HorariosEmpleado.RemoveRange(horariosViejos);

                        foreach (var horario in horariosPersonalizados)
                        {
                            horario.NumeroEmpleado = empleado.NumeroEmpleado;
                            db.HorariosEmpleado.Add(horario);
                        }
                    }
                }
                else
                {
                    // Si cambió de Personalizado a otro tipo,
                    // eliminar cualquier horario existente.
                    var horariosViejos = db.HorariosEmpleado
                        .Where(x => x.NumeroEmpleado == empleado.NumeroEmpleado)
                        .ToList();

                    if (horariosViejos.Any())
                    {
                        db.HorariosEmpleado.RemoveRange(horariosViejos);
                    }
                }

                db.SaveChanges();
            }

            MessageBox.Show("Empleado guardado correctamente.");

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void txtNumero_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnHorarioPersonalizado_Click(object sender, EventArgs e)
        {
            FrmHorarioPersonalizado frm = new FrmHorarioPersonalizado();

            // Si estamos editando un empleado existente,
            // enviar su número para cargar su horario.
            if (EmpleadoEditar != null)
            {
                frm.NumeroEmpleado = EmpleadoEditar.NumeroEmpleado;
            }

            if (frm.ShowDialog() == DialogResult.OK)
            {
                horariosPersonalizados = frm.Horarios;

                MessageBox.Show(
                    "Horario personalizado configurado correctamente.",
                    "Horario",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }
    }
    }

