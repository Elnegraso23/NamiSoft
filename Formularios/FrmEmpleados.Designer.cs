namespace NominaPro.Formularios
{
    partial class FrmEmpleados
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            pnlbuscador = new Panel();
            cmbFiltroEstado = new ComboBox();
            btnNuevo = new Button();
            txtBuscar = new TextBox();
            LblBuscar = new Label();
            dvgEmpleados = new DataGridView();
            btnEditar = new Button();
            btnDesactivar = new Button();
            btnActualizarSalario = new Button();
            btnEliminar = new Button();
            panel1 = new Panel();
            pnlbuscador.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dvgEmpleados).BeginInit();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // pnlbuscador
            // 
            pnlbuscador.BackColor = Color.WhiteSmoke;
            pnlbuscador.Controls.Add(cmbFiltroEstado);
            pnlbuscador.Controls.Add(btnNuevo);
            pnlbuscador.Controls.Add(txtBuscar);
            pnlbuscador.Controls.Add(LblBuscar);
            pnlbuscador.Dock = DockStyle.Top;
            pnlbuscador.Location = new Point(0, 0);
            pnlbuscador.Name = "pnlbuscador";
            pnlbuscador.Size = new Size(898, 42);
            pnlbuscador.TabIndex = 1;
            // 
            // cmbFiltroEstado
            // 
            cmbFiltroEstado.FormattingEnabled = true;
            cmbFiltroEstado.Items.AddRange(new object[] { "Activos", "", "Inactivos", "", "Todos" });
            cmbFiltroEstado.Location = new Point(377, 11);
            cmbFiltroEstado.Name = "cmbFiltroEstado";
            cmbFiltroEstado.Size = new Size(121, 23);
            cmbFiltroEstado.TabIndex = 4;
            cmbFiltroEstado.SelectedIndexChanged += cmbFiltroEstado_SelectedIndexChanged;
            // 
            // btnNuevo
            // 
            btnNuevo.Location = new Point(504, 10);
            btnNuevo.Name = "btnNuevo";
            btnNuevo.Size = new Size(139, 23);
            btnNuevo.TabIndex = 3;
            btnNuevo.Text = "Nuevo Empleado";
            btnNuevo.UseVisualStyleBackColor = true;
            btnNuevo.Click += btnNuevo_Click;
            // 
            // txtBuscar
            // 
            txtBuscar.Location = new Point(77, 11);
            txtBuscar.Name = "txtBuscar";
            txtBuscar.Size = new Size(294, 23);
            txtBuscar.TabIndex = 2;
            txtBuscar.TextChanged += txtBuscar_TextChanged;
            // 
            // LblBuscar
            // 
            LblBuscar.AutoSize = true;
            LblBuscar.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            LblBuscar.Location = new Point(12, 9);
            LblBuscar.Name = "LblBuscar";
            LblBuscar.Size = new Size(59, 21);
            LblBuscar.TabIndex = 2;
            LblBuscar.Text = "Buscar:";
            // 
            // dvgEmpleados
            // 
            dvgEmpleados.AllowUserToAddRows = false;
            dvgEmpleados.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dvgEmpleados.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dvgEmpleados.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dvgEmpleados.Location = new Point(0, 42);
            dvgEmpleados.MultiSelect = false;
            dvgEmpleados.Name = "dvgEmpleados";
            dvgEmpleados.ReadOnly = true;
            dvgEmpleados.RowHeadersVisible = false;
            dvgEmpleados.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dvgEmpleados.Size = new Size(898, 576);
            dvgEmpleados.TabIndex = 2;
            // 
            // btnEditar
            // 
            btnEditar.ImageAlign = ContentAlignment.BottomCenter;
            btnEditar.Location = new Point(3, 9);
            btnEditar.Name = "btnEditar";
            btnEditar.Size = new Size(78, 23);
            btnEditar.TabIndex = 3;
            btnEditar.Text = "Editar";
            btnEditar.UseVisualStyleBackColor = true;
            btnEditar.Click += btnEditar_Click;
            // 
            // btnDesactivar
            // 
            btnDesactivar.ImageAlign = ContentAlignment.BottomCenter;
            btnDesactivar.Location = new Point(87, 9);
            btnDesactivar.Name = "btnDesactivar";
            btnDesactivar.Size = new Size(129, 23);
            btnDesactivar.TabIndex = 4;
            btnDesactivar.Text = "Activar / Desactivar";
            btnDesactivar.UseVisualStyleBackColor = true;
            btnDesactivar.Click += btnDesactivar_Click;
            // 
            // btnActualizarSalario
            // 
            btnActualizarSalario.ImageAlign = ContentAlignment.BottomCenter;
            btnActualizarSalario.Location = new Point(222, 9);
            btnActualizarSalario.Name = "btnActualizarSalario";
            btnActualizarSalario.Size = new Size(131, 23);
            btnActualizarSalario.TabIndex = 5;
            btnActualizarSalario.Text = "Actualizar historial";
            btnActualizarSalario.UseVisualStyleBackColor = true;
            btnActualizarSalario.Click += btnActualizarSalario_Click;
            // 
            // btnEliminar
            // 
            btnEliminar.ImageAlign = ContentAlignment.BottomCenter;
            btnEliminar.Location = new Point(420, 9);
            btnEliminar.Name = "btnEliminar";
            btnEliminar.Size = new Size(78, 23);
            btnEliminar.TabIndex = 6;
            btnEliminar.Text = "Eliminar";
            btnEliminar.UseVisualStyleBackColor = true;
            btnEliminar.Click += btnEliminar_Click;
            // 
            // panel1
            // 
            panel1.Controls.Add(btnEditar);
            panel1.Controls.Add(btnEliminar);
            panel1.Controls.Add(btnDesactivar);
            panel1.Controls.Add(btnActualizarSalario);
            panel1.Dock = DockStyle.Bottom;
            panel1.Location = new Point(0, 624);
            panel1.Name = "panel1";
            panel1.Size = new Size(898, 44);
            panel1.TabIndex = 7;
            // 
            // FrmEmpleados
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(898, 668);
            Controls.Add(panel1);
            Controls.Add(dvgEmpleados);
            Controls.Add(pnlbuscador);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FrmEmpleados";
            Text = "Empleados";
            Load += FrmEmpleados_Load;
            pnlbuscador.ResumeLayout(false);
            pnlbuscador.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dvgEmpleados).EndInit();
            panel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private Panel pnlbuscador;
        private Label LblBuscar;
        private TextBox txtBuscar;
        private Button btnNuevo;
        private DataGridView dvgEmpleados;
        private Button btnEditar;
        private Button btnDesactivar;
        private Button btnActualizarSalario;
        private ComboBox cmbFiltroEstado;
        private Button btnEliminar;
        private Panel panel1;
    }
}