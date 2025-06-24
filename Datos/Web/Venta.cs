using System;
using System.Collections.Generic;

namespace Datos.Web
{
    public class Venta
    {
        public Guid IdCliente { get; set; }
        public Guid IdUsuario { get; set; }
        public Guid IdProducto { get; set; }
        public int Cantidad { get; set; }
    }
}