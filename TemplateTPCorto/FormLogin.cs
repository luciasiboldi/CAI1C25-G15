using System;
using System.Windows.Forms;
using Negocio;  // LoginNegocio y PasswordService

namespace TemplateTPCorto
{
    public partial class FormLogin : Form
    {
        private readonly LoginNegocio _loginNeg = new LoginNegocio();
        private readonly PasswordService _pwdSvc = new PasswordService();

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
                // 1) Autenticar
                _loginNeg.Autenticar(usuario, password);

                // 2) Forzar cambio si corresponde
                if (_pwdSvc.NecesitaCambiar(usuario))
                {
                    // → Alerta previa
                    MessageBox.Show(
                        "Su contraseña ha vencido. Por favor, cambie la contraseña.",
                        "Contraseña expirada",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );

                    using (var changeForm = new ChangePasswordForm())
                    {
                        if (changeForm.ShowDialog() != DialogResult.OK)
                        {
                            MessageBox.Show(
                                "Debe cambiar su contraseña para continuar.",
                                "Atención",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning
                            );
                            return;
                        }
                    }
                }

                // 3) Login exitoso
                MessageBox.Show(
                    "Ingreso exitoso",
                    "Éxito",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                // → Aquí podrías abrir tu FormPrincipal:
                // var mainForm = new FormPrincipal(usuario);
                // this.Hide();
                // mainForm.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Error de autenticación",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
            }
        }

        private void btnCambiarPassword_Click(object sender, EventArgs e)
        {
            using (var changeForm = new ChangePasswordForm())
            {
                changeForm.ShowDialog();
            }
        }
    }
}
