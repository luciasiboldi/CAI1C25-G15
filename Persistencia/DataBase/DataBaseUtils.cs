using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Persistencia.DataBase
{
    public class DataBaseUtils
    {
        private string GetTablePath(string nombreArchivo)
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string solutionRoot = Path.GetFullPath(Path.Combine(baseDirectory, @"..\..\.."));
            return Path.Combine(solutionRoot, "Persistencia", "DataBase", "Tablas", nombreArchivo);
        }

        public List<string> BuscarRegistro(string nombreArchivo)
        {
            string rutaArchivo = GetTablePath(nombreArchivo);
            var listado = new List<string>();

            try
            {
                using (StreamReader sr = new StreamReader(rutaArchivo))
                {
                    string linea;
                    while ((linea = sr.ReadLine()) != null)
                    {
                        listado.Add(linea);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("No se pudo leer el archivo:");
                Console.WriteLine(e.Message);
            }
            return listado;
        }

        public void BorrarRegistro(string id, string nombreArchivo)
        {
            string rutaArchivo = GetTablePath(nombreArchivo);

            if (!File.Exists(rutaArchivo))
            {
                Console.WriteLine("El archivo no existe: " + rutaArchivo);
                return;
            }

            try
            {
                var listado = File.ReadAllLines(rutaArchivo).ToList();

                var registrosRestantes = listado.Where(linea =>
                {
                    if (string.IsNullOrEmpty(linea)) return false;
                    var campos = linea.Split(';');
                    return campos.Length > 0 && campos[0] != id;
                }).ToList();

                File.WriteAllLines(rutaArchivo, registrosRestantes);

                Console.WriteLine($"Registro con ID {id} borrado correctamente.");
            }
            catch (Exception e)
            {
                Console.WriteLine("Error al intentar borrar el registro:");
                Console.WriteLine($"Mensaje: {e.Message}");
                Console.WriteLine($"Pila de errores: {e.StackTrace}");
            }
        }

        public void AgregarRegistro(string nombreArchivo, string nuevoRegistro)
        {
            string rutaArchivo = GetTablePath(nombreArchivo);

            try
            {
                using (StreamWriter sw = new StreamWriter(rutaArchivo, append: true))
                {
                    sw.WriteLine(nuevoRegistro);
                }

                Console.WriteLine("Registro agregado correctamente.");
            }
            catch (Exception e)
            {
                Console.WriteLine("Error al intentar agregar el registro:");
                Console.WriteLine($"Mensaje: {e.Message}");
                Console.WriteLine($"Pila de errores: {e.StackTrace}");
            }
        }
    }
}
