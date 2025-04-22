using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.Storage;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Azure.Storage.Blobs;
using System;
using Microsoft.Azure.WebJobs;

namespace AzureFunctionMetrobus
{
    public static class MetrobusReceiverFunction
    {
        [FunctionName("SaveMetrobusData")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequestMessage req, // Cambiado de HttpRequest a HttpRequestMessage
            ILogger log)
        {
            log.LogInformation("Recibiendo datos del Metrobus.");

            string requestBody = await req.Content.ReadAsStringAsync();  // Usamos req.Content en lugar de req.Body

            // Nombre dinámico del archivo
            string fileName = $"metrobus-{System.DateTime.UtcNow:yyyyMMdd-HHmmss}.json";

            // Leer el string de conexión del Storage desde configuración
            string connectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");

            // Nombre del contenedor en Blob Storage
            string containerName = "raw-metrobus";

            // Guardar en Blob
            BlobContainerClient container = new BlobContainerClient(connectionString, containerName);
            await container.CreateIfNotExistsAsync();
            BlobClient blob = container.GetBlobClient(fileName);
            using var ms = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(requestBody));
            await blob.UploadAsync(ms);

            return new HttpResponseMessage(System.Net.HttpStatusCode.OK)
            {
                Content = new StringContent($"Archivo guardado como {fileName}")
            };
        }
    }
}
