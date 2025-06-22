using System;

namespace Datos.Web
{
    public class Producto
    {
        public Guid Id { get; set; }
        public int IdCategoria { get; set; }
        public string Nombre { get; set; }
        public double Precio { get; set; }
        public int Stock { get; set; }
        public DateTime FechaAlta { get; set; }
        public DateTime? FechaBaja { get; set; }

        public override string ToString()
        {
            return $"{Nombre} - ${Precio}";
        }
    }
}