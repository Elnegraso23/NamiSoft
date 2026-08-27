namespace NominaPro
{
    partial class FrmHorarioPersonalizado
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
            lbl = new Label();
            panel2 = new Panel();
            btnGuardar = new Button();
            dgvHorario = new DataGridView();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvHorario).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(lbl);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(384, 76);
            panel1.TabIndex = 0;
            // 
            // lbl
            // 
            lbl.AutoSize = true;
            lbl.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lbl.Location = new Point(82, 32);
            lbl.Name = "lbl";
            lbl.Size = new Size(218, 21);
            lbl.TabIndex = 0;
            lbl.Text = "HORARIO PERSONALIZADO";
            // 
            // panel2
            // 
            panel2.Controls.Add(btnGuardar);
            panel2.Dock = DockStyle.Bottom;
            panel2.Location = new Point(0, 297);
            panel2.Name = "panel2";
            panel2.Size = new Size(384, 34);
            panel2.TabIndex = 1;
            // 
            // btnGuardar
            // 
            btnGuardar.Location = new Point(12, 3);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(104, 23);
            btnGuardar.TabIndex = 0;
            btnGuardar.Text = "Guardar Horario";
            btnGuardar.UseVisualStyleBackColor = true;
            btnGuardar.Click += btnGuardar_Click;
            // 
            // dgvHorario
            // 
            dgvHorario.AllowUserToAddRows = false;
            dgvHorario.AllowUserToDeleteRows = false;
            dgvHorario.AllowUserToResizeRows = false;
            dgvHorario.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvHorario.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvHorario.Location = new Point(0, 82);
            dgvHorario.Name = "dgvHorario";
            dgvHorario.RowHeadersVisible = false;
            dgvHorario.SelectionMode = DataGridViewSelectionMode.CellSelect;
            dgvHorario.Size = new Size(384, 212);
            dgvHorario.TabIndex = 2;
            // 
            // FrmHorarioPersonalizado
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(384, 331);
            Controls.Add(dgvHorario);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Name = "FrmHorarioPersonalizado";
            Text = "FrmHorarioPersonalizado";
            Load += FrmHorarioPersonalizado_Load;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvHorario).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Label lbl;
        private Panel panel2;
        private Button btnGuardar;
        private DataGridView dgvHorario;
    }
}