namespace TemplateTPCorto
{
    partial class AdminAuthorizationForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.DataGridView dgvUsuarios;
        private System.Windows.Forms.Button btnGuardarCambios;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.dgvUsuarios = new System.Windows.Forms.DataGridView();
            this.btnGuardarCambios = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsuarios)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvUsuarios
            // 
            this.dgvUsuarios.AllowUserToAddRows = false;
            this.dgvUsuarios.AllowUserToDeleteRows = false;
            this.dgvUsuarios.Dock = System.Windows.Forms.DockStyle.Top;
            this.dgvUsuarios.Height = 300;
            // 
            // btnGuardarCambios
            // 
            this.btnGuardarCambios.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnGuardarCambios.Height = 30;
            this.btnGuardarCambios.Text = "Guardar cambios";
            this.btnGuardarCambios.Click += new System.EventHandler(this.btnGuardarCambios_Click);
            // 
            // AdminAuthorizationForm
            // 
            this.ClientSize = new System.Drawing.Size(600, 330);
            this.Controls.Add(this.dgvUsuarios);
            this.Controls.Add(this.btnGuardarCambios);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Administrador – Gestión de Usuarios";
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsuarios)).EndInit();
            this.ResumeLayout(false);
        }
    }
}