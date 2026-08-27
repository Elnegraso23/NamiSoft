namespace NominaPro.Formularios
{
    partial class FrmNuevoEmpleado
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
            DckSuperior = new Panel();
            LblTituloNuevoEmpleado = new Label();
            LblNumeroEmpleado = new Label();
            LblNombreEmpleado = new Label();
            LblPuesto = new Label();
            Lbl = new Label();
            LblSalarioHora = new Label();
            txtNumero = new TextBox();
            txtNombre = new TextBox();
            txtPuesto = new TextBox();
            txtDepartamento = new TextBox();
            txtSalario = new TextBox();
            LblTurno = new Label();
            LblDescanso = new Label();
            LblEstado = new Label();
            cmbTurno = new ComboBox();
            cmbDescanso = new ComboBox();
            checkEstado = new CheckBox();
            btnCancelar = new Button();
            btnGuardar = new Button();
            btnHorarioPersonalizado = new Button();
            DckSuperior.SuspendLayout();
            SuspendLayout();
            // 
            // DckSuperior
            // 
            DckSuperior.BackColor = SystemColors.ActiveCaption;
            DckSuperior.Controls.Add(LblTituloNuevoEmpleado);
            DckSuperior.Dock = DockStyle.Top;
            DckSuperior.Location = new Point(0, 0);
            DckSuperior.Name = "DckSuperior";
            DckSuperior.Size = new Size(684, 38);
            DckSuperior.TabIndex = 0;
            // 
            // LblTituloNuevoEmpleado
            // 
            LblTituloNuevoEmpleado.AutoSize = true;
            LblTituloNuevoEmpleado.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            LblTituloNuevoEmpleado.Location = new Point(229, 9);
            LblTituloNuevoEmpleado.Name = "LblTituloNuevoEmpleado";
            LblTituloNuevoEmpleado.Size = new Size(184, 25);
            LblTituloNuevoEmpleado.TabIndex = 0;
            LblTituloNuevoEmpleado.Text = "NUEVO EMPLEADO";
            // 
            // LblNumeroEmpleado
            // 
            LblNumeroEmpleado.AutoSize = true;
            LblNumeroEmpleado.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            LblNumeroEmpleado.Location = new Point(12, 65);
            LblNumeroEmpleado.Name = "LblNumeroEmpleado";
            LblNumeroEmpleado.Size = new Size(144, 17);
            LblNumeroEmpleado.TabIndex = 1;
            LblNumeroEmpleado.Text = "Número de empleado:";
            // 
            // LblNombreEmpleado
            // 
            LblNombreEmpleado.AutoSize = true;
            LblNombreEmpleado.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            LblNombreEmpleado.Location = new Point(12, 102);
            LblNombreEmpleado.Name = "LblNombreEmpleado";
            LblNombreEmpleado.Size = new Size(122, 17);
            LblNombreEmpleado.TabIndex = 2;
            LblNombreEmpleado.Text = "Nombre completo:";
            // 
            // LblPuesto
            // 
            LblPuesto.AutoSize = true;
            LblPuesto.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            LblPuesto.Location = new Point(12, 145);
            LblPuesto.Name = "LblPuesto";
            LblPuesto.Size = new Size(53, 17);
            LblPuesto.TabIndex = 3;
            LblPuesto.Text = "Puesto:";
            // 
            // Lbl
            // 
            Lbl.AutoSize = true;
            Lbl.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            Lbl.Location = new Point(12, 185);
            Lbl.Name = "Lbl";
            Lbl.Size = new Size(99, 17);
            Lbl.TabIndex = 4;
            Lbl.Text = "Departamento:";
            // 
            // LblSalarioHora
            // 
            LblSalarioHora.AutoSize = true;
            LblSalarioHora.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            LblSalarioHora.Location = new Point(12, 225);
            LblSalarioHora.Name = "LblSalarioHora";
            LblSalarioHora.Size = new Size(108, 17);
            LblSalarioHora.TabIndex = 5;
            LblSalarioHora.Text = "Salario por hora:";
            // 
            // txtNumero
            // 
            txtNumero.Location = new Point(229, 64);
            txtNumero.Name = "txtNumero";
            txtNumero.Size = new Size(84, 23);
            txtNumero.TabIndex = 6;
            txtNumero.TextChanged += txtNumero_TextChanged;
            // 
            // txtNombre
            // 
            txtNombre.Location = new Point(229, 102);
            txtNombre.Name = "txtNombre";
            txtNombre.Size = new Size(235, 23);
            txtNombre.TabIndex = 7;
            // 
            // txtPuesto
            // 
            txtPuesto.Location = new Point(229, 144);
            txtPuesto.Name = "txtPuesto";
            txtPuesto.Size = new Size(184, 23);
            txtPuesto.TabIndex = 8;
            // 
            // txtDepartamento
            // 
            txtDepartamento.Location = new Point(229, 184);
            txtDepartamento.Name = "txtDepartamento";
            txtDepartamento.Size = new Size(184, 23);
            txtDepartamento.TabIndex = 9;
            // 
            // txtSalario
            // 
            txtSalario.Location = new Point(229, 224);
            txtSalario.Name = "txtSalario";
            txtSalario.Size = new Size(84, 23);
            txtSalario.TabIndex = 10;
            // 
            // LblTurno
            // 
            LblTurno.AutoSize = true;
            LblTurno.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            LblTurno.Location = new Point(12, 264);
            LblTurno.Name = "LblTurno";
            LblTurno.Size = new Size(94, 17);
            LblTurno.TabIndex = 11;
            LblTurno.Text = "Tipo de turno:";
            // 
            // LblDescanso
            // 
            LblDescanso.AutoSize = true;
            LblDescanso.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            LblDescanso.Location = new Point(12, 301);
            LblDescanso.Name = "LblDescanso";
            LblDescanso.Size = new Size(68, 17);
            LblDescanso.TabIndex = 12;
            LblDescanso.Text = "Descanso:";
            // 
            // LblEstado
            // 
            LblEstado.AutoSize = true;
            LblEstado.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            LblEstado.Location = new Point(12, 338);
            LblEstado.Name = "LblEstado";
            LblEstado.Size = new Size(52, 17);
            LblEstado.TabIndex = 13;
            LblEstado.Text = "Estado:";
            // 
            // cmbTurno
            // 
            cmbTurno.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbTurno.FormattingEnabled = true;
            cmbTurno.Items.AddRange(new object[] { "Mañana", "", "Tarde", "", "Personalizado", "", "Especial" });
            cmbTurno.Location = new Point(229, 264);
            cmbTurno.Name = "cmbTurno";
            cmbTurno.Size = new Size(121, 23);
            cmbTurno.TabIndex = 14;
            cmbTurno.SelectedIndexChanged += cmbTurno_SelectedIndexChanged;
            // 
            // cmbDescanso
            // 
            cmbDescanso.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbDescanso.FormattingEnabled = true;
            cmbDescanso.Items.AddRange(new object[] { "Lunes", "", "Martes", "", "Miércoles", "", "Jueves", "", "Viernes", "", "Sábado", "Domingo" });
            cmbDescanso.Location = new Point(229, 301);
            cmbDescanso.Name = "cmbDescanso";
            cmbDescanso.Size = new Size(121, 23);
            cmbDescanso.TabIndex = 15;
            // 
            // checkEstado
            // 
            checkEstado.AutoSize = true;
            checkEstado.Location = new Point(229, 338);
            checkEstado.Name = "checkEstado";
            checkEstado.Size = new Size(98, 19);
            checkEstado.TabIndex = 16;
            checkEstado.Text = "Estado Activo";
            checkEstado.UseVisualStyleBackColor = true;
            checkEstado.CheckedChanged += checkBox1_CheckedChanged;
            // 
            // btnCancelar
            // 
            btnCancelar.Location = new Point(389, 398);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(75, 23);
            btnCancelar.TabIndex = 17;
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = true;
            btnCancelar.Click += btnCancelar_Click;
            // 
            // btnGuardar
            // 
            btnGuardar.Location = new Point(292, 398);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(75, 23);
            btnGuardar.TabIndex = 18;
            btnGuardar.Text = "Guardar";
            btnGuardar.UseVisualStyleBackColor = true;
            btnGuardar.Click += btnGuardar_Click;
            // 
            // btnHorarioPersonalizado
            // 
            btnHorarioPersonalizado.Location = new Point(389, 264);
            btnHorarioPersonalizado.Name = "btnHorarioPersonalizado";
            btnHorarioPersonalizado.Size = new Size(117, 23);
            btnHorarioPersonalizado.TabIndex = 19;
            btnHorarioPersonalizado.Text = "Configurar Horario";
            btnHorarioPersonalizado.UseVisualStyleBackColor = true;
            btnHorarioPersonalizado.Click += btnHorarioPersonalizado_Click;
            // 
            // FrmNuevoEmpleado
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(684, 461);
            Controls.Add(btnHorarioPersonalizado);
            Controls.Add(btnGuardar);
            Controls.Add(btnCancelar);
            Controls.Add(checkEstado);
            Controls.Add(cmbDescanso);
            Controls.Add(cmbTurno);
            Controls.Add(LblEstado);
            Controls.Add(LblDescanso);
            Controls.Add(LblTurno);
            Controls.Add(txtSalario);
            Controls.Add(txtDepartamento);
            Controls.Add(txtPuesto);
            Controls.Add(txtNombre);
            Controls.Add(txtNumero);
            Controls.Add(LblSalarioHora);
            Controls.Add(Lbl);
            Controls.Add(LblPuesto);
            Controls.Add(LblNombreEmpleado);
            Controls.Add(LblNumeroEmpleado);
            Controls.Add(DckSuperior);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmNuevoEmpleado";
            StartPosition = FormStartPosition.CenterParent;
            Text = "v";
            Load += FrmNuevoEmpleado_Load;
            DckSuperior.ResumeLayout(false);
            DckSuperior.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel DckSuperior;
        private Label LblTituloNuevoEmpleado;
        private Label LblNumeroEmpleado;
        private Label LblNombreEmpleado;
        private Label LblPuesto;
        private Label Lbl;
        private Label LblSalarioHora;
        private TextBox txtNumero;
        private TextBox txtNombre;
        private TextBox txtPuesto;
        private TextBox txtDepartamento;
        private TextBox txtSalario;
        private Label LblTurno;
        private Label LblDescanso;
        private Label LblEstado;
        private ComboBox cmbTurno;
        private ComboBox cmbDescanso;
        private CheckBox checkEstado;
        private Button btnCancelar;
        private Button btnGuardar;
        private Button btnHorarioPersonalizado;
    }
}