using System;
using System.Collections.Generic;

namespace Datos.Web
{
    public class Venta
    {
        public Guid IdCliente { get; set; }
        public List<VentaProducto> Productos { get; set; }
        public Guid IdUsuario { get; set; }
    }
}