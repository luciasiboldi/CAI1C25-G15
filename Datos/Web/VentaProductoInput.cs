using System;

namespace Datos.Web
{
    public class VentaProductoInput
    {
        public Guid IdCliente { get; set; }
        public Guid IdUsuario { get; set; }
        public Guid IdProducto { get; set; }
        public int Cantidad { get; set; }
    }
}