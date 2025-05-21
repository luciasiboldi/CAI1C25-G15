using System;
using System.Windows.Forms;
using Negocio;    // LoginNegocio

namespace TemplateTPCorto
{
    public partial class FormLogin : Form
    {
        private readonly LoginNegocio _loginNeg = new LoginNegocio();

        public FormLogin()
        {
            InitializeComponent();
        }

        private void btnIngresar_Click(object sender, EventArgs e)
        {
            var usuario = txtUsuario.Text.Trim();
            var password = txtPassword.Text;

            try
            {
                // Si no lanza excepción, las credenciales son correctas
                _loginNeg.Autenticar(usuario, password);

                MessageBox.Show(
                    "Ingreso exitoso",
                    "Éxito",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
            catch (Exception)
            {
                // Cualquier fallo (usuario no existe, contraseña mal, bloqueado…)
                MessageBox.Show(
                    "Usuario o contraseña incorrecto",
                    "Error de autenticación",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
            }
        }
    }
}