using Datos.Web;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace Persistencia
{
    public class ProductoPersistencia
    {
        private static readonly string baseUrl = "https://cai-tp.azurewebsites.net";

        public static List<Producto> TraerTodosLosProductos()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);
                var response = client.GetAsync("/api/Producto/TraerProductos").Result;
                var json = response.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<List<Producto>>(json);
            }
        }
    }
}