using System;

namespace Datos.Web
{
    public class Cliente
    {
        public Guid IdCliente { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public long dNI { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public DateTime FechaAlta { get; set; }
        public DateTime? FechaBaja { get; set; }
    }
}