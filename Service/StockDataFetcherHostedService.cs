using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ClientManagement.Models;
using ClientManagement;

public class StockDataFetcherHostedService : IHostedService, IDisposable
{
    private Timer _timer;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<StockDataFetcherHostedService> _logger;

    public StockDataFetcherHostedService(IServiceScopeFactory scopeFactory, ILogger<StockDataFetcherHostedService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Stock Data Fetcher Hosted Service is starting.");
        _timer = new Timer(FetchStockData, null, TimeSpan.Zero, TimeSpan.FromHours(6));
        return Task.CompletedTask;
    }

    private async void FetchStockData(object state)
    {
        using (var scope = _scopeFactory.CreateScope())
        {
            var stockMarketService = scope.ServiceProvider.GetRequiredService<StockMarketService>();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            try
            {
                var stockData = await stockMarketService.GetStockDataAsync("AAPL");

                if (stockData != null && stockData.Results != null && stockData.Results.Count > 0)
                {
                    var stockQuotes = stockData.Results.Select(q => new StockQuote
                    {
                        V = q.V,
                        VW = q.VW,
                        O = q.O,
                        C = q.C,
                        H = q.H,
                        L = q.L,
                        T = q.T,
                        N = q.N,
                       RegDate = DateTime.UtcNow
                    }).ToList();

                    dbContext.StockQuotes.AddRange(stockQuotes);
                    await dbContext.SaveChangesAsync();

                    _logger.LogInformation("Stock quotes successfully saved to the database.");
                }
                else
                {
                    _logger.LogWarning("No stock data returned for AAPL.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching stock data.");
            }
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Stock Data Fetcher Hosted Service is stopping.");
        _timer?.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}
