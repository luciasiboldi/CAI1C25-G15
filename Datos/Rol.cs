using System;

namespace Datos
{
    public class Rol
    {
        public string IdRol { get; set; }
        public string Nombre { get; set; }

        public Rol(string registroCsv)
        {
            // Formato: idRol;nombreRol
            var cols = registroCsv.Split(';');
            IdRol = cols[0];
            Nombre = cols[1];
        }
    }
}
