using System;
using System.IO;
using System.Linq;

namespace Persistencia
{
    public class PerfilPersistencia
    {
        private readonly string _baseDir = AppDomain.CurrentDomain.BaseDirectory;
        private const string UsuarioPerfilCsv = "usuario_perfil.csv";
        private const string PerfilCsv = "perfil.csv";
        private const string RolCsv = "rol.csv";
        private const string PerfilRolCsv = "perfil_rol.csv";

        private string Ruta(string fileName) => Path.Combine(_baseDir, fileName);

       
        public string ObtenerIdPerfil(string legajo)
        {
            var path = Ruta(UsuarioPerfilCsv);
            if (!File.Exists(path)) return null;

            return File.ReadAllLines(path)
                       .Skip(1)                                    // <-- Saltar la línea "legajo;idPerfil"
                       .Where(l => !string.IsNullOrWhiteSpace(l))
                       .Select(l => l.Split(';'))
                       .Where(cols => cols[0]
                             .Equals(legajo, StringComparison.OrdinalIgnoreCase))
                       .Select(cols => cols[1])
                       .FirstOrDefault();
        }

       
        public string[] ObtenerRolesPorPerfil(string idPerfil)
        {
            var rolesMap = File.ReadAllLines(Ruta(RolCsv))
                               .Skip(1)  // skip header
                               .Select(l => l.Split(';'))
                               .ToDictionary(c => c[0], c => c[1]);

            var rolIds = File.ReadAllLines(Ruta(PerfilRolCsv))
                             .Skip(1)
                             .Select(l => l.Split(';'))
                             .Where(c => c[0] == idPerfil)
                             .Select(c => c[1]);

            return rolIds.Where(rolesMap.ContainsKey)
                         .Select(id => rolesMap[id])
                         .ToArray();
        }
    }
}