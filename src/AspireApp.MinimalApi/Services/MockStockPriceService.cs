using System.Security.Cryptography;
using AspireApp.SharedLib.DTOs;

namespace AspireApp.MinimalApi.Services;

/// <summary>
/// Mock implementation of stock price service for development and testing
/// Generates realistic-looking stock data without requiring external API
/// </summary>
public class MockStockPriceService : IStockPriceService
{
    private readonly ILogger<MockStockPriceService> _logger;
    private readonly Dictionary<string, decimal> _baselinePrices = new()
    {
        { "AAPL", 175.50m },
        { "MSFT", 380.25m },
        { "GOOG", 140.75m },
        { "AMZN", 155.00m },
        { "META", 485.50m },
        { "TSLA", 245.00m },
        { "NVDA", 495.75m },
        { "NFLX", 485.25m }
    };

    /// <summary>
    /// Initializes a new instance of the MockStockPriceService
    /// </summary>
    /// <param name="logger">Logger instance</param>
    public MockStockPriceService(ILogger<MockStockPriceService> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Gets mock stock price data for the specified symbols
    /// </summary>
    public Task<IEnumerable<StockPriceDto>> GetStockPricesAsync(IEnumerable<string> symbols, CancellationToken cancellationToken = default)
    {
        if (symbols == null)
        {
            throw new ArgumentNullException(nameof(symbols));
        }

        var symbolsList = symbols.ToList();
        _logger.LogInformation("Retrieving stock prices for {Count} symbols: {Symbols}", 
            symbolsList.Count, string.Join(", ", symbolsList));

        var stockPrices = symbolsList.Select(symbol => GenerateMockStockPrice(symbol)).ToList();

        return Task.FromResult<IEnumerable<StockPriceDto>>(stockPrices);
    }

    private StockPriceDto GenerateMockStockPrice(string symbol)
    {
        var normalizedSymbol = symbol.ToUpperInvariant();
        
        // Use baseline price if known, otherwise generate a random price
        var basePrice = _baselinePrices.TryGetValue(normalizedSymbol, out var price) 
            ? price 
            : GetSecureRandomDecimal(50m, 500m);

        // Generate a realistic daily change (between -5% and +5%)
        var changePercent = GetSecureRandomDecimal(-5m, 5m);
        var dailyChange = basePrice * (changePercent / 100m);
        var currentPrice = Math.Round(basePrice + dailyChange, 2);
        dailyChange = Math.Round(dailyChange, 2);

        return new StockPriceDto(
            Symbol: normalizedSymbol,
            CurrentPrice: currentPrice,
            DailyChange: dailyChange,
            DailyChangePercent: Math.Round(changePercent, 2),
            LastUpdated: DateTime.UtcNow
        );
    }

    private static decimal GetSecureRandomDecimal(decimal minValue, decimal maxValue)
    {
        if (minValue >= maxValue)
        {
            throw new ArgumentException("minValue must be less than maxValue");
        }

        var range = maxValue - minValue;
        var randomValue = RandomNumberGenerator.GetInt32(0, 10000) / 10000m;
        return minValue + (range * randomValue);
    }
}
