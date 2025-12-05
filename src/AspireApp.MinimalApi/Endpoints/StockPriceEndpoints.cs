using AspireApp.MinimalApi.Services;

namespace AspireApp.MinimalApi.Endpoints;

/// <summary>
/// Stock Price API endpoints
/// </summary>
public static class StockPriceEndpoints
{
    /// <summary>
    /// Maps stock price endpoints to the application
    /// </summary>
    public static void MapStockPriceEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/stocks")
                       .WithTags("Stocks");

        // GET /api/stocks?symbols=AAPL,MSFT,GOOG
        group.MapGet("", GetStockPrices)
             .WithName("GetStockPrices")
             .WithSummary("Get stock prices")
             .WithDescription("Retrieve current stock prices for the specified symbols");
    }

    /// <summary>
    /// Get stock prices for specified symbols
    /// </summary>
    private static async Task<IResult> GetStockPrices(
        IStockPriceService stockPriceService,
        string? symbols,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(symbols))
        {
            return Results.BadRequest(new { Error = "At least one stock symbol must be provided" });
        }

        try
        {
            var symbolList = symbols.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            
            if (symbolList.Length == 0)
            {
                return Results.BadRequest(new { Error = "At least one valid stock symbol must be provided" });
            }

            var stockPrices = await stockPriceService.GetStockPricesAsync(symbolList, cancellationToken).ConfigureAwait(false);
            return Results.Ok(stockPrices);
        }
        catch (OperationCanceledException)
        {
            return Results.StatusCode(499); // Client Closed Request
        }
        catch (Exception ex)
        {
            return Results.Problem(
                title: "Stock data temporarily unavailable",
                detail: ex.Message,
                statusCode: StatusCodes.Status503ServiceUnavailable);
        }
    }
}
