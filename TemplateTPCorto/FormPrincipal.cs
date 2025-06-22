using System;
using System.Windows.Forms;

namespace TemplateTPCorto
{
    public partial class FormPrincipal : Form
    {
        private readonly string _usuario;
        private readonly string _idPerfil;

        public FormPrincipal(string usuario, string idPerfil)
        {
            InitializeComponent();

            _usuario = usuario;
            _idPerfil = idPerfil;

            // Saludo
            lblBienvenido.Text = $"Bienvenido, {_usuario}";

            // Sólo habilita botones según perfil
            btnSupervisor.Enabled = (_idPerfil == "2");
            btnAdministrador.Enabled = (_idPerfil == "3");

            this.Load += FormPrincipal_Load;
        }

        private void FormPrincipal_Load(object sender, EventArgs e)
        {
            if (_idPerfil == "1") // Perfil Operador
            {
                using (var f = new CarritoForm(_usuario))
                {
                    this.Visible = false;
                    f.ShowDialog();
                    this.Close(); // Cierra el form principal al cerrar el carrito
                }
            }
        }

        private void btnSupervisor_Click(object sender, EventArgs e)
        {
            using (var f = new SupervisorForm())
                f.ShowDialog();
        }

        private void btnAdministrador_Click(object sender, EventArgs e)
        {
            using (var f = new AdminAuthorizationForm())
                f.ShowDialog();
        }
    }
}