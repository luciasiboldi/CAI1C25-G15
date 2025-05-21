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
            return File.ReadAllLines(ruta)
                       .Skip(1)                                // salta header
                       .Where(l => !string.IsNullOrWhiteSpace(l));
        }

        private void AgregarLinea(string file, string linea)
        {
            var ruta = Ruta(file);
            File.AppendAllText(ruta, linea + Environment.NewLine);
        }

        public Credencial ObtenerCredencial(string usuario)
        {
            var fila = LeerTodas(CredencialesCsv)
                      .Select(l => new Credencial(l))
                      .FirstOrDefault(c => c.NombreUsuario.Equals(usuario, StringComparison.OrdinalIgnoreCase));
            return fila;
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
    }
}