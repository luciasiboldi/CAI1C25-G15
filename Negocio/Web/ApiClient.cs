using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

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

        public static async Task<T> GetAsync<T>(string path)
        {
            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                string json_response = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<T>(json_response, options);
            }
            else
            {
                throw new Exception($"Error al llamar a la API: {response.ReasonPhrase}");
            }
        }

        public static async Task<HttpResponseMessage> PostAsync<T>(string path, T data)
        {
            var json = JsonSerializer.Serialize(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync(path, content);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Error al llamar a la API: {response.ReasonPhrase}");
            }
            return response;
        }
    }
}