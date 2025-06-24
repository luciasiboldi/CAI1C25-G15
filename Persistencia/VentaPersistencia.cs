using Datos.Ventas;
using Newtonsoft.Json;
using Persistencia.WebService.Utils;
using System;
using System.Net.Http;

namespace Persistencia
{
    public class VentaPersistencia
    {
        private readonly Guid idUsuario = new Guid("784c07f2-2b26-4973-9235-4064e94832b5");
        private readonly Guid idClientePrueba = new Guid("d2541fe1-681c-426d-bb87-05479efdf51f");

        public string agregarVenta(RegistrarVenta original)
        {
            try
            {
                // Crear SIEMPRE un nuevo objeto con los valores correctos
                var registrarVenta = new RegistrarVenta
                {
                    IdCliente = idClientePrueba, // El de prueba
                    IdUsuario = idUsuario, // El tuyo
                    IdProducto = original.IdProducto,
                    Cantidad = original.Cantidad
                };

                var settings = new JsonSerializerSettings
                {
                    ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver()
                };

                var jsonRequest = JsonConvert.SerializeObject(registrarVenta, settings);

                string endpoint = "/api/Venta/AgregarVenta";
                var uri = "https://cai-tp.azurewebsites.net" + endpoint;

                HttpResponseMessage response = WebHelper.Post(endpoint, jsonRequest);
                string contenidoRespuesta = response.Content.ReadAsStringAsync().Result;

                string debugInfo = $"URL: {uri}\n\n" +
                                   $"JSON enviado:\n{jsonRequest}\n\n" +
                                   $"IdCliente usado: {registrarVenta.IdCliente}\n" +
                                   $"IdUsuario usado: {registrarVenta.IdUsuario}\n" +
                                   $"Status Code: {response.StatusCode}\n\n" +
                                   $"Respuesta:\n{contenidoRespuesta}";

                if (!response.IsSuccessStatusCode)
                {
                    return "ERROR\n" + debugInfo;
                }

                return "ÉXITO\n" + debugInfo;
            }
            catch (Exception ex)
            {
                return $"ERROR - EXCEPCIÓN\nMensaje: {ex.Message}\nStackTrace: {ex.StackTrace}";
            }
        }
    }
}