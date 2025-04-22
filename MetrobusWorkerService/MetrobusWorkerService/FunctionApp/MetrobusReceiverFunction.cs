using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Azure.Storage.Blobs;
using System;
using System.Net;

namespace AzureFunctionMetrobus
{
    public class MetrobusReceiverFunction
    {
        private readonly ILogger _logger;

        public MetrobusReceiverFunction(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<MetrobusReceiverFunction>();
        }

        [Function("SaveMetrobusData")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequestData req)
        {
            _logger.LogInformation("Recibiendo datos del Metrobus.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            // Nombre dinámico del archivo
            string fileName = $"metrobus-{System.DateTime.UtcNow:yyyyMMdd-HHmmss}.json";

            // Leer el string de conexión del Storage desde configuración
            string connectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
            string containerName = "raw-metrobus";

            // Guardar en Blob
            BlobContainerClient container = new BlobContainerClient(connectionString, containerName);
            await container.CreateIfNotExistsAsync();
            BlobClient blob = container.GetBlobClient(fileName);
            var ms = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(requestBody));
            await blob.UploadAsync(ms);

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.WriteString($"Archivo guardado como {fileName}");

            return response;
        }
    }
}
