namespace NominaPro
{
    partial class FrmPrincipal
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmPrincipal));
            panelMenu = new Panel();
            ibtnNomina = new FontAwesome.Sharp.IconButton();
            ibtnConfiguracion = new FontAwesome.Sharp.IconButton();
            ibtnHorario = new FontAwesome.Sharp.IconButton();
            ibtnEmpleados = new FontAwesome.Sharp.IconButton();
            ibtnChecadas = new FontAwesome.Sharp.IconButton();
            LblFecha = new Label();
            piclogo = new PictureBox();
            IblEmpresa = new Label();
            ibtnSalir = new Button();
            panel1 = new Panel();
            lblEstadoTexto = new Label();
            lblEstado = new Label();
            btnChecadas = new Button();
            lblArchivo = new Label();
            panelSuperior = new Panel();
            LblDescripcion = new Label();
            panelContenedor = new Panel();
            TimerFecha = new System.Windows.Forms.Timer(components);
            panelMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)piclogo).BeginInit();
            panel1.SuspendLayout();
            panelSuperior.SuspendLayout();
            panelContenedor.SuspendLayout();
            SuspendLayout();
            // 
            // panelMenu
            // 
            panelMenu.BackColor = SystemColors.ActiveCaption;
            panelMenu.Controls.Add(ibtnNomina);
            panelMenu.Controls.Add(ibtnConfiguracion);
            panelMenu.Controls.Add(ibtnHorario);
            panelMenu.Controls.Add(ibtnEmpleados);
            panelMenu.Controls.Add(ibtnChecadas);
            panelMenu.Controls.Add(LblFecha);
            panelMenu.Controls.Add(piclogo);
            panelMenu.Controls.Add(IblEmpresa);
            panelMenu.Controls.Add(ibtnSalir);
            panelMenu.Controls.Add(panel1);
            panelMenu.Controls.Add(btnChecadas);
            panelMenu.Dock = DockStyle.Left;
            panelMenu.Location = new Point(0, 0);
            panelMenu.Name = "panelMenu";
            panelMenu.Size = new Size(109, 506);
            panelMenu.TabIndex = 0;
            panelMenu.Paint += panelMenu_Paint_1;
            // 
            // ibtnNomina
            // 
            ibtnNomina.FlatStyle = FlatStyle.Flat;
            ibtnNomina.IconChar = FontAwesome.Sharp.IconChar.FileInvoiceDollar;
            ibtnNomina.IconColor = Color.Black;
            ibtnNomina.IconFont = FontAwesome.Sharp.IconFont.Auto;
            ibtnNomina.IconSize = 30;
            ibtnNomina.Location = new Point(0, 356);
            ibtnNomina.Name = "ibtnNomina";
            ibtnNomina.Size = new Size(109, 40);
            ibtnNomina.TabIndex = 11;
            ibtnNomina.Text = "Generar Nómina";
            ibtnNomina.TextAlign = ContentAlignment.MiddleLeft;
            ibtnNomina.TextImageRelation = TextImageRelation.ImageBeforeText;
            ibtnNomina.UseVisualStyleBackColor = true;
            ibtnNomina.Click += btnNomina_Click;
            // 
            // ibtnConfiguracion
            // 
            ibtnConfiguracion.FlatStyle = FlatStyle.Flat;
            ibtnConfiguracion.IconChar = FontAwesome.Sharp.IconChar.Cogs;
            ibtnConfiguracion.IconColor = Color.Black;
            ibtnConfiguracion.IconFont = FontAwesome.Sharp.IconFont.Auto;
            ibtnConfiguracion.IconSize = 30;
            ibtnConfiguracion.Location = new Point(0, 310);
            ibtnConfiguracion.Name = "ibtnConfiguracion";
            ibtnConfiguracion.Size = new Size(109, 40);
            ibtnConfiguracion.TabIndex = 10;
            ibtnConfiguracion.Text = "Settings";
            ibtnConfiguracion.TextAlign = ContentAlignment.MiddleLeft;
            ibtnConfiguracion.TextImageRelation = TextImageRelation.ImageBeforeText;
            ibtnConfiguracion.UseVisualStyleBackColor = true;
            ibtnConfiguracion.Click += btnConfiguracion_Click;
            // 
            // ibtnHorario
            // 
            ibtnHorario.FlatStyle = FlatStyle.Flat;
            ibtnHorario.IconChar = FontAwesome.Sharp.IconChar.Calendar;
            ibtnHorario.IconColor = Color.Black;
            ibtnHorario.IconFont = FontAwesome.Sharp.IconFont.Auto;
            ibtnHorario.IconSize = 30;
            ibtnHorario.Location = new Point(0, 264);
            ibtnHorario.Name = "ibtnHorario";
            ibtnHorario.Size = new Size(109, 40);
            ibtnHorario.TabIndex = 10;
            ibtnHorario.Text = "Horario Manual";
            ibtnHorario.TextAlign = ContentAlignment.MiddleLeft;
            ibtnHorario.TextImageRelation = TextImageRelation.ImageBeforeText;
            ibtnHorario.UseVisualStyleBackColor = true;
            ibtnHorario.Click += btnHorario_Click;
            // 
            // ibtnEmpleados
            // 
            ibtnEmpleados.FlatStyle = FlatStyle.Flat;
            ibtnEmpleados.IconChar = FontAwesome.Sharp.IconChar.Users;
            ibtnEmpleados.IconColor = Color.Black;
            ibtnEmpleados.IconFont = FontAwesome.Sharp.IconFont.Auto;
            ibtnEmpleados.IconSize = 30;
            ibtnEmpleados.Location = new Point(0, 218);
            ibtnEmpleados.Name = "ibtnEmpleados";
            ibtnEmpleados.Size = new Size(109, 40);
            ibtnEmpleados.TabIndex = 9;
            ibtnEmpleados.Text = "Empleados";
            ibtnEmpleados.TextAlign = ContentAlignment.MiddleLeft;
            ibtnEmpleados.TextImageRelation = TextImageRelation.ImageBeforeText;
            ibtnEmpleados.UseVisualStyleBackColor = true;
            ibtnEmpleados.Click += btnEmpleados_Click;
            // 
            // ibtnChecadas
            // 
            ibtnChecadas.FlatStyle = FlatStyle.Flat;
            ibtnChecadas.IconChar = FontAwesome.Sharp.IconChar.File;
            ibtnChecadas.IconColor = Color.Black;
            ibtnChecadas.IconFont = FontAwesome.Sharp.IconFont.Auto;
            ibtnChecadas.IconSize = 30;
            ibtnChecadas.Location = new Point(0, 172);
            ibtnChecadas.Name = "ibtnChecadas";
            ibtnChecadas.Size = new Size(109, 40);
            ibtnChecadas.TabIndex = 8;
            ibtnChecadas.Text = "Cargar Checadas";
            ibtnChecadas.TextAlign = ContentAlignment.MiddleLeft;
            ibtnChecadas.TextImageRelation = TextImageRelation.ImageBeforeText;
            ibtnChecadas.UseVisualStyleBackColor = true;
            ibtnChecadas.Click += btnChecadas_Click;
            // 
            // LblFecha
            // 
            LblFecha.AutoSize = true;
            LblFecha.Font = new Font("Segoe UI", 11.25F, FontStyle.Italic, GraphicsUnit.Point, 0);
            LblFecha.Location = new Point(12, 113);
            LblFecha.Name = "LblFecha";
            LblFecha.Size = new Size(85, 20);
            LblFecha.TabIndex = 0;
            LblFecha.Text = "00/00/0000";
            LblFecha.TextAlign = ContentAlignment.MiddleCenter;
            LblFecha.Click += LblFecha_Click;
            // 
            // piclogo
            // 
            piclogo.BorderStyle = BorderStyle.FixedSingle;
            piclogo.Image = (Image)resources.GetObject("piclogo.Image");
            piclogo.Location = new Point(-17, 0);
            piclogo.Name = "piclogo";
            piclogo.Size = new Size(144, 110);
            piclogo.SizeMode = PictureBoxSizeMode.Zoom;
            piclogo.TabIndex = 0;
            piclogo.TabStop = false;
            // 
            // IblEmpresa
            // 
            IblEmpresa.AutoSize = true;
            IblEmpresa.Font = new Font("Segoe UI Black", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            IblEmpresa.Location = new Point(1, 71);
            IblEmpresa.Name = "IblEmpresa";
            IblEmpresa.Size = new Size(105, 25);
            IblEmpresa.TabIndex = 0;
            IblEmpresa.Text = "CASA RED";
            IblEmpresa.TextAlign = ContentAlignment.MiddleCenter;
            IblEmpresa.Click += label1_Click;
            // 
            // ibtnSalir
            // 
            ibtnSalir.BackColor = SystemColors.ActiveCaption;
            ibtnSalir.Cursor = Cursors.Hand;
            ibtnSalir.Dock = DockStyle.Bottom;
            ibtnSalir.FlatAppearance.BorderSize = 0;
            ibtnSalir.FlatStyle = FlatStyle.Flat;
            ibtnSalir.Font = new Font("Segoe UI", 12F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            ibtnSalir.ForeColor = Color.Red;
            ibtnSalir.Location = new Point(0, 477);
            ibtnSalir.Name = "ibtnSalir";
            ibtnSalir.Size = new Size(109, 29);
            ibtnSalir.TabIndex = 6;
            ibtnSalir.Text = "SALIR";
            ibtnSalir.UseVisualStyleBackColor = false;
            ibtnSalir.Click += btnSalir_Click;
            // 
            // panel1
            // 
            panel1.BackColor = Color.LightSlateGray;
            panel1.Controls.Add(lblEstadoTexto);
            panel1.Controls.Add(lblEstado);
            panel1.Cursor = Cursors.Help;
            panel1.Location = new Point(0, 402);
            panel1.Name = "panel1";
            panel1.Size = new Size(109, 69);
            panel1.TabIndex = 0;
            // 
            // lblEstadoTexto
            // 
            lblEstadoTexto.AutoSize = true;
            lblEstadoTexto.Font = new Font("Segoe UI", 8.25F, FontStyle.Italic, GraphicsUnit.Point, 0);
            lblEstadoTexto.ForeColor = SystemColors.Control;
            lblEstadoTexto.Location = new Point(5, 37);
            lblEstadoTexto.Name = "lblEstadoTexto";
            lblEstadoTexto.Size = new Size(101, 13);
            lblEstadoTexto.TabIndex = 1;
            lblEstadoTexto.Text = "Sin archivo cargado";
            lblEstadoTexto.Click += lblEstadoArchivo_Click;
            // 
            // lblEstado
            // 
            lblEstado.AutoSize = true;
            lblEstado.ForeColor = SystemColors.Control;
            lblEstado.Location = new Point(24, 13);
            lblEstado.Name = "lblEstado";
            lblEstado.Size = new Size(59, 15);
            lblEstado.TabIndex = 0;
            lblEstado.Text = "ESTADO ●";
            lblEstado.Click += LblEstado_Click;
            // 
            // btnChecadas
            // 
            btnChecadas.Location = new Point(0, 0);
            btnChecadas.Name = "btnChecadas";
            btnChecadas.Size = new Size(75, 23);
            btnChecadas.TabIndex = 7;
            // 
            // lblArchivo
            // 
            lblArchivo.AutoSize = true;
            lblArchivo.Font = new Font("Segoe UI", 9F, FontStyle.Italic, GraphicsUnit.Point, 0);
            lblArchivo.ForeColor = SystemColors.Control;
            lblArchivo.Location = new Point(74, 361);
            lblArchivo.Name = "lblArchivo";
            lblArchivo.Size = new Size(53, 15);
            lblArchivo.TabIndex = 2;
            lblArchivo.Text = "Ninguno\r\n";
            lblArchivo.Click += lblArchivo_Click;
            // 
            // panelSuperior
            // 
            panelSuperior.BackColor = Color.LightSlateGray;
            panelSuperior.Controls.Add(LblDescripcion);
            panelSuperior.Dock = DockStyle.Top;
            panelSuperior.Location = new Point(109, 0);
            panelSuperior.Name = "panelSuperior";
            panelSuperior.Size = new Size(729, 41);
            panelSuperior.TabIndex = 1;
            // 
            // LblDescripcion
            // 
            LblDescripcion.AutoSize = true;
            LblDescripcion.BackColor = Color.Transparent;
            LblDescripcion.Font = new Font("Segoe UI Black", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            LblDescripcion.ForeColor = Color.White;
            LblDescripcion.Location = new Point(6, 9);
            LblDescripcion.Name = "LblDescripcion";
            LblDescripcion.Size = new Size(193, 25);
            LblDescripcion.TabIndex = 0;
            LblDescripcion.Text = "Sistema de Nómina";
            // 
            // panelContenedor
            // 
            panelContenedor.BackColor = Color.WhiteSmoke;
            panelContenedor.BorderStyle = BorderStyle.FixedSingle;
            panelContenedor.Controls.Add(lblArchivo);
            panelContenedor.Cursor = Cursors.Hand;
            panelContenedor.Dock = DockStyle.Fill;
            panelContenedor.Location = new Point(109, 41);
            panelContenedor.Name = "panelContenedor";
            panelContenedor.Size = new Size(729, 465);
            panelContenedor.TabIndex = 2;
            panelContenedor.Paint += panelContenedor_Paint;
            // 
            // TimerFecha
            // 
            TimerFecha.Interval = 1000;
            TimerFecha.Tick += TimerFecha_Tick_1;
            // 
            // FrmPrincipal
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.WhiteSmoke;
            ClientSize = new Size(838, 506);
            Controls.Add(panelContenedor);
            Controls.Add(panelSuperior);
            Controls.Add(panelMenu);
            Name = "FrmPrincipal";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Nomina Casa Red";
            WindowState = FormWindowState.Maximized;
            Load += FrmPrincipal_Load;
            panelMenu.ResumeLayout(false);
            panelMenu.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)piclogo).EndInit();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panelSuperior.ResumeLayout(false);
            panelSuperior.PerformLayout();
            panelContenedor.ResumeLayout(false);
            panelContenedor.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panelMenu;
        private Panel panelSuperior;
        private PictureBox piclogo;
        private Panel panelContenedor;
        private Label IblEmpresa;
        private Button btnChecadas;
        private Panel panel1;
        private Label lblEstado;
        private Button ibtnSalir;
        private Label LblDescripcion;
        private Label LblFecha;
        private System.Windows.Forms.Timer TimerFecha;
        private Label lblEstadoTexto;
        private Label lblArchivo;
        private FontAwesome.Sharp.IconButton ibtnChecadas;
        private FontAwesome.Sharp.IconButton ibtnEmpleados;
        private FontAwesome.Sharp.IconButton ibtnNomina;
        private FontAwesome.Sharp.IconButton ibtnConfiguracion;
        private FontAwesome.Sharp.IconButton ibtnHorario;
    }
}
