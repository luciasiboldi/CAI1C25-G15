using System.Collections.Generic;
using System.Threading.Tasks;
using Datos.Web;

namespace Negocio.Web
{
    public class ClienteService
    {
        public static async Task<List<Cliente>> GetClientes()
        {
            return await ApiClient.GetAsync<List<Cliente>>("api/Cliente/GetClientes");
        }
    }
}