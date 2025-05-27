using System;
using System.Windows.Forms;
using Negocio;        // LoginNegocio, PasswordService
using Persistencia;   // UsuarioPersistencia

namespace TemplateTPCorto
{
    public partial class FormLogin : Form
    {
        private readonly LoginNegocio _loginNeg = new LoginNegocio();
        private readonly PasswordService _pwdSvc = new PasswordService();
        private readonly UsuarioPersistencia _up = new UsuarioPersistencia();

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
                // 1) Autentico y obtengo Credencial (trae .Legajo)
                var cred = _loginNeg.Autenticar(usuario, password);
                var legajo = cred.Legajo;

                // 2) Forzar cambio de contraseña si corresponde
                if (_pwdSvc.NecesitaCambiar(usuario))
                {
                    MessageBox.Show(
                        "Su contraseña ha vencido. Por favor, cámbiela ahora.",
                        "Contraseña expirada",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
                    using (var f = new ChangePasswordForm())
                        if (f.ShowDialog() != DialogResult.OK)
                            return;
                }

                // 3) Leer perfil (1=Operador, 2=Supervisor, 3=Administrador)
                var idPerfil = _up.ObtenerIdPerfil(legajo);

                // 4) Abrir siempre FormPrincipal y pasar perfil
                using (var main = new FormPrincipal(usuario, idPerfil))
                {
                    this.Hide();
                    main.ShowDialog();
                    this.Show();
                }
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
            using (var f = new ChangePasswordForm())
            {
                f.ShowDialog();
            }
        }
    }
}