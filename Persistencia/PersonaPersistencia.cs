using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Datos;

namespace Persistencia
{
    public class PersonaPersistencia
    {
        private const string PersonaCsv = "persona.csv";

        private string Ruta(string fileName)
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string solutionRoot = Path.GetFullPath(Path.Combine(baseDirectory, @"..\..\.."));
            return Path.Combine(solutionRoot, "Persistencia", "DataBase", "Tablas", fileName);
        }

        private IEnumerable<string> LeerTodas()
        {
            var path = Ruta(PersonaCsv);
            if (!File.Exists(path)) return Enumerable.Empty<string>();
            return File.ReadAllLines(path)
                       .Skip(1)
                       .Where(l => !string.IsNullOrWhiteSpace(l));
        }

        public List<Persona> ObtenerTodas()
        {
            var personas = new List<Persona>();
            var lineas = LeerTodas();
            foreach (var linea in lineas)
            {
                personas.Add(new Persona(linea));
            }
            return personas;
        }

        public void ActualizarPersona(Persona personaActualizada)
        {
            var path = Ruta(PersonaCsv);
            if (!File.Exists(path)) return;

            var lines = File.ReadAllLines(path).ToList();

            // Encuentra la línea para actualizar
            for (int i = 1; i < lines.Count; i++) // Empieza en 1 para saltar cabecera
            {
                var cols = lines[i].Split(';');
                if (cols.Length > 0 && cols[0] == personaActualizada.Legajo)
                {
                    lines[i] = personaActualizada.ToCsv();
                    break;
                }
            }
            File.WriteAllLines(path, lines);
        }
    }
}