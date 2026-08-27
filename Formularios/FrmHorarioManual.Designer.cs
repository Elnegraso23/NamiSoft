namespace NominaPro.Formularios
{
    partial class FrmHorarioManual
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
            panelarriba = new Panel();
            LblTit = new Label();
            panel1 = new Panel();
            BtnBuscar = new Button();
            dtpSemana = new DateTimePicker();
            LblSemana = new Label();
            cmbEmpleado = new ComboBox();
            LblEmpleado = new Label();
            dvgHorarioManual = new DataGridView();
            btnGuardarCambios = new Button();
            panel2 = new Panel();
            btnCancelar = new Button();
            sqliteCommand1 = new Microsoft.Data.Sqlite.SqliteCommand();
            btnActualizarRetardos = new Button();
            panelarriba.SuspendLayout();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dvgHorarioManual).BeginInit();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // panelarriba
            // 
            panelarriba.BackColor = SystemColors.InactiveBorder;
            panelarriba.Controls.Add(LblTit);
            panelarriba.Dock = DockStyle.Top;
            panelarriba.Location = new Point(0, 0);
            panelarriba.Name = "panelarriba";
            panelarriba.Size = new Size(1595, 34);
            panelarriba.TabIndex = 0;
            // 
            // LblTit
            // 
            LblTit.AutoSize = true;
            LblTit.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            LblTit.Location = new Point(3, 9);
            LblTit.Name = "LblTit";
            LblTit.Size = new Size(156, 21);
            LblTit.TabIndex = 2;
            LblTit.Text = "HORARIO MANUAL";
            LblTit.Click += LblTit_Click;
            // 
            // panel1
            // 
            panel1.BackColor = SystemColors.ScrollBar;
            panel1.Controls.Add(BtnBuscar);
            panel1.Controls.Add(dtpSemana);
            panel1.Controls.Add(LblSemana);
            panel1.Controls.Add(cmbEmpleado);
            panel1.Controls.Add(LblEmpleado);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 34);
            panel1.Name = "panel1";
            panel1.Size = new Size(1595, 34);
            panel1.TabIndex = 1;
            // 
            // BtnBuscar
            // 
            BtnBuscar.ForeColor = SystemColors.ControlText;
            BtnBuscar.Location = new Point(578, 3);
            BtnBuscar.Name = "BtnBuscar";
            BtnBuscar.Size = new Size(75, 23);
            BtnBuscar.TabIndex = 2;
            BtnBuscar.Text = "Buscar";
            BtnBuscar.UseVisualStyleBackColor = true;
            BtnBuscar.Click += BtnBuscar_Click;
            // 
            // dtpSemana
            // 
            dtpSemana.Location = new Point(372, 4);
            dtpSemana.Name = "dtpSemana";
            dtpSemana.Size = new Size(200, 23);
            dtpSemana.TabIndex = 2;
            // 
            // LblSemana
            // 
            LblSemana.AutoSize = true;
            LblSemana.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            LblSemana.ForeColor = Color.Red;
            LblSemana.Location = new Point(305, 4);
            LblSemana.Name = "LblSemana";
            LblSemana.Size = new Size(61, 17);
            LblSemana.TabIndex = 3;
            LblSemana.Text = "Semana :";
            LblSemana.Click += LblSemana_Click;
            // 
            // cmbEmpleado
            // 
            cmbEmpleado.FormattingEnabled = true;
            cmbEmpleado.Location = new Point(92, 3);
            cmbEmpleado.Name = "cmbEmpleado";
            cmbEmpleado.Size = new Size(81, 23);
            cmbEmpleado.TabIndex = 2;
            // 
            // LblEmpleado
            // 
            LblEmpleado.AutoSize = true;
            LblEmpleado.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            LblEmpleado.ForeColor = Color.Red;
            LblEmpleado.Location = new Point(12, 4);
            LblEmpleado.Name = "LblEmpleado";
            LblEmpleado.Size = new Size(74, 17);
            LblEmpleado.TabIndex = 2;
            LblEmpleado.Text = "Empleado :";
            // 
            // dvgHorarioManual
            // 
            dvgHorarioManual.AllowUserToAddRows = false;
            dvgHorarioManual.AllowUserToDeleteRows = false;
            dvgHorarioManual.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dvgHorarioManual.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dvgHorarioManual.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dvgHorarioManual.Location = new Point(3, 74);
            dvgHorarioManual.MultiSelect = false;
            dvgHorarioManual.Name = "dvgHorarioManual";
            dvgHorarioManual.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dvgHorarioManual.Size = new Size(1589, 483);
            dvgHorarioManual.TabIndex = 2;
            dvgHorarioManual.CellContentClick += dvgHorarioManual_CellContentClick;
            dvgHorarioManual.CellValidating += dvgHorarioManual_CellValidating;
            // 
            // btnGuardarCambios
            // 
            btnGuardarCambios.ForeColor = SystemColors.ControlText;
            btnGuardarCambios.Location = new Point(3, 3);
            btnGuardarCambios.Name = "btnGuardarCambios";
            btnGuardarCambios.Size = new Size(156, 23);
            btnGuardarCambios.TabIndex = 3;
            btnGuardarCambios.Text = "Guardar Cambios";
            btnGuardarCambios.UseVisualStyleBackColor = true;
            btnGuardarCambios.Click += btnGuardarCambios_Click;
            // 
            // panel2
            // 
            panel2.BackColor = SystemColors.InactiveBorder;
            panel2.Controls.Add(btnActualizarRetardos);
            panel2.Controls.Add(btnCancelar);
            panel2.Controls.Add(btnGuardarCambios);
            panel2.Dock = DockStyle.Bottom;
            panel2.Location = new Point(0, 563);
            panel2.Name = "panel2";
            panel2.Size = new Size(1595, 29);
            panel2.TabIndex = 4;
            // 
            // btnCancelar
            // 
            btnCancelar.ForeColor = SystemColors.ControlText;
            btnCancelar.Location = new Point(327, 3);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(97, 23);
            btnCancelar.TabIndex = 5;
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = true;
            // 
            // sqliteCommand1
            // 
            sqliteCommand1.CommandTimeout = 30;
            sqliteCommand1.Connection = null;
            sqliteCommand1.Transaction = null;
            sqliteCommand1.UpdatedRowSource = System.Data.UpdateRowSource.None;
            // 
            // btnActualizarRetardos
            // 
            btnActualizarRetardos.ForeColor = SystemColors.ControlText;
            btnActualizarRetardos.Location = new Point(165, 3);
            btnActualizarRetardos.Name = "btnActualizarRetardos";
            btnActualizarRetardos.Size = new Size(156, 23);
            btnActualizarRetardos.TabIndex = 6;
            btnActualizarRetardos.Text = "Actualizar Retardos";
            btnActualizarRetardos.UseVisualStyleBackColor = true;
            btnActualizarRetardos.Click += btnActualizarRetardos_Click;
            // 
            // FrmHorarioManual
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1595, 592);
            Controls.Add(panel2);
            Controls.Add(dvgHorarioManual);
            Controls.Add(panel1);
            Controls.Add(panelarriba);
            Name = "FrmHorarioManual";
            Text = "FrmHorarioManual";
            Load += FrmHorarioManual_Load;
            panelarriba.ResumeLayout(false);
            panelarriba.PerformLayout();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dvgHorarioManual).EndInit();
            panel2.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel panelarriba;
        private Label LblTit;
        private Panel panel1;
        private Label LblEmpleado;
        private DateTimePicker dtpSemana;
        private Label LblSemana;
        private ComboBox cmbEmpleado;
        private Button BtnBuscar;
        private DataGridView dvgHorarioManual;
        private Button btnGuardarCambios;
        private Panel panel2;
        private Button btnCancelar;
        private Microsoft.Data.Sqlite.SqliteCommand sqliteCommand1;
        private Button btnActualizarRetardos;
    }
}