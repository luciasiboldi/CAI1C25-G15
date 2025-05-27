using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Datos;

namespace Persistencia
{
    public class PersonaPersistencia
    {
        private readonly string _baseDir = AppDomain.CurrentDomain.BaseDirectory;
        private const string PersonaCsv = "persona.csv";

        private IEnumerable<string> LeerTodas()
        {
            var path = Path.Combine(_baseDir, PersonaCsv);
            if (!File.Exists(path)) return Enumerable.Empty<string>();
            return File.ReadAllLines(path)
                       .Skip(1)
                       .Where(l => !string.IsNullOrWhiteSpace(l));
        }

        public List<Persona> ObtenerTodas()
            => LeerTodas().Select(l => new Persona(l)).ToList();

        public void ActualizarPersona(Persona p)
        {
            var path = Path.Combine(_baseDir, PersonaCsv);
            var lines = File.ReadAllLines(path).ToList();
            for (int i = 1; i < lines.Count; i++)
            {
                var cols = lines[i].Split(';');
                if (cols[0] == p.Legajo)
                {
                    cols[1] = p.Nombre;
                    cols[2] = p.Apellido;
                    cols[3] = p.DNI.ToString();
                    lines[i] = string.Join(";", cols);
                    break;
                }
            }
            File.WriteAllLines(path, lines);
        }
    }
}