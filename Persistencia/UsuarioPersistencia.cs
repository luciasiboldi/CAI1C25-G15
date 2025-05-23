using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Datos;

namespace Persistencia
{
    public class UsuarioPersistencia
    {
        private readonly string _carpeta =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory);
        private const string CredencialesCsv = "credenciales.csv";
        private const string IntentosCsv = "login_intentos.csv";
        private const string BloqueadosCsv = "usuario_bloqueado.csv";
        public const int MaxIntentos = 3;

        private string Ruta(string nombre) => Path.Combine(_carpeta, nombre);

        private IEnumerable<string> LeerTodas(string file)
        {
            var ruta = Ruta(file);
            if (!File.Exists(ruta)) return Enumerable.Empty<string>();

            var todas = File.ReadAllLines(ruta)
                            .Where(l => !string.IsNullOrWhiteSpace(l));

            // Solo la credenciales tiene cabecera:
            if (file.Equals(CredencialesCsv, StringComparison.OrdinalIgnoreCase))
                return todas.Skip(1);

            return todas;
        }

        private void AgregarLinea(string file, string linea)
        {
            var ruta = Ruta(file);
            File.AppendAllText(ruta, linea + Environment.NewLine);
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
               .Any(l => l.Split(';')[0] == legajo);

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
            => AgregarLinea(BloqueadosCsv, legajo);

        public void ActualizarCredencial(string usuario, string nuevaContrasena)
        {
            var ruta = Ruta(CredencialesCsv);
            var lines = File.ReadAllLines(ruta).ToList();
            if (lines.Count <= 1) return;  // solo cabecera

            for (int i = 1; i < lines.Count; i++)
            {
                var cols = lines[i].Split(';');
                if (cols.Length < 5) continue;

                if (cols[1].Equals(usuario, StringComparison.OrdinalIgnoreCase))
                {
                    cols[2] = nuevaContrasena;                        // nueva pass
                    cols[4] = DateTime.Today.ToString("d/M/yyyy");    // fecha último login
                    lines[i] = string.Join(";", cols);
                    break;
                }
            }

            File.WriteAllLines(ruta, lines);
        }
    }
}
