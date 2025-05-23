using System;

namespace Datos
{
    public class PerfilRol
    {
        public string IdPerfil { get; set; }
        public string IdRol { get; set; }

        public PerfilRol(string registroCsv)
        {
            // Formato: idPerfil;idRol
            var cols = registroCsv.Split(';');
            IdPerfil = cols[0];
            IdRol = cols[1];
        }
    }
}
