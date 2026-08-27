namespace NominaPro.Formularios
{
    partial class FrmNomina
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
            panel1 = new Panel();
            lbltitulo = new Label();
            panel2 = new Panel();
            nudPropinas = new NumericUpDown();
            label1 = new Label();
            btnCalcularNomina = new Button();
            dtpSemana = new DateTimePicker();
            LblSemana = new Label();
            dvgNomina = new DataGridView();
            panel3 = new Panel();
            btnExportarExcel = new Button();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudPropinas).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dvgNomina).BeginInit();
            panel3.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = SystemColors.ControlLightLight;
            panel1.Controls.Add(lbltitulo);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(800, 38);
            panel1.TabIndex = 0;
            // 
            // lbltitulo
            // 
            lbltitulo.AutoSize = true;
            lbltitulo.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lbltitulo.Location = new Point(3, 9);
            lbltitulo.Name = "lbltitulo";
            lbltitulo.Size = new Size(206, 21);
            lbltitulo.TabIndex = 0;
            lbltitulo.Text = "Generar Nómina Semanal";
            // 
            // panel2
            // 
            panel2.BackColor = SystemColors.AppWorkspace;
            panel2.Controls.Add(nudPropinas);
            panel2.Controls.Add(label1);
            panel2.Controls.Add(btnCalcularNomina);
            panel2.Controls.Add(dtpSemana);
            panel2.Controls.Add(LblSemana);
            panel2.Dock = DockStyle.Top;
            panel2.Location = new Point(0, 38);
            panel2.Name = "panel2";
            panel2.Size = new Size(800, 38);
            panel2.TabIndex = 1;
            // 
            // nudPropinas
            // 
            nudPropinas.DecimalPlaces = 2;
            nudPropinas.Location = new Point(276, 9);
            nudPropinas.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
            nudPropinas.Name = "nudPropinas";
            nudPropinas.Size = new Size(120, 23);
            nudPropinas.TabIndex = 4;
            nudPropinas.TextAlign = HorizontalAlignment.Right;
            nudPropinas.ThousandsSeparator = true;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.Location = new Point(209, 10);
            label1.Name = "label1";
            label1.Size = new Size(66, 17);
            label1.TabIndex = 3;
            label1.Text = "Propinas :";
            // 
            // btnCalcularNomina
            // 
            btnCalcularNomina.Location = new Point(536, 8);
            btnCalcularNomina.Name = "btnCalcularNomina";
            btnCalcularNomina.Size = new Size(108, 23);
            btnCalcularNomina.TabIndex = 2;
            btnCalcularNomina.Text = "Calcular Nómina";
            btnCalcularNomina.UseVisualStyleBackColor = true;
            btnCalcularNomina.Click += btnCalcularNomina_Click;
            // 
            // dtpSemana
            // 
            dtpSemana.Format = DateTimePickerFormat.Short;
            dtpSemana.Location = new Point(70, 6);
            dtpSemana.Name = "dtpSemana";
            dtpSemana.Size = new Size(120, 23);
            dtpSemana.TabIndex = 2;
            // 
            // LblSemana
            // 
            LblSemana.AutoSize = true;
            LblSemana.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            LblSemana.Location = new Point(3, 9);
            LblSemana.Name = "LblSemana";
            LblSemana.Size = new Size(61, 17);
            LblSemana.TabIndex = 0;
            LblSemana.Text = "Semana :";
            // 
            // dvgNomina
            // 
            dvgNomina.AllowUserToDeleteRows = false;
            dvgNomina.AllowUserToResizeRows = false;
            dvgNomina.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dvgNomina.BackgroundColor = SystemColors.ControlLightLight;
            dvgNomina.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dvgNomina.Dock = DockStyle.Fill;
            dvgNomina.Location = new Point(0, 76);
            dvgNomina.MultiSelect = false;
            dvgNomina.Name = "dvgNomina";
            dvgNomina.ReadOnly = true;
            dvgNomina.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dvgNomina.Size = new Size(800, 374);
            dvgNomina.TabIndex = 2;
            // 
            // panel3
            // 
            panel3.BackColor = SystemColors.ControlLightLight;
            panel3.Controls.Add(btnExportarExcel);
            panel3.Dock = DockStyle.Bottom;
            panel3.Location = new Point(0, 416);
            panel3.Name = "panel3";
            panel3.Size = new Size(800, 34);
            panel3.TabIndex = 3;
            // 
            // btnExportarExcel
            // 
            btnExportarExcel.Enabled = false;
            btnExportarExcel.Location = new Point(3, 8);
            btnExportarExcel.Name = "btnExportarExcel";
            btnExportarExcel.Size = new Size(100, 23);
            btnExportarExcel.TabIndex = 4;
            btnExportarExcel.Text = "Exportar Excel";
            btnExportarExcel.UseVisualStyleBackColor = true;
            btnExportarExcel.Click += btnExportarExcel_Click;
            // 
            // FrmNomina
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(panel3);
            Controls.Add(dvgNomina);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Name = "FrmNomina";
            Text = "Generar Nomina";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudPropinas).EndInit();
            ((System.ComponentModel.ISupportInitialize)dvgNomina).EndInit();
            panel3.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Label lbltitulo;
        private Panel panel2;
        private Label LblSemana;
        private DateTimePicker dtpSemana;
        private Button btnCalcularNomina;
        private DataGridView dvgNomina;
        private Panel panel3;
        private Button btnExportarExcel;
        private NumericUpDown nudPropinas;
        private Label label1;
    }
}