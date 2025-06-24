
using Datos.Ventas;
using Datos.Web;
using System;

namespace Datos.Ventas
{
    public class RegistrarVenta
    {
        public Guid IdCliente { get; set; }
        public Guid IdUsuario { get; set; }  // Este se fija en el código, no lo carga el usuario
        public Guid IdProducto { get; set; }
        public int Cantidad { get; set; }
    }
}