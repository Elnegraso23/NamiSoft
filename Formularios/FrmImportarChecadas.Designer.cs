namespace NominaPro.Formularios
{
    partial class FrmImportarChecadas
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
            txtArchivo = new TextBox();
            LblArchivo = new Label();
            LblTituloChecadas = new Label();
            btnSeleccionar = new Button();
            dgvVistaPrevia = new DataGridView();
            btnImportar = new Button();
            lblEstado = new Label();
            LblEstadoChecadas = new Label();
            openFileDialog1 = new OpenFileDialog();
            panel1 = new Panel();
            panel2 = new Panel();
            panel3 = new Panel();
            ((System.ComponentModel.ISupportInitialize)dgvVistaPrevia).BeginInit();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            panel3.SuspendLayout();
            SuspendLayout();
            // 
            // txtArchivo
            // 
            txtArchivo.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtArchivo.Location = new Point(12, 60);
            txtArchivo.Name = "txtArchivo";
            txtArchivo.ReadOnly = true;
            txtArchivo.Size = new Size(816, 23);
            txtArchivo.TabIndex = 0;
            // 
            // LblArchivo
            // 
            LblArchivo.AutoSize = true;
            LblArchivo.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            LblArchivo.Location = new Point(12, 36);
            LblArchivo.Name = "LblArchivo";
            LblArchivo.Size = new Size(170, 21);
            LblArchivo.TabIndex = 1;
            LblArchivo.Text = "Archivo seleccionado:\n";
            LblArchivo.Click += label1_Click;
            // 
            // LblTituloChecadas
            // 
            LblTituloChecadas.AutoSize = true;
            LblTituloChecadas.Font = new Font("Segoe UI Black", 14.25F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            LblTituloChecadas.Location = new Point(12, 9);
            LblTituloChecadas.Name = "LblTituloChecadas";
            LblTituloChecadas.Size = new Size(225, 25);
            LblTituloChecadas.TabIndex = 2;
            LblTituloChecadas.Text = "IMPORTAR CHECADAS";
            LblTituloChecadas.TextAlign = ContentAlignment.TopCenter;
            // 
            // btnSeleccionar
            // 
            btnSeleccionar.Location = new Point(12, 89);
            btnSeleccionar.Name = "btnSeleccionar";
            btnSeleccionar.Size = new Size(131, 20);
            btnSeleccionar.TabIndex = 3;
            btnSeleccionar.Text = "Seleccionar Excel";
            btnSeleccionar.UseVisualStyleBackColor = true;
            btnSeleccionar.Click += btnSeleccionar_Click;
            // 
            // dgvVistaPrevia
            // 
            dgvVistaPrevia.AllowUserToAddRows = false;
            dgvVistaPrevia.AllowUserToDeleteRows = false;
            dgvVistaPrevia.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvVistaPrevia.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvVistaPrevia.Dock = DockStyle.Fill;
            dgvVistaPrevia.Location = new Point(0, 0);
            dgvVistaPrevia.Name = "dgvVistaPrevia";
            dgvVistaPrevia.ReadOnly = true;
            dgvVistaPrevia.RowHeadersVisible = false;
            dgvVistaPrevia.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvVistaPrevia.Size = new Size(816, 338);
            dgvVistaPrevia.TabIndex = 4;
            dgvVistaPrevia.CellContentClick += dataGridView1_CellContentClick;
            // 
            // btnImportar
            // 
            btnImportar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnImportar.Enabled = false;
            btnImportar.Location = new Point(12, 9);
            btnImportar.Name = "btnImportar";
            btnImportar.Size = new Size(131, 20);
            btnImportar.TabIndex = 5;
            btnImportar.Text = "Importar Checadas";
            btnImportar.UseVisualStyleBackColor = true;
            btnImportar.Click += btnImportar_Click;
            // 
            // lblEstado
            // 
            lblEstado.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            lblEstado.AutoSize = true;
            lblEstado.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblEstado.Location = new Point(66, 32);
            lblEstado.Name = "lblEstado";
            lblEstado.Size = new Size(156, 21);
            lblEstado.TabIndex = 6;
            lblEstado.Text = "Esperando archivo...";
            lblEstado.Click += lblEstado_Click;
            // 
            // LblEstadoChecadas
            // 
            LblEstadoChecadas.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            LblEstadoChecadas.AutoSize = true;
            LblEstadoChecadas.Font = new Font("Segoe UI Light", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            LblEstadoChecadas.Location = new Point(12, 32);
            LblEstadoChecadas.Name = "LblEstadoChecadas";
            LblEstadoChecadas.Size = new Size(59, 21);
            LblEstadoChecadas.TabIndex = 7;
            LblEstadoChecadas.Text = "Estado:";
            // 
            // openFileDialog1
            // 
            openFileDialog1.FileName = "openFileDialog1";
            // 
            // panel1
            // 
            panel1.Controls.Add(lblEstado);
            panel1.Controls.Add(LblEstadoChecadas);
            panel1.Controls.Add(btnImportar);
            panel1.Dock = DockStyle.Bottom;
            panel1.Location = new Point(0, 458);
            panel1.Name = "panel1";
            panel1.Size = new Size(816, 62);
            panel1.TabIndex = 8;
            panel1.Paint += panel1_Paint;
            // 
            // panel2
            // 
            panel2.Controls.Add(LblTituloChecadas);
            panel2.Controls.Add(LblArchivo);
            panel2.Controls.Add(txtArchivo);
            panel2.Controls.Add(btnSeleccionar);
            panel2.Dock = DockStyle.Top;
            panel2.Location = new Point(0, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(816, 120);
            panel2.TabIndex = 9;
            panel2.Paint += panel2_Paint;
            // 
            // panel3
            // 
            panel3.AutoSize = true;
            panel3.Controls.Add(dgvVistaPrevia);
            panel3.Dock = DockStyle.Fill;
            panel3.Location = new Point(0, 120);
            panel3.Name = "panel3";
            panel3.Size = new Size(816, 338);
            panel3.TabIndex = 10;
            // 
            // FrmImportarChecadas
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            ClientSize = new Size(816, 520);
            Controls.Add(panel3);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Name = "FrmImportarChecadas";
            Text = "FrmChecadas";
            ((System.ComponentModel.ISupportInitialize)dgvVistaPrevia).EndInit();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            panel3.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtArchivo;
        private Label LblArchivo;
        private Label LblTituloChecadas;
        private Button btnSeleccionar;
        private DataGridView dgvVistaPrevia;
        private Button btnImportar;
        private Label lblEstado;
        private Label LblEstadoChecadas;
        private OpenFileDialog openFileDialog1;
        private Panel panel1;
        private Panel panel2;
        private Panel panel3;
    }
}