using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Persistencia;
using System.Collections.Generic;

namespace TemplateTPCorto
{
    public partial class AdminAuthorizationForm : Form
    {
        private readonly UsuarioPersistencia _up = new UsuarioPersistencia();
        private readonly PerfilPersistencia _pp = new PerfilPersistencia();
        private DataTable _tabla;
        private Dictionary<string, (bool isBlocked, string profileId)> _originalValues;

        public AdminAuthorizationForm()
        {
            InitializeComponent();
            CargarUsuarios();
            dgvUsuarios.CellValueChanged += dgvUsuarios_CellValueChanged;
        }

        private Dictionary<string, string> ObtenerPerfilesComoDiccionario()
        {
            var diccionario = new Dictionary<string, string>();
            var listaPerfiles = _pp.ObtenerTodosLosPerfiles();
            foreach (var perfil in listaPerfiles)
            {
                diccionario[perfil.IdPerfil] = perfil.NombrePerfil;
            }
            return diccionario;
        }

        private void CargarUsuarios()
        {
            var creds = _up.ObtenerTodosLosUsuarios();
            var perfiles = ObtenerPerfilesComoDiccionario();

            _tabla = new DataTable();
            _tabla.Columns.Add("Legajo");
            _tabla.Columns.Add("Usuario");
            _tabla.Columns.Add("Bloqueado", typeof(bool));
            _tabla.Columns.Add("PerfilID", typeof(string));

            _originalValues = new Dictionary<string, (bool, string)>();

            foreach (var c in creds)
            {
                var idp = _up.ObtenerIdPerfil(c.Legajo) ?? "1";
                var isBlocked = _up.EsUsuarioBloqueado(c.Legajo);
                _tabla.Rows.Add(c.Legajo, c.NombreUsuario, isBlocked, idp);
                _originalValues[c.Legajo] = (isBlocked, idp);
            }

            dgvUsuarios.DataSource = _tabla;

            var combo = new DataGridViewComboBoxColumn
            {
                HeaderText = "Nuevo Perfil",
                DataPropertyName = "PerfilID",
                DataSource = new BindingSource(perfiles, null),
                ValueMember = "Key",
                DisplayMember = "Value"
            };
            dgvUsuarios.Columns.Add(combo);
        }

        private void dgvUsuarios_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                dgvUsuarios.EndEdit();
            }
        }

        private void btnGuardarCambios_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgvUsuarios.Rows)
            {
                if (row.IsNewRow) continue;

                var legajo = row.Cells["Legajo"].Value.ToString();
                if (!_originalValues.ContainsKey(legajo)) continue;

                var (bloqueOriginal, perfilOriginal) = _originalValues[legajo];

                var bloqueActual = Convert.ToBoolean(row.Cells["Bloqueado"].Value);
                var perfilActual = row.Cells["PerfilID"].Value.ToString();

                // bloqueo / desbloqueo
                if (bloqueOriginal != bloqueActual)
                {
                    if (bloqueActual) _up.BloquearUsuario(legajo);
                    else
                    {
                        _up.DesbloquearUsuario(legajo);
                        _up.LimpiarIntentos(legajo);
                    }
                }

                // cambio de perfil
                if (perfilOriginal != perfilActual)
                    _up.ActualizarPerfilUsuario(legajo, perfilActual);
            }

            MessageBox.Show("Cambios guardados.", "OK",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }
    }
}