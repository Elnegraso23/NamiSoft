namespace NominaPro.Formularios
{
    partial class FrmConfiguracion
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
            pnlsuperior = new Panel();
            lblconfi = new Label();
            lblTurnomañana = new Label();
            lblentradamañana = new Label();
            label1 = new Label();
            label2 = new Label();
            panel1 = new Panel();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            label6 = new Label();
            label7 = new Label();
            label8 = new Label();
            label9 = new Label();
            label10 = new Label();
            label11 = new Label();
            label12 = new Label();
            btnGuardarConfiguracion = new Button();
            dtpEntradaManana = new DateTimePicker();
            dtpSalidaMañana = new DateTimePicker();
            nudToleranciaManana = new NumericUpDown();
            nudToleranciaTarde = new NumericUpDown();
            dtpSalidaTarde = new DateTimePicker();
            dtpEntradaTarde = new DateTimePicker();
            nudCadaRetardos = new NumericUpDown();
            nudDescuentoRetardos = new NumericUpDown();
            chkPrimaDominical = new CheckBox();
            nudPrimaDominical = new NumericUpDown();
            pnlsuperior.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudToleranciaManana).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudToleranciaTarde).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudCadaRetardos).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudDescuentoRetardos).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudPrimaDominical).BeginInit();
            SuspendLayout();
            // 
            // pnlsuperior
            // 
            pnlsuperior.BackColor = Color.Silver;
            pnlsuperior.Controls.Add(lblconfi);
            pnlsuperior.Dock = DockStyle.Top;
            pnlsuperior.Location = new Point(0, 0);
            pnlsuperior.Name = "pnlsuperior";
            pnlsuperior.Size = new Size(800, 38);
            pnlsuperior.TabIndex = 0;
            // 
            // lblconfi
            // 
            lblconfi.AutoSize = true;
            lblconfi.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblconfi.Location = new Point(3, 9);
            lblconfi.Name = "lblconfi";
            lblconfi.Size = new Size(251, 21);
            lblconfi.TabIndex = 1;
            lblconfi.Text = "CONFIGURACIÓN DE CÁLCULOS";
            // 
            // lblTurnomañana
            // 
            lblTurnomañana.AutoSize = true;
            lblTurnomañana.BackColor = SystemColors.ActiveCaption;
            lblTurnomañana.Font = new Font("Segoe UI", 9.75F, FontStyle.Italic, GraphicsUnit.Point, 0);
            lblTurnomañana.Location = new Point(12, 52);
            lblTurnomañana.Name = "lblTurnomañana";
            lblTurnomañana.Size = new Size(111, 17);
            lblTurnomañana.TabIndex = 2;
            lblTurnomañana.Text = "TURNO MAÑANA";
            lblTurnomañana.Click += lblTurnomañana_Click;
            // 
            // lblentradamañana
            // 
            lblentradamañana.AutoSize = true;
            lblentradamañana.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblentradamañana.Location = new Point(12, 80);
            lblentradamañana.Name = "lblentradamañana";
            lblentradamañana.Size = new Size(86, 17);
            lblentradamañana.TabIndex = 3;
            lblentradamañana.Text = "Hora Entrada";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.Location = new Point(12, 109);
            label1.Name = "label1";
            label1.Size = new Size(76, 17);
            label1.TabIndex = 4;
            label1.Text = "Hora Salida";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.Location = new Point(12, 140);
            label2.Name = "label2";
            label2.Size = new Size(67, 17);
            label2.TabIndex = 6;
            label2.Text = "Tolerancia";
            // 
            // panel1
            // 
            panel1.BackColor = Color.Silver;
            panel1.Dock = DockStyle.Bottom;
            panel1.Location = new Point(0, 425);
            panel1.Name = "panel1";
            panel1.Size = new Size(800, 28);
            panel1.TabIndex = 7;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.BackColor = SystemColors.ActiveCaption;
            label3.Font = new Font("Segoe UI", 9.75F, FontStyle.Italic, GraphicsUnit.Point, 0);
            label3.Location = new Point(12, 171);
            label3.Name = "label3";
            label3.Size = new Size(94, 17);
            label3.TabIndex = 9;
            label3.Text = "TURNO TARDE";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label4.Location = new Point(12, 258);
            label4.Name = "label4";
            label4.Size = new Size(67, 17);
            label4.TabIndex = 13;
            label4.Text = "Tolerancia";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label5.Location = new Point(12, 229);
            label5.Name = "label5";
            label5.Size = new Size(76, 17);
            label5.TabIndex = 11;
            label5.Text = "Hora Salida";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label6.Location = new Point(12, 201);
            label6.Name = "label6";
            label6.Size = new Size(86, 17);
            label6.TabIndex = 10;
            label6.Text = "Hora Entrada";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.BackColor = SystemColors.ActiveCaption;
            label7.Font = new Font("Segoe UI", 9.75F, FontStyle.Italic, GraphicsUnit.Point, 0);
            label7.Location = new Point(12, 289);
            label7.Name = "label7";
            label7.Size = new Size(70, 17);
            label7.TabIndex = 14;
            label7.Text = "RETARDOS";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label8.Location = new Point(12, 345);
            label8.Name = "label8";
            label8.Size = new Size(67, 17);
            label8.TabIndex = 16;
            label8.Text = "Descontar";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label9.Location = new Point(12, 319);
            label9.Name = "label9";
            label9.Size = new Size(38, 17);
            label9.TabIndex = 15;
            label9.Text = "Cada";
            label9.Click += label9_Click;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.BackColor = SystemColors.ActiveCaption;
            label10.Font = new Font("Segoe UI", 9.75F, FontStyle.Italic, GraphicsUnit.Point, 0);
            label10.Location = new Point(291, 52);
            label10.Name = "label10";
            label10.Size = new Size(117, 17);
            label10.TabIndex = 17;
            label10.Text = "PRIMA DOMINICAL";
            label10.Click += label10_Click;
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label11.Location = new Point(291, 109);
            label11.Name = "label11";
            label11.Size = new Size(69, 17);
            label11.TabIndex = 19;
            label11.Text = "Porcentaje";
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label12.Location = new Point(291, 80);
            label12.Name = "label12";
            label12.Size = new Size(85, 17);
            label12.TabIndex = 18;
            label12.Text = "Aplicar Prima";
            // 
            // btnGuardarConfiguracion
            // 
            btnGuardarConfiguracion.BackColor = SystemColors.ActiveBorder;
            btnGuardarConfiguracion.Location = new Point(291, 343);
            btnGuardarConfiguracion.Name = "btnGuardarConfiguracion";
            btnGuardarConfiguracion.Size = new Size(139, 23);
            btnGuardarConfiguracion.TabIndex = 20;
            btnGuardarConfiguracion.Text = "Guardar Configuracion";
            btnGuardarConfiguracion.UseVisualStyleBackColor = false;
            btnGuardarConfiguracion.Click += btnGuardarConfiguracion_Click;
            // 
            // dtpEntradaManana
            // 
            dtpEntradaManana.Format = DateTimePickerFormat.Time;
            dtpEntradaManana.Location = new Point(129, 80);
            dtpEntradaManana.Name = "dtpEntradaManana";
            dtpEntradaManana.ShowUpDown = true;
            dtpEntradaManana.Size = new Size(95, 23);
            dtpEntradaManana.TabIndex = 21;
            // 
            // dtpSalidaMañana
            // 
            dtpSalidaMañana.Format = DateTimePickerFormat.Time;
            dtpSalidaMañana.Location = new Point(129, 109);
            dtpSalidaMañana.Name = "dtpSalidaMañana";
            dtpSalidaMañana.ShowUpDown = true;
            dtpSalidaMañana.Size = new Size(95, 23);
            dtpSalidaMañana.TabIndex = 22;
            // 
            // nudToleranciaManana
            // 
            nudToleranciaManana.Location = new Point(129, 138);
            nudToleranciaManana.Maximum = new decimal(new int[] { 60, 0, 0, 0 });
            nudToleranciaManana.Name = "nudToleranciaManana";
            nudToleranciaManana.Size = new Size(35, 23);
            nudToleranciaManana.TabIndex = 23;
            nudToleranciaManana.Value = new decimal(new int[] { 10, 0, 0, 0 });
            // 
            // nudToleranciaTarde
            // 
            nudToleranciaTarde.Location = new Point(129, 259);
            nudToleranciaTarde.Maximum = new decimal(new int[] { 60, 0, 0, 0 });
            nudToleranciaTarde.Name = "nudToleranciaTarde";
            nudToleranciaTarde.Size = new Size(35, 23);
            nudToleranciaTarde.TabIndex = 26;
            nudToleranciaTarde.Value = new decimal(new int[] { 10, 0, 0, 0 });
            // 
            // dtpSalidaTarde
            // 
            dtpSalidaTarde.Format = DateTimePickerFormat.Time;
            dtpSalidaTarde.Location = new Point(129, 230);
            dtpSalidaTarde.Name = "dtpSalidaTarde";
            dtpSalidaTarde.ShowUpDown = true;
            dtpSalidaTarde.Size = new Size(95, 23);
            dtpSalidaTarde.TabIndex = 25;
            // 
            // dtpEntradaTarde
            // 
            dtpEntradaTarde.Format = DateTimePickerFormat.Time;
            dtpEntradaTarde.Location = new Point(129, 201);
            dtpEntradaTarde.Name = "dtpEntradaTarde";
            dtpEntradaTarde.ShowUpDown = true;
            dtpEntradaTarde.Size = new Size(95, 23);
            dtpEntradaTarde.TabIndex = 24;
            // 
            // nudCadaRetardos
            // 
            nudCadaRetardos.Location = new Point(129, 313);
            nudCadaRetardos.Maximum = new decimal(new int[] { 10, 0, 0, 0 });
            nudCadaRetardos.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            nudCadaRetardos.Name = "nudCadaRetardos";
            nudCadaRetardos.Size = new Size(35, 23);
            nudCadaRetardos.TabIndex = 27;
            nudCadaRetardos.Value = new decimal(new int[] { 3, 0, 0, 0 });
            // 
            // nudDescuentoRetardos
            // 
            nudDescuentoRetardos.Increment = new decimal(new int[] { 30, 0, 0, 0 });
            nudDescuentoRetardos.Location = new Point(129, 345);
            nudDescuentoRetardos.Maximum = new decimal(new int[] { 240, 0, 0, 0 });
            nudDescuentoRetardos.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            nudDescuentoRetardos.Name = "nudDescuentoRetardos";
            nudDescuentoRetardos.Size = new Size(35, 23);
            nudDescuentoRetardos.TabIndex = 28;
            nudDescuentoRetardos.Value = new decimal(new int[] { 30, 0, 0, 0 });
            // 
            // chkPrimaDominical
            // 
            chkPrimaDominical.AutoSize = true;
            chkPrimaDominical.Location = new Point(412, 78);
            chkPrimaDominical.Name = "chkPrimaDominical";
            chkPrimaDominical.Size = new Size(15, 14);
            chkPrimaDominical.TabIndex = 29;
            chkPrimaDominical.UseVisualStyleBackColor = true;
            // 
            // nudPrimaDominical
            // 
            nudPrimaDominical.DecimalPlaces = 2;
            nudPrimaDominical.Location = new Point(412, 109);
            nudPrimaDominical.Name = "nudPrimaDominical";
            nudPrimaDominical.Size = new Size(49, 23);
            nudPrimaDominical.TabIndex = 30;
            nudPrimaDominical.Value = new decimal(new int[] { 25, 0, 0, 0 });
            // 
            // FrmConfiguracion
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 453);
            Controls.Add(nudPrimaDominical);
            Controls.Add(chkPrimaDominical);
            Controls.Add(nudDescuentoRetardos);
            Controls.Add(nudCadaRetardos);
            Controls.Add(nudToleranciaTarde);
            Controls.Add(dtpSalidaTarde);
            Controls.Add(dtpEntradaTarde);
            Controls.Add(nudToleranciaManana);
            Controls.Add(dtpSalidaMañana);
            Controls.Add(dtpEntradaManana);
            Controls.Add(btnGuardarConfiguracion);
            Controls.Add(label11);
            Controls.Add(label12);
            Controls.Add(label10);
            Controls.Add(label8);
            Controls.Add(label9);
            Controls.Add(label7);
            Controls.Add(label4);
            Controls.Add(label5);
            Controls.Add(label6);
            Controls.Add(label3);
            Controls.Add(panel1);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(lblentradamañana);
            Controls.Add(lblTurnomañana);
            Controls.Add(pnlsuperior);
            Name = "FrmConfiguracion";
            Text = "FrmConfiguracion";
            Load += FrmConfiguracion_Load;
            pnlsuperior.ResumeLayout(false);
            pnlsuperior.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudToleranciaManana).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudToleranciaTarde).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudCadaRetardos).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudDescuentoRetardos).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudPrimaDominical).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel pnlsuperior;
        private Label lblconfi;
        private Label lblTurnomañana;
        private Label lblentradamañana;
        private Label label1;
        private Label label2;
        private Panel panel1;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label label8;
        private Label label9;
        private Label label10;
        private Label label11;
        private Label label12;
        private Button btnGuardarConfiguracion;
        private DateTimePicker dtpEntradaManana;
        private DateTimePicker dtpSalidaMañana;
        private NumericUpDown nudToleranciaManana;
        private NumericUpDown nudToleranciaTarde;
        private DateTimePicker dtpSalidaTarde;
        private DateTimePicker dtpEntradaTarde;
        private NumericUpDown nudCadaRetardos;
        private NumericUpDown nudDescuentoRetardos;
        private CheckBox chkPrimaDominical;
        private NumericUpDown nudPrimaDominical;
    }
}