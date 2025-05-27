namespace TemplateTPCorto
{
    partial class SupervisorForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.DataGridView dgvBloqueados;
        private System.Windows.Forms.Button btnDesbloquear;
        private System.Windows.Forms.Button btnModificarPersona;
        private System.Windows.Forms.Button btnMostrarSupervisores;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.dgvBloqueados = new System.Windows.Forms.DataGridView();
            this.btnDesbloquear = new System.Windows.Forms.Button();
            this.btnModificarPersona = new System.Windows.Forms.Button();
            this.btnMostrarSupervisores = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBloqueados)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvBloqueados
            // 
            this.dgvBloqueados.AllowUserToAddRows = false;
            this.dgvBloqueados.AllowUserToDeleteRows = false;
            this.dgvBloqueados.ReadOnly = true;
            this.dgvBloqueados.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvBloqueados.Location = new System.Drawing.Point(20, 20);
            this.dgvBloqueados.Size = new System.Drawing.Size(360, 160);
            // 
            // btnDesbloquear
            // 
            this.btnDesbloquear.Location = new System.Drawing.Point(20, 200);
            this.btnDesbloquear.Size = new System.Drawing.Size(360, 30);
            this.btnDesbloquear.Text = "Desbloquear usuario seleccionado";
            this.btnDesbloquear.Click += new System.EventHandler(this.btnDesbloquear_Click);
            // 
            // btnModificarPersona
            // 
            this.btnModificarPersona.Location = new System.Drawing.Point(20, 240);
            this.btnModificarPersona.Size = new System.Drawing.Size(360, 30);
            this.btnModificarPersona.Text = "Modificar datos de operador";
            this.btnModificarPersona.Click += new System.EventHandler(this.btnModificarPersona_Click);
            // 
            // btnMostrarSupervisores
            // 
            this.btnMostrarSupervisores.Location = new System.Drawing.Point(20, 280);
            this.btnMostrarSupervisores.Size = new System.Drawing.Size(360, 30);
            this.btnMostrarSupervisores.Text = "Mostrar Supervisores";
            this.btnMostrarSupervisores.Click += new System.EventHandler(this.btnMostrarSupervisores_Click);
            // 
            // SupervisorForm
            // 
            this.ClientSize = new System.Drawing.Size(400, 330);
            this.Controls.Add(this.dgvBloqueados);
            this.Controls.Add(this.btnDesbloquear);
            this.Controls.Add(this.btnModificarPersona);
            this.Controls.Add(this.btnMostrarSupervisores);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Supervisor – Gestión de Operadores";
            ((System.ComponentModel.ISupportInitialize)(this.dgvBloqueados)).EndInit();
            this.ResumeLayout(false);
        }
    }
}