using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

public class DataFetcher
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;

    public DataFetcher(HttpClient httpClient, IConfiguration config)
    {
        _httpClient = httpClient;
        _config = config;
    }

    public async Task<byte[]> FetchFeedAsync()
    {
        var credentials = new
        {
            user = _config["MetrobusApi:usuario"],
            password = _config["MetrobusApi:senha"]
        };

        var content = new StringContent(JsonSerializer.Serialize(credentials), Encoding.UTF8, "application/json");
        var postResponse = await _httpClient.PostAsync(_config["MetrobusApi:Endpoint"], content);
        postResponse.EnsureSuccessStatusCode();

        var url = await postResponse.Content.ReadAsStringAsync();

        var feedResponse = await _httpClient.GetAsync(url);
        feedResponse.EnsureSuccessStatusCode();

        return await feedResponse.Content.ReadAsByteArrayAsync();
    }
}
