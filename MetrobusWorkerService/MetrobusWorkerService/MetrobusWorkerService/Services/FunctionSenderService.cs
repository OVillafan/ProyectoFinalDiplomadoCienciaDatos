using System.Text;
using System.Text.Json;
using TransitRealtime;

namespace MetrobusWorkerService.Services;
public class FunctionSenderService : IFunctionSenderService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;

    public FunctionSenderService(HttpClient httpClient, IConfiguration config)
    {
        _httpClient = httpClient;
        _config = config;
    }

    public async Task SendToFunctionAsync(FeedMessage feed)
    {
        var json = JsonSerializer.Serialize(feed.Entity.First());

        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // URL completa con clave de función incluida
        var functionUrl = Environment.GetEnvironmentVariable("FUNC_KEY");

        var response = await _httpClient.PostAsync(functionUrl, content);

        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine($"Error enviando a la Azure Function: {response.StatusCode}");
        }
        else
        {
            Console.WriteLine("Datos enviados exitosamente a Azure Function.");
        }
    }
}
