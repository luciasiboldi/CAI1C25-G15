using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace Persistencia
{
    public class PerfilPersistencia
    {
        private const string UsuarioPerfilCsv = "usuario_perfil.csv";
        private const string PerfilCsv = "perfil.csv";
        private const string RolCsv = "rol.csv";
        private const string PerfilRolCsv = "perfil_rol.csv";

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

        private IEnumerable<string> LeerTodas(string fileName)
        {
            var path = Ruta(fileName);
            if (!File.Exists(path)) return new List<string>();

            var lineas = LeerLineasNoVacias(path);
            return SaltarCabecera(lineas);
        }

        public string ObtenerIdPerfil(string legajo)
        {
            var path = Ruta(UsuarioPerfilCsv);
            if (!File.Exists(path)) return null;

            var lineas = SaltarCabecera(LeerLineasNoVacias(path));

            foreach (var linea in lineas)
            {
                var columnas = linea.Split(';');
                if (columnas.Length > 1 && columnas[0].Equals(legajo, StringComparison.OrdinalIgnoreCase))
                {
                    return columnas[1];
                }
            }
            return null;
        }

        private Dictionary<string, string> MapearRoles()
        {
            var lineasRoles = SaltarCabecera(LeerLineasNoVacias(Ruta(RolCsv)));
            var mapa = new Dictionary<string, string>();
            foreach (var linea in lineasRoles)
            {
                var columnas = linea.Split(';');
                if (columnas.Length > 1)
                {
                    mapa[columnas[0]] = columnas[1];
                }
            }
            return mapa;
        }

        private List<string> ObtenerIdsDeRolPorPerfil(string idPerfil)
        {
            var lineasPerfilRol = SaltarCabecera(LeerLineasNoVacias(Ruta(PerfilRolCsv)));
            var idsRoles = new List<string>();
            foreach (var linea in lineasPerfilRol)
            {
                var columnas = linea.Split(';');
                if (columnas.Length > 1 && columnas[0] == idPerfil)
                {
                    idsRoles.Add(columnas[1]);
                }
            }
            return idsRoles;
        }

        public string[] ObtenerRolesPorPerfil(string idPerfil)
        {
            var rolesMap = MapearRoles();
            var rolIds = ObtenerIdsDeRolPorPerfil(idPerfil);
            var rolesFinales = new List<string>();

            foreach (var id in rolIds)
            {
                if (rolesMap.ContainsKey(id))
                {
                    rolesFinales.Add(rolesMap[id]);
                }
            }
            return rolesFinales.ToArray();
        }

        public List<Datos.Perfil> ObtenerTodosLosPerfiles()
        {
            var lineas = LeerTodas(PerfilCsv);
            var perfiles = new List<Datos.Perfil>();
            foreach (var linea in lineas)
            {
                perfiles.Add(new Datos.Perfil(linea));
            }
            return perfiles;
        }
    }
}