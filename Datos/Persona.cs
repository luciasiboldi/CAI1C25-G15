using System;

namespace Datos
{
    public class Persona
    {
        public string Legajo { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public int DNI { get; set; }
        public DateTime FechaIngreso { get; set; }

        public Persona(string registro)
        {
            var cols = registro.Split(';');
            Legajo = cols[0];
            Nombre = cols[1];
            Apellido = cols[2];
            DNI = int.Parse(cols[3]);
            FechaIngreso = DateTime.Parse(cols[4]);
        }

        public string ToCsv()
        {
            return $"{Legajo};{Nombre};{Apellido};{DNI};{FechaIngreso:d/M/yyyy}";
        }
    }
}