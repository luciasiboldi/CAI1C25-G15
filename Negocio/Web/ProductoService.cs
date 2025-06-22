using System.Collections.Generic;
using System.Threading.Tasks;
using Datos.Web;

namespace Negocio.Web
{
    public class ProductoService
    {
        public static async Task<List<Producto>> TraerTodosLosProductos()
        {
            return await ApiClient.GetAsync<List<Producto>>("api/productos");
        }
    }
}