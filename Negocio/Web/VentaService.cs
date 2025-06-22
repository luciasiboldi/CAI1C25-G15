using System.Collections.Generic;
using System.Threading.Tasks;
using Datos.Web;

namespace Negocio.Web
{
    public class VentaService
    {
        public static async Task<bool> AgregarVenta(Venta venta)
        {
            var response = await ApiClient.PostAsync("api/Venta/AgregarVenta", venta);
            return response.IsSuccessStatusCode;
        }
    }
}