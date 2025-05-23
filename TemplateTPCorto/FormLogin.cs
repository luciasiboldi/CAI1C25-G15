using System;
using System.Windows.Forms;
using Negocio;
using Persistencia;
using Datos;


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

                // 2) Si necesita cambiar (primer login o expiró):
                if (_pwdSvc.NecesitaCambiar(usuario))
                {
                    // ← Nueva alerta antes de abrir el form
                    MessageBox.Show(
                        "Su contraseña ha vencido. Por favor, cambie la contraseña.",
                        "Contraseña expirada",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );

                    using (var changeForm = new ChangePasswordForm())
                    {
                        var dr = changeForm.ShowDialog();
                        if (dr != DialogResult.OK)
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

                // ... abrir tu FormPrincipal, etc.
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
            // Abre siempre el form, que allí pide usuario y passwords
            using (var changeForm = new ChangePasswordForm())
            {
                changeForm.ShowDialog();
            }
        }
    }
}
