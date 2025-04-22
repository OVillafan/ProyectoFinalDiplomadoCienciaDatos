using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
//using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Azure.Storage.Blobs;
using Microsoft.Azure.Functions.Worker;
using System;

namespace AzureFunctionMetrobus
{
    public static class MetrobusReceiverFunction
    {
        [FunctionName("SaveMetrobusData")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Recibiendo datos del Metrobus.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            // Nombre dinámico del archivo
            string fileName = $"metrobus-{System.DateTime.UtcNow:yyyyMMdd-HHmmss}.json";

            // Leer el string de conexión del Storage desde configuración
            string connectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
            // Aquí va
            string containerName = "raw-metrobus";

            // Guardar en Blob
            BlobContainerClient container = new BlobContainerClient(connectionString, containerName);
            await container.CreateIfNotExistsAsync();
            BlobClient blob = container.GetBlobClient(fileName);
            var ms = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(requestBody));
            await blob.UploadAsync(ms);

            return new OkObjectResult($"Archivo guardado como {fileName}");
        }
    }
}
