using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Datos.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Negocio.Web
{
    public static class ApiClient
    {
        private static readonly HttpClient client;

        static ApiClient()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://cai-tp.azurewebsites.net/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        private static async Task<T> GetAsync<T>(string path)
        {
            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                string json_response = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(json_response);
            }
            else
            {
                throw new Exception($"Error al llamar a la API: {response.ReasonPhrase}");
            }
        }

        private static async Task<HttpResponseMessage> PostAsync<T>(string path, T data)
        {
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                DefaultValueHandling = DefaultValueHandling.Ignore
            };
            var json = JsonConvert.SerializeObject(data, settings);

            // --- DEBUG: Imprimir el JSON que se envía ---
            Console.WriteLine("Enviando JSON a la API:");
            Console.WriteLine(json);
            // --- FIN DEBUG ---

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync(path, content);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Error al llamar a la API: {response.ReasonPhrase}");
            }
            return response;
        }

        // Métodos de Cliente
        public static async Task<List<Cliente>> GetClientes()
        {
            return await GetAsync<List<Cliente>>("api/Cliente/GetClientes");
        }

        public static async Task<Cliente> GetCliente(Guid id)
        {
            return await GetAsync<Cliente>($"api/Cliente/GetCliente?id={id}");
        }

        public static async Task<bool> AgregarCliente(Cliente cliente)
        {
            var response = await PostAsync("api/Cliente/AgregarCliente", cliente);
            return response.IsSuccessStatusCode;
        }

        // Métodos de Producto
        public static async Task<List<Producto>> TraerTodosLosProductos()
        {
            return await GetAsync<List<Producto>>("api/Producto/TraerProductos");
        }

        public static async Task<bool> AgregarProducto(Producto producto)
        {
            var response = await PostAsync("api/Producto/AgregarProducto", producto);
            return response.IsSuccessStatusCode;
        }

        // Métodos de Categoría
        public static async Task<List<Categoria>> GetCategorias()
        {
            return await GetAsync<List<Categoria>>("api/Categoria/GetCategorias");
        }

        // Métodos de Venta
        public static async Task<bool> AgregarVenta(VentaProductoInput venta)
        {
            var response = await PostAsync("api/Venta/AgregarVenta", venta);
            return response.IsSuccessStatusCode;
        }
    }
}