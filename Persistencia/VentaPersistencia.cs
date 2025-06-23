using Datos.Ventas;
using Newtonsoft.Json;
using System;
using System.Net.Http;

namespace Persistencia
{
    public class VentaPersistencia
    {
        private readonly Guid idUsuario = new Guid("784c07f2-2b26-4973-9235-4064e94832b5");

        public string agregarVenta(RegistrarVenta registrarVenta)
        {
            // El usuario no lo elige, lo pone el sistema
            registrarVenta.IdUsuario = idUsuario;

            // CamelCase para que lo entienda el WebService
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver()
            };

            var jsonRequest = JsonConvert.SerializeObject(cargaVenta, settings);

            // Solo para debug, ver el JSON en el Output
            System.Diagnostics.Debug.WriteLine("JSON enviado:\n" + jsonRequest);

            HttpResponseMessage response = WebHelper.Post("/api/Venta/AgregarVenta", jsonRequest);
            string contenidoRespuesta = response.Content.ReadAsStringAsync().Result;

            if (!response.IsSuccessStatusCode)
            {
                return "ERROR\n" + contenidoRespuesta + "\n\nJSON enviado:\n" + jsonRequest;
            }

            return contenidoRespuesta;
        }
    }
}