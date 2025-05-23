namespace TemplateTPCorto
{
    partial class SupervisorForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Button btnModificarPersona;
        private System.Windows.Forms.Button btnDesbloquearCredencial;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.btnModificarPersona = new System.Windows.Forms.Button();
            this.btnDesbloquearCredencial = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // SupervisorForm
            // 
            this.ClientSize = new System.Drawing.Size(300, 200);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Supervisor";
            // 
            // btnModificarPersona
            // 
            this.btnModificarPersona.Location = new System.Drawing.Point(50, 40);
            this.btnModificarPersona.Size = new System.Drawing.Size(200, 30);
            this.btnModificarPersona.Text = "Modificar persona";
            this.btnModificarPersona.Click += new System.EventHandler(this.btnModificarPersona_Click);
            // 
            // btnDesbloquearCredencial
            // 
            this.btnDesbloquearCredencial.Location = new System.Drawing.Point(50, 100);
            this.btnDesbloquearCredencial.Size = new System.Drawing.Size(200, 30);
            this.btnDesbloquearCredencial.Text = "Desbloquear credencial";
            this.btnDesbloquearCredencial.Click += new System.EventHandler(this.btnDesbloquearCredencial_Click);
            // 
            // Agregar controles
            // 
            this.Controls.Add(this.btnModificarPersona);
            this.Controls.Add(this.btnDesbloquearCredencial);
            this.ResumeLayout(false);
        }
    }
}
