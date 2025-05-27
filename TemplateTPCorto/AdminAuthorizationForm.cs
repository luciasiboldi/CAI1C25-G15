using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Persistencia;

namespace TemplateTPCorto
{
    public partial class AdminAuthorizationForm : Form
    {
        private readonly UsuarioPersistencia _up = new UsuarioPersistencia();
        private DataTable _tabla;

        public AdminAuthorizationForm()
        {
            InitializeComponent();
            CargarUsuarios();
        }

        private void CargarUsuarios()
        {
            var baseDir = AppDomain.CurrentDomain.BaseDirectory;
            var creds = File.ReadAllLines(Path.Combine(baseDir, "credenciales.csv"))
                              .Skip(1)
                              .Select(l => l.Split(';'))
                              .Select(c => new { Legajo = c[0], Usuario = c[1] });

            var perf = File.ReadAllLines(Path.Combine(baseDir, "usuario_perfil.csv"))
                           .Skip(1)
                           .Select(l => l.Split(';'))
                           .ToDictionary(c => c[0], c => c[1]);

            var bloque = File.ReadAllLines(Path.Combine(baseDir, "usuario_bloqueado.csv"))
                             .Skip(1)
                             .Select(l => l.Split(';')[0])
                             .ToHashSet();

            _tabla = new DataTable();
            _tabla.Columns.Add("Legajo");
            _tabla.Columns.Add("Usuario");
            _tabla.Columns.Add("Bloqueado", typeof(bool));
            _tabla.Columns.Add("PerfilID", typeof(string));

            foreach (var c in creds)
            {
                var idp = perf.TryGetValue(c.Legajo, out var p) ? p : "1";
                _tabla.Rows.Add(c.Legajo, c.Usuario, bloque.Contains(c.Legajo), idp);
            }

            dgvUsuarios.DataSource = _tabla;

            // ComboBox column: seleccionar nuevo perfil
            var perfiles = File.ReadAllLines(Path.Combine(baseDir, "perfil.csv"))
                               .Skip(1)
                               .Select(l => l.Split(';'))
                               .ToDictionary(x => x[0], x => x[1]);

            var combo = new DataGridViewComboBoxColumn
            {
                HeaderText = "Nuevo Perfil",
                DataPropertyName = "PerfilID",
                DataSource = perfiles.ToList(),
                ValueMember = "Key",
                DisplayMember = "Value"
            };
            dgvUsuarios.Columns.Add(combo);

            // Checkbox columna para editar bloqueo
            var chk = new DataGridViewCheckBoxColumn
            {
                HeaderText = "Bloqueado?",
                DataPropertyName = "Bloqueado"
            };
            dgvUsuarios.Columns.Add(chk);
        }

        private void btnGuardarCambios_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgvUsuarios.Rows)
            {
                var legajo = row.Cells["Legajo"].Value.ToString();
                var bloqueOld = (bool)row.Cells["Bloqueado"].Value;
                var bloqueNew = Convert.ToBoolean(row.Cells["Bloqueado?"].Value);
                var perfilOld = row.Cells["PerfilID"].Tag?.ToString() ?? row.Cells["PerfilID"].Value.ToString();
                var perfilNew = row.Cells["Nuevo Perfil"].Value.ToString();

                // bloqueo / desbloqueo
                if (bloqueOld != bloqueNew)
                {
                    if (bloqueNew) _up.BloquearUsuario(legajo);
                    else _up.DesbloquearUsuario(legajo);
                }

                // cambio de perfil
                if (perfilOld != perfilNew)
                    _up.ActualizarPerfilUsuario(legajo, perfilNew);
            }

            MessageBox.Show("Cambios guardados.", "OK",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }
    }
}