using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Datos;

namespace Persistencia
{
    public class UsuarioPersistencia
    {
        private readonly string _baseDir = AppDomain.CurrentDomain.BaseDirectory;
        private const string CredencialesCsv = "credenciales.csv";
        private const string IntentosCsv = "login_intentos.csv";
        private const string BloqueadosCsv = "usuario_bloqueado.csv";
        private const string UsuarioPerfilCsv = "usuario_perfil.csv";
        public const int MaxIntentos = 3;

        private string Ruta(string fileName) => Path.Combine(_baseDir, fileName);

        private IEnumerable<string> LeerTodas(string fileName)
        {
            var path = Ruta(fileName);
            if (!File.Exists(path)) return Enumerable.Empty<string>();

            var lines = File.ReadAllLines(path)
                            .Where(l => !string.IsNullOrWhiteSpace(l));
            // Solo credenciales tiene cabecera:
            if (fileName.Equals(CredencialesCsv, StringComparison.OrdinalIgnoreCase))
                return lines.Skip(1);
            return lines;
        }

        private void AgregarLinea(string fileName, string linea)
        {
            var path = Ruta(fileName);
            File.AppendAllText(path, linea + Environment.NewLine);
        }

        public Credencial ObtenerCredencial(string usuario)
        {
            return LeerTodas(CredencialesCsv)
                .Select(l => new Credencial(l))
                .FirstOrDefault(c => c.NombreUsuario
                    .Equals(usuario, StringComparison.OrdinalIgnoreCase));
        }

        public bool EsUsuarioBloqueado(string legajo)
            => LeerTodas(BloqueadosCsv)
                .Select(l => l.Split(';')[0])
                .Contains(legajo);

        public int ObtenerIntentos(string legajo)
            => LeerTodas(IntentosCsv)
                .Count(l => l.Split(';')[0] == legajo);

        public void RegistrarIntento(string legajo)
        {
            var linea = $"{legajo};{DateTime.Now:dd/MM/yyyy}";
            AgregarLinea(IntentosCsv, linea);
            if (ObtenerIntentos(legajo) >= MaxIntentos)
                BloquearUsuario(legajo);
        }

        public void BloquearUsuario(string legajo)
        {
            AgregarLinea(BloqueadosCsv, legajo);
        }

        public void DesbloquearUsuario(string legajo)
        {
            var path = Ruta(BloqueadosCsv);
            if (!File.Exists(path)) return;

            var kept = File.ReadAllLines(path)
                          .Where(l => string.IsNullOrWhiteSpace(l)
                                      || !l.Split(';')[0].Equals(legajo));
            File.WriteAllLines(path, kept);
        }

        public void ActualizarPerfilUsuario(string legajo, string nuevoPerfilId)
        {
            var path = Ruta(UsuarioPerfilCsv);
            if (!File.Exists(path)) return;

            var lines = File.ReadAllLines(path).ToList();
            if (lines.Count < 1) return;

            var header = lines[0];
            var data = lines.Skip(1)
                .Select(l =>
                {
                    var parts = l.Split(';');
                    return parts[0] == legajo
                        ? $"{legajo};{nuevoPerfilId}"
                        : l;
                });

            File.WriteAllLines(path, new[] { header }.Concat(data));
        }
        /// <summary>
        /// Reescribe en credenciales.csv la columna de contraseña y actualiza fecha de último login.
        /// </summary>
        public void ActualizarCredencial(string usuario, string nuevaContrasena)
        {
            var path = Ruta(CredencialesCsv);
            if (!File.Exists(path)) return;

            var lines = File.ReadAllLines(path).ToList();
            if (lines.Count <= 1) return;  // sólo cabecera o vacío

            // Cabecera en lines[0], datos desde 1...
            for (int i = 1; i < lines.Count; i++)
            {
                var cols = lines[i].Split(';');
                // cols[1] es NombreUsuario
                if (cols.Length < 5) continue;
                if (!cols[1].Equals(usuario, StringComparison.OrdinalIgnoreCase))
                    continue;

                // Actualizo contraseña (col 2) y fecha último login (col 4)
                cols[2] = nuevaContrasena;
                cols[4] = DateTime.Today.ToString("d/M/yyyy");
                lines[i] = string.Join(";", cols);
                break;
            }

            File.WriteAllLines(path, lines);
        }
        public string ObtenerIdPerfil(string legajo)
        {
            // Lee el CSV usuario_perfil.csv (sin saltar cabecera)
            var lines = LeerTodas(UsuarioPerfilCsv);
            foreach (var l in lines)
            {
                var cols = l.Split(';');
                if (cols[0].Equals(legajo, StringComparison.OrdinalIgnoreCase))
                    return cols[1];
            }
            return null;
        }
    }
}