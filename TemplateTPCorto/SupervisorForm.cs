using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using Datos;
using Persistencia;

namespace TemplateTPCorto
{
    public partial class SupervisorForm : Form
    {
        private readonly UsuarioPersistencia _up = new UsuarioPersistencia();
        private readonly PerfilPersistencia _pr = new PerfilPersistencia();
        private readonly PersonaPersistencia _pp = new PersonaPersistencia();
        private DataTable _tablaOperadores;

        public SupervisorForm()
        {
            InitializeComponent();
            ConfigurarGrid();
            CargarOperadores();
            dgvBloqueados.SelectionChanged += DgvOperadores_SelectionChanged;
        }

        private void ConfigurarGrid()
        {
            _tablaOperadores = new DataTable();
            _tablaOperadores.Columns.Add("Legajo");
            _tablaOperadores.Columns.Add("Usuario");
            _tablaOperadores.Columns.Add("Nombre");
            _tablaOperadores.Columns.Add("Apellido");
            _tablaOperadores.Columns.Add("DNI");
            _tablaOperadores.Columns.Add("FechaIngreso", typeof(DateTime));
            _tablaOperadores.Columns.Add("Bloqueado", typeof(bool));
            dgvBloqueados.DataSource = _tablaOperadores;
        }

        private void CargarOperadores()
        {
            _tablaOperadores.Rows.Clear();
            var todosLosUsuarios = _up.ObtenerTodosLosUsuarios();
            var todasLasPersonas = _pp.ObtenerTodas();
            var personasDict = new Dictionary<string, Persona>();
            foreach (var p in todasLasPersonas)
            {
                personasDict[p.Legajo] = p;
            }

            foreach (var usuario in todosLosUsuarios)
            {
                string idPerfil = _pr.ObtenerIdPerfil(usuario.Legajo);
                if (idPerfil == "1") // Perfil "Operador"
                {
                    Persona persona = null;
                    if (personasDict.ContainsKey(usuario.Legajo))
                    {
                        persona = personasDict[usuario.Legajo];
                    }

                    var estaBloqueado = _up.EsUsuarioBloqueado(usuario.Legajo);

                    _tablaOperadores.Rows.Add(
                        usuario.Legajo,
                        usuario.NombreUsuario,
                        persona?.Nombre ?? string.Empty,
                        persona?.Apellido ?? string.Empty,
                        persona?.DNI.ToString() ?? string.Empty,
                        persona?.FechaIngreso,
                        estaBloqueado);
                }
            }
        }

        private void DgvOperadores_SelectionChanged(object sender, EventArgs e)
        {
            bool isBlocked = false;
            if (dgvBloqueados.SelectedRows.Count > 0)
            {
                var row = dgvBloqueados.SelectedRows[0];
                isBlocked = (bool)row.Cells["Bloqueado"].Value;
            }
            btnDesbloquear.Enabled = isBlocked;
        }

        private void btnDesbloquear_Click(object sender, EventArgs e)
        {
            if (dgvBloqueados.SelectedRows.Count == 0) return;

            var legajo = dgvBloqueados.SelectedRows[0].Cells["Legajo"].Value.ToString();

            _up.DesbloquearUsuario(legajo);
            _up.LimpiarIntentos(legajo);

            MessageBox.Show($"El usuario {legajo} ha sido desbloqueado.", "Éxito",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
            CargarOperadores();
        }

        private void btnModificarPersona_Click(object sender, EventArgs e)
        {
            if (dgvBloqueados.SelectedRows.Count == 0)
            {
                MessageBox.Show("Por favor, seleccione un usuario para modificar.", "Atención",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var legajo = dgvBloqueados.SelectedRows[0].Cells["Legajo"].Value.ToString();
            var persona = EncontrarPersonaPorLegajo(legajo);
            var credencial = EncontrarCredencialPorLegajo(legajo);

            if (persona == null || credencial == null)
            {
                MessageBox.Show("No se encontraron los datos completos del usuario.", "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (var form = new ModifyPersonForm(persona, credencial))
            {
                var result = form.ShowDialog();
                if (result == DialogResult.OK)
                {
                    _pp.ActualizarPersona(form.PersonaEditada);
                    _up.ActualizarUsuario(legajo, form.UsuarioEditado);
                    MessageBox.Show("Los datos del usuario han sido actualizados.", "Éxito",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarOperadores();
                }
            }
        }

        private Persona EncontrarPersonaPorLegajo(string legajo)
        {
            var todasLasPersonas = _pp.ObtenerTodas();
            foreach (var p in todasLasPersonas)
            {
                if (p.Legajo == legajo) return p;
            }
            return null;
        }

        private Credencial EncontrarCredencialPorLegajo(string legajo)
        {
            var todosLosUsuarios = _up.ObtenerTodosLosUsuarios();
            foreach (var c in todosLosUsuarios)
            {
                if (c.Legajo == legajo) return c;
            }
            return null;
        }

        private void btnMostrarSupervisores_Click(object sender, EventArgs e)
        {
            var todasLasPersonas = _pp.ObtenerTodas();
            var supervisores = new List<string>();

            foreach (var persona in todasLasPersonas)
            {
                if (_up.ObtenerIdPerfil(persona.Legajo) == "2") // Perfil "Supervisor"
                {
                    supervisores.Add($"{persona.Legajo}: {persona.Nombre} {persona.Apellido}");
                }
            }

            if (supervisores.Count == 0)
            {
                MessageBox.Show("No se encontraron supervisores.", "Supervisores");
                return;
            }

            var texto = string.Join(Environment.NewLine, supervisores);
            MessageBox.Show(texto, "Lista de Supervisores");
        }
    }
}