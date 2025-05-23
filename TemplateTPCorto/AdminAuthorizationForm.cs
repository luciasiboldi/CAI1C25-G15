using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Persistencia;

namespace TemplateTPCorto
{
    public partial class AdminAuthorizationForm : Form
    {
        public AdminAuthorizationForm()
        {
            InitializeComponent();
            CargarOperacionesPendientes();
        }

        private void CargarOperacionesPendientes()
        {
            var file = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "operaciones.csv"
            );
            if (!File.Exists(file)) return;
            var lines = File.ReadAllLines(file).Skip(1);
            // …
        }

        // ← Este método faltaba:
        private void btnAprobar_Click(object sender, EventArgs e)
        {
            if (dgvOperaciones.SelectedRows.Count == 0) return;
            var id = dgvOperaciones.SelectedRows[0].Cells["ID"].Value?.ToString();
            // … lógica de aprobación …
            MessageBox.Show($"Operación {id} aprobada.", "OK", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
