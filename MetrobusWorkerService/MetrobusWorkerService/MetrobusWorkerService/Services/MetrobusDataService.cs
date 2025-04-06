using System.Text.Json;
using System.Text;
using TransitRealtime;

public class MetrobusDataService : IMetrobusDataService
{
    private readonly HttpClient _httpClient;
    private readonly string _authUrl;
    private readonly string _user;
    private readonly string _password;

    public MetrobusDataService(HttpClient httpClient, IConfiguration config)
    {
        _httpClient = httpClient;
        _authUrl = config["MetrobusApi:Endpoint"];
        _user = config["MetrobusApi:usuario"];
        _password = config["MetrobusApi:senha"];
    }

    public async Task<FeedMessage?> GetFeedMessageAsync()
    {
        // 1. Obtener URL firmada
        var content = new StringContent(JsonSerializer.Serialize(new
        {
            usuario = _user,
            senha = _password
        }), Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync(_authUrl, content);

        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine("Error autenticando: " + response.StatusCode);
            return null;
        }

        var json = await response.Content.ReadAsStringAsync();
        using var jsonDoc = JsonDocument.Parse(json);
        var downloadUrl = jsonDoc.RootElement.GetProperty("urlRealTime").GetString(); 

        if (string.IsNullOrEmpty(downloadUrl))
        {
            Console.WriteLine("No se recibió una URL válida.");
            return null;
        }

        // 2. Descargar archivo binario GTFS-RT
        var protobufBytes = await _httpClient.GetByteArrayAsync(downloadUrl);

        // 3. Parsear con Protobuf
        var feedMessage = FeedMessage.Parser.ParseFrom(protobufBytes);
        return feedMessage;
    }
}
