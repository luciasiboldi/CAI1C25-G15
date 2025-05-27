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