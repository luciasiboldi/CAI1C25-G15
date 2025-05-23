namespace TemplateTPCorto
{
    partial class AdminAuthorizationForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.DataGridView dgvOperaciones;
        private System.Windows.Forms.Button btnAprobar;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.dgvOperaciones = new System.Windows.Forms.DataGridView();
            this.btnAprobar = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOperaciones)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvOperaciones
            // 
            this.dgvOperaciones.AllowUserToAddRows = false;
            this.dgvOperaciones.AllowUserToDeleteRows = false;
            this.dgvOperaciones.ReadOnly = true;
            this.dgvOperaciones.Dock = System.Windows.Forms.DockStyle.Top;
            this.dgvOperaciones.Height = 300;
            // 
            // btnAprobar
            // 
            this.btnAprobar.Location = new System.Drawing.Point(100, 320);
            this.btnAprobar.Size = new System.Drawing.Size(100, 30);
            this.btnAprobar.Text = "Aprobar";
            this.btnAprobar.Click += new System.EventHandler(this.btnAprobar_Click);
            // 
            // AdminAuthorizationForm
            // 
            this.ClientSize = new System.Drawing.Size(400, 370);
            this.Controls.Add(this.dgvOperaciones);
            this.Controls.Add(this.btnAprobar);
            this.Text = "Autorizaciones";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            ((System.ComponentModel.ISupportInitialize)(this.dgvOperaciones)).EndInit();
            this.ResumeLayout(false);
        }
    }
}
