using AspireApp.SharedLib.DTOs;

namespace AspireApp.MinimalApi.Services;

/// <summary>
/// Service interface for retrieving stock price data
/// </summary>
public interface IStockPriceService
{
    /// <summary>
    /// Gets stock price data for the specified symbols
    /// </summary>
    /// <param name="symbols">List of stock symbols to retrieve</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Collection of stock price data</returns>
    Task<IEnumerable<StockPriceDto>> GetStockPricesAsync(IEnumerable<string> symbols, CancellationToken cancellationToken = default);
}
