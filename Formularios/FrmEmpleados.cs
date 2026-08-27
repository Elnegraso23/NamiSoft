using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using NominaPro.Datos;
using NominaPro.Modelos;
using NominaPro.Servicios;

namespace NominaPro.Formularios
{
    public partial class FrmEmpleados : Form
    {
        public FrmEmpleados()
        {
            InitializeComponent();
        }

        private void FrmEmpleados_Load(object sender, EventArgs e)
        {
            CargarEmpleados();
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            FrmNuevoEmpleado frm = new FrmNuevoEmpleado();

            if (frm.ShowDialog() == DialogResult.OK)
            {
                CargarEmpleados();
            }
        }

        private void CargarEmpleados()
        {
            using (var db = new NominaContext())
            {
                var consulta = db.Empleados.AsQueryable();

                switch (cmbFiltroEstado.Text)
                {
                    case "Activos":
                        consulta = consulta.Where(e => e.Activo);
                        break;

                    case "Inactivos":
                        consulta = consulta.Where(e => !e.Activo);
                        break;

                    case "Todos":
                        break;
                }

                dvgEmpleados.DataSource = consulta.ToList();
            }

            dvgEmpleados.Columns["Id"].Visible = false;

            dvgEmpleados.Columns["TipoHorario"].Visible = false;

            dvgEmpleados.Columns["NumeroEmpleado"].HeaderText = "Número";

            dvgEmpleados.Columns["Nombre"].HeaderText = "Nombre";

            dvgEmpleados.Columns["Puesto"].HeaderText = "Puesto";

            dvgEmpleados.Columns["Departamento"].HeaderText = "Departamento";

            dvgEmpleados.Columns["SalarioHora"].HeaderText = "Salario Hora";

            dvgEmpleados.Columns["Turno"].HeaderText = "Turno";

            dvgEmpleados.Columns["Descanso"].HeaderText = "Descanso";

            dvgEmpleados.Columns["Activo"].HeaderText = "Activo";

            dvgEmpleados.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }



        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (dvgEmpleados.CurrentRow == null)
            {
                MessageBox.Show("Seleccione un empleado.");
                return;
            }

            Empleado empleado = (Empleado)dvgEmpleados.CurrentRow.DataBoundItem;

            FrmNuevoEmpleado frm = new FrmNuevoEmpleado();

            frm.EmpleadoEditar = empleado;

            if (frm.ShowDialog() == DialogResult.OK)
            {
                CargarEmpleados();
            }
        }

        private void btnDesactivar_Click(object sender, EventArgs e)
        {
            if (dvgEmpleados.CurrentRow == null)
            {
                MessageBox.Show("Seleccione un empleado.");

                return;
            }

            Empleado empleado = (Empleado)dvgEmpleados.CurrentRow.DataBoundItem;

            using (var db = new NominaContext())
            {
                Empleado emp = db.Empleados.Find(empleado.Id)!;

                emp.Activo = !emp.Activo;

                db.SaveChanges();
            }

            CargarEmpleados();
        }

        private void btnActualizarSalario_Click(object sender, EventArgs e)
        {
            DialogResult r = MessageBox.Show(
        "Se buscarán checadas pendientes para todos los empleados registrados.\n\n¿Desea continuar?",
        "Actualizar historial",
        MessageBoxButtons.YesNo,
        MessageBoxIcon.Question);

            if (r != DialogResult.Yes)
                return;

            SincronizarChecadasPendientesService servicio =
                new SincronizarChecadasPendientesService();

            MessageBox.Show(
                servicio.Sincronizar(),
                "Proceso terminado",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void cmbFiltroEstado_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarEmpleados();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {

        }

        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            using (var db = new NominaContext())
            {
                string texto = txtBuscar.Text.ToLower();

                var consulta = db.Empleados.AsQueryable();

                switch (cmbFiltroEstado.Text)
                {
                    case "Activos":
                        consulta = consulta.Where(e => e.Activo);
                        break;

                    case "Inactivos":
                        consulta = consulta.Where(e => !e.Activo);
                        break;
                }

                consulta = consulta.Where(e =>
                    e.Nombre.ToLower().Contains(texto) ||
                    e.NumeroEmpleado.ToLower().Contains(texto) ||
                    e.Puesto.ToLower().Contains(texto) ||
                    e.Departamento.ToLower().Contains(texto));

                dvgEmpleados.DataSource = consulta.ToList();
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dvgEmpleados.CurrentRow == null)
            {
                MessageBox.Show("Seleccione un empleado.");
                return;
            }

            Empleado empleado = (Empleado)dvgEmpleados.CurrentRow.DataBoundItem;

            DialogResult respuesta = MessageBox.Show(
                $"¿Está seguro de eliminar a {empleado.Nombre}?\n\nEsta acción no se puede deshacer.",
                "Eliminar empleado",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (respuesta == DialogResult.No)
                return;

            using (var db = new NominaContext())
            {
                Empleado emp = db.Empleados.Find(empleado.Id)!;

                db.Empleados.Remove(emp);

                db.SaveChanges();
            }

            CargarEmpleados();

            MessageBox.Show("El empleado fue eliminado correctamente de la base de datos.","Empleado eliminado",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information);
        }
    }
}
