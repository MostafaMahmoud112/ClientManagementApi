using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using ClientManagement.Models;

public class StockMarketService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public StockMarketService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public async Task<StockData> GetStockDataAsync(string symbol)
    {
        var apiKey = _configuration["PolygonApiKey"];
        var url = $"https://api.polygon.io/v2/aggs/ticker/{symbol}/range/1/day/2023-01-09/2023-02-10?adjusted=true&sort=asc&apiKey={apiKey}";

        var responseString = await _httpClient.GetStringAsync(url);
        var stockData = JsonSerializer.Deserialize<StockData>(responseString, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true 
        });

        return stockData;
    }
}
