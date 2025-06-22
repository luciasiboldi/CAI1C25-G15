using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Datos;

namespace Persistencia
{
    public class UsuarioPersistencia
    {
        private const string CredencialesCsv = "credenciales.csv";
        private const string IntentosCsv = "login_intentos.csv";
        private const string BloqueadosCsv = "usuario_bloqueado.csv";
        private const string UsuarioPerfilCsv = "usuario_perfil.csv";
        public const int MaxIntentos = 3;

        private string Ruta(string fileName)
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string solutionRoot = Path.GetFullPath(Path.Combine(baseDirectory, @"..\..\.."));
            return Path.Combine(solutionRoot, "Persistencia", "DataBase", "Tablas", fileName);
        }

        private IEnumerable<string> LeerLineasNoVacias(string path)
        {
            var lineas = File.ReadAllLines(path);
            var resultado = new List<string>();
            foreach (var linea in lineas)
            {
                if (!string.IsNullOrWhiteSpace(linea))
                {
                    resultado.Add(linea);
                }
            }
            return resultado;
        }

        private IEnumerable<string> SaltarCabecera(IEnumerable<string> lineas)
        {
            bool esPrimeraLinea = true;
            var resultado = new List<string>();
            foreach (var linea in lineas)
            {
                if (esPrimeraLinea)
                {
                    esPrimeraLinea = false;
                    continue;
                }
                resultado.Add(linea);
            }
            return resultado;
        }

        private IEnumerable<string> LeerTodas(string fileName, bool skipHeader = true)
        {
            var path = Ruta(fileName);
            if (!File.Exists(path)) return new List<string>();

            var lines = LeerLineasNoVacias(path);

            if (skipHeader)
            {
                return SaltarCabecera(lines);
            }
            return lines;
        }

        private void AgregarLinea(string fileName, string linea)
        {
            var path = Ruta(fileName);
            File.AppendAllText(path, linea + Environment.NewLine);
        }

        public Credencial ObtenerCredencial(string usuario)
        {
            var lineas = LeerTodas(CredencialesCsv);
            foreach (var linea in lineas)
            {
                var credencial = new Credencial(linea);
                if (credencial.NombreUsuario.Equals(usuario, StringComparison.OrdinalIgnoreCase))
                {
                    return credencial;
                }
            }
            return null;
        }

        public IEnumerable<Credencial> ObtenerTodosLosUsuarios()
        {
            var lineas = LeerTodas(CredencialesCsv);
            var credenciales = new List<Credencial>();
            foreach (var linea in lineas)
            {
                credenciales.Add(new Credencial(linea));
            }
            return credenciales;
        }

        public bool EsUsuarioBloqueado(string legajo)
        {
            var lineas = LeerTodas(BloqueadosCsv, skipHeader: false);
            foreach (var linea in lineas)
            {
                var columnas = linea.Split(';');
                if (columnas.Length > 0 && columnas[0] == legajo)
                {
                    return true;
                }
            }
            return false;
        }

        public int ObtenerIntentos(string legajo)
        {
            var lineas = LeerTodas(IntentosCsv, skipHeader: false);
            int contador = 0;
            foreach (var linea in lineas)
            {
                var columnas = linea.Split(';');
                if (columnas.Length > 0 && columnas[0] == legajo)
                {
                    contador++;
                }
            }
            return contador;
        }

        public void RegistrarIntento(string legajo)
        {
            var linea = $"{legajo};{DateTime.Now:dd/MM/yyyy}";
            AgregarLinea(IntentosCsv, linea);
            if (ObtenerIntentos(legajo) >= MaxIntentos)
                BloquearUsuario(legajo);
        }

        public void LimpiarIntentos(string legajo)
        {
            var path = Ruta(IntentosCsv);
            if (!File.Exists(path)) return;

            var lineas = File.ReadAllLines(path);
            var lineasAGuardar = new List<string>();
            foreach (var linea in lineas)
            {
                if (string.IsNullOrWhiteSpace(linea))
                {
                    lineasAGuardar.Add(linea);
                    continue;
                }
                var columnas = linea.Split(';');
                if (columnas.Length == 0 || !columnas[0].Equals(legajo))
                {
                    lineasAGuardar.Add(linea);
                }
            }
            File.WriteAllLines(path, lineasAGuardar);
        }

        public void BloquearUsuario(string legajo)
        {
            AgregarLinea(BloqueadosCsv, legajo);
        }

        public void DesbloquearUsuario(string legajo)
        {
            var path = Ruta(BloqueadosCsv);
            if (!File.Exists(path)) return;

            var lineas = File.ReadAllLines(path);
            var lineasAGuardar = new List<string>();
            foreach (var linea in lineas)
            {
                if (string.IsNullOrWhiteSpace(linea))
                {
                    lineasAGuardar.Add(linea);
                    continue;
                }
                var columnas = linea.Split(';');
                if (columnas.Length == 0 || !columnas[0].Equals(legajo))
                {
                    lineasAGuardar.Add(linea);
                }
            }
            File.WriteAllLines(path, lineasAGuardar);
        }

        public void ActualizarPerfilUsuario(string legajo, string nuevoPerfilId)
        {
            var path = Ruta(UsuarioPerfilCsv);
            if (!File.Exists(path)) return;

            var lines = File.ReadAllLines(path).ToList();
            var userLineIndex = -1;

            // Start from 1 to skip header
            for (int i = 1; i < lines.Count; i++)
            {
                if (string.IsNullOrWhiteSpace(lines[i])) continue;

                var cols = lines[i].Split(';');
                if (cols.Length > 0 && cols[0].Equals(legajo, StringComparison.OrdinalIgnoreCase))
                {
                    userLineIndex = i;
                    break;
                }
            }

            var newLine = $"{legajo};{nuevoPerfilId}";

            if (userLineIndex != -1)
            {
                // User found, update the line
                lines[userLineIndex] = newLine;
            }
            else
            {
                // User not found, add a new line
                lines.Add(newLine);
            }

            File.WriteAllLines(path, lines);
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
            // Lee el CSV usuario_perfil.csv (saltando cabecera)
            var lines = LeerTodas(UsuarioPerfilCsv);
            foreach (var l in lines)
            {
                var cols = l.Split(';');
                if (cols[0].Equals(legajo, StringComparison.OrdinalIgnoreCase))
                    return cols[1];
            }
            return null;
        }

        public void ActualizarUsuario(string legajo, string nuevoNombreUsuario)
        {
            var path = Ruta(CredencialesCsv);
            if (!File.Exists(path)) return;

            var lines = File.ReadAllLines(path).ToList();
            for (int i = 1; i < lines.Count; i++) // Saltar cabecera
            {
                var cols = lines[i].Split(';');
                if (cols.Length > 0 && cols[0] == legajo)
                {
                    cols[1] = nuevoNombreUsuario; // Actualizar el nombre de usuario
                    lines[i] = string.Join(";", cols);
                    break;
                }
            }
            File.WriteAllLines(path, lines);
        }
    }
}