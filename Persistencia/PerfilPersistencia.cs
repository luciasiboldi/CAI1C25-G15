using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Datos;

namespace Persistencia
{
    public class PerfilPersistencia
    {
        private readonly string _baseDir =
            AppDomain.CurrentDomain.BaseDirectory;
        private const string UsuarioPerfilCsv = "usuario_perfil.csv";
        private const string PerfilCsv = "perfil.csv";
        private const string RolCsv = "rol.csv";
        private const string PerfilRolCsv = "perfil_rol.csv";

        private string Ruta(string nombre) =>
            Path.Combine(_baseDir, nombre);

        private IEnumerable<string> Leer(string file)
        {
            var path = Ruta(file);
            if (!File.Exists(path)) return Enumerable.Empty<string>();
            return File.ReadAllLines(path)
                       .Skip(1)                          // saltar header
                       .Where(l => !string.IsNullOrWhiteSpace(l));
        }

        public string ObtenerIdPerfil(string legajo)
        {
            // usuario_perfil.csv: legajo;idPerfil
            return Leer(UsuarioPerfilCsv)
                .Select(l => l.Split(';'))
                .Where(c => c[0].Equals(legajo, StringComparison.OrdinalIgnoreCase))
                .Select(c => c[1])
                .FirstOrDefault();
        }

        public IEnumerable<Rol> ObtenerRolesPorPerfil(string idPerfil)
        {
            // cargar todos los roles
            var roles = Leer(RolCsv).Select(l => new Rol(l)).ToList();
            // cargar mapeos perfil-rol
            var mappings = Leer(PerfilRolCsv).Select(l => new PerfilRol(l));
            // filtrar IdRol para este perfil
            var ids = mappings
                .Where(m => m.IdPerfil == idPerfil)
                .Select(m => m.IdRol);
            // devolver sólo los Rol cuyo IdRol esté en ids
            return roles.Where(r => ids.Contains(r.IdRol));
        }
    }
}
