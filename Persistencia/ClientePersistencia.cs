using Datos.Web;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace Persistencia
{
    public class ClientePersistencia
    {
        private static readonly string baseUrl = "https://cai-tp.azurewebsites.net";

        public static List<Cliente> GetClientes()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);
                var response = client.GetAsync("/api/Cliente/GetClientes").Result;
                var json = response.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<List<Cliente>>(json);
            }
        }

        public static Cliente GetCliente(Guid id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);
                var response = client.GetAsync($"/api/Cliente/GetCliente?id={id}").Result;
                var json = response.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<Cliente>(json);
            }
        }
    }
}