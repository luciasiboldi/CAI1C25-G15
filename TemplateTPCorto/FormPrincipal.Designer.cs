namespace TemplateTPCorto
{
    partial class FormPrincipal
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblBienvenido;
        private System.Windows.Forms.Button btnSupervisor;
        private System.Windows.Forms.Button btnAdministrador;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblBienvenido = new System.Windows.Forms.Label();
            this.btnSupervisor = new System.Windows.Forms.Button();
            this.btnAdministrador = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblBienvenido
            // 
            this.lblBienvenido.AutoSize = true;
            this.lblBienvenido.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblBienvenido.Location = new System.Drawing.Point(20, 20);
            this.lblBienvenido.Name = "lblBienvenido";
            this.lblBienvenido.Size = new System.Drawing.Size(110, 21);
            this.lblBienvenido.Text = "Bienvenido, --";
            // 
            // btnSupervisor
            // 
            this.btnSupervisor.Location = new System.Drawing.Point(20, 60);
            this.btnSupervisor.Name = "btnSupervisor";
            this.btnSupervisor.Size = new System.Drawing.Size(180, 30);
            this.btnSupervisor.Text = "Panel Supervisor";
            this.btnSupervisor.UseVisualStyleBackColor = true;
            this.btnSupervisor.Click += new System.EventHandler(this.btnSupervisor_Click);
            // 
            // btnAdministrador
            // 
            this.btnAdministrador.Location = new System.Drawing.Point(220, 60);
            this.btnAdministrador.Name = "btnAdministrador";
            this.btnAdministrador.Size = new System.Drawing.Size(180, 30);
            this.btnAdministrador.Text = "Panel Administrador";
            this.btnAdministrador.UseVisualStyleBackColor = true;
            this.btnAdministrador.Click += new System.EventHandler(this.btnAdministrador_Click);
            // 
            // FormPrincipal
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(420, 150);
            this.Controls.Add(this.lblBienvenido);
            this.Controls.Add(this.btnSupervisor);
            this.Controls.Add(this.btnAdministrador);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormPrincipal";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Pantalla Principal";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}