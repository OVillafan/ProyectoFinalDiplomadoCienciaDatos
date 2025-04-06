using System.Net.Http;
using System.Threading.Tasks;

public class ProtoFetcher
{
    private readonly HttpClient _httpClient;

    public ProtoFetcher(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task DownloadProtoAsync(string url, string outputPath)
    {
        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var bytes = await response.Content.ReadAsByteArrayAsync();
        await File.WriteAllBytesAsync(outputPath, bytes);
    }
}
