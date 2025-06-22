using System;
using System.Windows.Forms;
using Datos;

namespace TemplateTPCorto
{
    public partial class ModifyPersonForm : Form
    {
        public Persona PersonaEditada { get; private set; }
        public string UsuarioEditado { get; private set; }

        public ModifyPersonForm(Persona persona, Credencial credencial)
        {
            InitializeComponent();
            PersonaEditada = persona;
            UsuarioEditado = credencial.NombreUsuario;

            txtNombre.Text = persona.Nombre;
            txtApellido.Text = persona.Apellido;
            txtDNI.Text = persona.DNI.ToString();
            dtpFechaIngreso.Value = persona.FechaIngreso;
            txtUsuario.Text = credencial.NombreUsuario;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            // Validaciones
            if (string.IsNullOrWhiteSpace(txtNombre.Text) ||
                string.IsNullOrWhiteSpace(txtApellido.Text) ||
                !int.TryParse(txtDNI.Text, out int dni) ||
                string.IsNullOrWhiteSpace(txtUsuario.Text))
            {
                MessageBox.Show("Por favor, complete todos los campos correctamente.", "Error de Validación",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Actualizar el objeto Persona y Usuario
            PersonaEditada.Nombre = txtNombre.Text;
            PersonaEditada.Apellido = txtApellido.Text;
            PersonaEditada.DNI = dni;
            PersonaEditada.FechaIngreso = dtpFechaIngreso.Value;
            UsuarioEditado = txtUsuario.Text;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
