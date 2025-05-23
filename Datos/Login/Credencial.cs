using System;
using System.Globalization;

namespace Datos
{
    public class Credencial
    {
        private string   _legajo;
        private string   _nombreUsuario;
        private string   _contrasena;
        private DateTime _fechaAlta;
        private DateTime _fechaUltimoLogin;

        public string   Legajo            { get => _legajo;            set => _legajo = value; }
        public string   NombreUsuario     { get => _nombreUsuario;     set => _nombreUsuario = value; }
        public string   Contrasena        { get => _contrasena;        set => _contrasena = value; }
        public DateTime FechaAlta         { get => _fechaAlta;         set => _fechaAlta = value; }
        public DateTime FechaUltimoLogin  { get => _fechaUltimoLogin;  set => _fechaUltimoLogin = value; }

        public Credencial(string registro)
        {
            var datos = registro.Split(';');
            
            _legajo           = datos[0];
            _nombreUsuario    = datos[1];
            _contrasena       = datos[2];
            _fechaAlta        = DateTime.ParseExact(datos[3], "d/M/yyyy", CultureInfo.InvariantCulture);
            _fechaUltimoLogin = DateTime.ParseExact(datos[4], "d/M/yyyy", CultureInfo.InvariantCulture);
        }
    }
}
