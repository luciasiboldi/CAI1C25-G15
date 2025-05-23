using System;

namespace Datos
{
    public class Perfil
    {
        public string IdPerfil { get; set; }
        public string NombrePerfil { get; set; }

        public Perfil(string registroCsv)
        {
            // Formato: idPerfil;nombrePerfil
            var cols = registroCsv.Split(';');
            IdPerfil = cols[0];
            NombrePerfil = cols[1];
        }
    }
}
