using System;
using System.Windows.Forms;
using Negocio;

namespace TemplateTPCorto
{
    public partial class ChangePasswordForm : Form
    {
        private readonly PasswordService _pwdSvc = new PasswordService();

        public ChangePasswordForm()
        {
            InitializeComponent();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            var usuario = txtUsuario.Text.Trim();
            var actualPwd = txtCurrentPassword.Text;
            var nuevaPwd = txtPassword.Text;
            var confirmaPwd = txtConfirm.Text;

            try
            {
                // 1) Validar usuario+pwd actual
                if (string.IsNullOrEmpty(usuario))
                    throw new Exception("Debes ingresar el usuario.");
                if (string.IsNullOrEmpty(actualPwd))
                    throw new Exception("Debes ingresar la contraseña actual.");

                // Autentica y bloqueos
                _pwdSvc.CambiarPassword(usuario, actualPwd, nuevaPwd);

                // 2) Validar reglas de nueva contraseña
                if (nuevaPwd.Length < 8)
                    throw new Exception("La nueva contraseña debe tener al menos 8 caracteres.");
                if (nuevaPwd == actualPwd)
                    throw new Exception("La nueva contraseña debe ser distinta de la actual.");
                if (nuevaPwd != confirmaPwd)
                    throw new Exception("La confirmación no coincide con la nueva contraseña.");

                MessageBox.Show(
                    "Contraseña cambiada con éxito.",
                    "OK",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }
    }
}
