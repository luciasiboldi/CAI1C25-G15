using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Persistencia;  // UsuarioPersistencia, PerfilPersistencia, PersonaPersistencia

namespace TemplateTPCorto
{
    public partial class SupervisorForm : Form
    {
        private readonly UsuarioPersistencia _up = new UsuarioPersistencia();
        private readonly PerfilPersistencia _pr = new PerfilPersistencia();
        private readonly PersonaPersistencia _pp = new PersonaPersistencia();

        public SupervisorForm()
        {
            InitializeComponent();
            CargarBloqueados();
        }

        private void CargarBloqueados()
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "usuario_bloqueado.csv");
            if (!File.Exists(path)) return;

            // Leer sólo legajos bloqueados
            var bloqueados = File.ReadAllLines(path)
                                 .Skip(1)
                                 .Where(l => !string.IsNullOrWhiteSpace(l))
                                 .Select(l => l.Split(';')[0])
                                 .ToList();

            var dt = new DataTable();
            dt.Columns.Add("Legajo");
            dt.Columns.Add("Usuario");

            foreach (var leg in bloqueados)
            {
                //  Sólo si el perfil del legajo es OPERADOR (idPerfil == "1")
                if (_pr.ObtenerIdPerfil(leg) != "1") continue;

                var cred = _up.ObtenerCredencial(leg);
                dt.Rows.Add(leg, cred?.NombreUsuario ?? leg);
            }

            dgvBloqueados.DataSource = dt;
        }

        private void btnDesbloquear_Click(object sender, EventArgs e)
        {
            if (dgvBloqueados.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccioná un usuario bloqueado.", "Atención",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var legajo = dgvBloqueados.SelectedRows[0].Cells["Legajo"].Value.ToString();
            _up.DesbloquearUsuario(legajo);

            MessageBox.Show($"Usuario {legajo} desbloqueado.", "Éxito",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
            CargarBloqueados();
        }

        private void btnModificarPersona_Click(object sender, EventArgs e)
        {
            using (var f = new ModifyPersonForm())
                f.ShowDialog();
        }

        private void btnMostrarSupervisores_Click(object sender, EventArgs e)
        {
            // Muestra nombres y apellidos de todos los supervisores
            var supervisores = _pp.ObtenerTodas()
                                  .Where(p => _up.ObtenerIdPerfil(p.Legajo) == "2")
                                  .ToList();

            if (!supervisores.Any())
            {
                MessageBox.Show("No hay supervisores asignados.", "Supervisores");
                return;
            }

            var texto = string.Join(Environment.NewLine,
                supervisores.Select(s => $"{s.Legajo}: {s.Nombre} {s.Apellido}"));
            MessageBox.Show(texto, "Lista de Supervisores");
        }
    }
}