using System;
using System.Windows.Forms;

namespace TemplateTPCorto           // ↓ idéntico namespace
{
    public partial class SupervisorForm : Form  // ↓ idéntico nombre + partial + hereda de Form
    {
        private readonly string _usuario;
        public SupervisorForm(string usuario)
        {
            InitializeComponent();
            _usuario = usuario;
        }

        // Los manejadores que el designer espera:
        private void btnModificarPersona_Click(object sender, EventArgs e)
        {
            using (var f = new ModifyPersonForm())
            {
                f.ShowDialog();
            }
        }

        private void btnDesbloquearCredencial_Click(object sender, EventArgs e)
        {
            using (var f = new ChangePasswordForm(/*…*/))
            {
                f.ShowDialog();
            }
        }

    }
}
