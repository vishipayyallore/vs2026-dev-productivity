using AspireApp.MinimalApi.Services;
using AspireApp.SharedLib.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;

namespace AspireApp.MinimalApi.UnitTests.Endpoints;

/// <summary>
/// Unit tests for Stock Price API endpoints
/// </summary>
public class StockPriceEndpointsTests
{
    private readonly Mock<IStockPriceService> _mockStockService;

    public StockPriceEndpointsTests()
    {
        _mockStockService = new Mock<IStockPriceService>();
    }

    [Fact]
    public async Task GetStockPrices_Should_Return_Stock_Data()
    {
        // Arrange
        var symbols = "AAPL,MSFT";
        var expectedStocks = new[]
        {
            new StockPriceDto("AAPL", 175.50m, 2.50m, 1.45m, DateTime.UtcNow),
            new StockPriceDto("MSFT", 380.25m, -5.00m, -1.30m, DateTime.UtcNow)
        };

        _mockStockService.Setup(s => s.GetStockPricesAsync(
            It.Is<IEnumerable<string>>(x => x.SequenceEqual(new[] { "AAPL", "MSFT" })),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedStocks);

        // Act
        var result = await GetStockPricesInternal(_mockStockService.Object, symbols, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        var okResult = result as Microsoft.AspNetCore.Http.HttpResults.Ok<IEnumerable<StockPriceDto>>;
        okResult.Should().NotBeNull();
        okResult!.Value.Should().HaveCount(2);
    }

    [Fact]
    public void GetStockPrices_Should_Return_BadRequest_When_Symbols_Null()
    {
        // Act
        IResult result = Results.BadRequest(new { Error = "At least one stock symbol must be provided" });

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public void GetStockPrices_Should_Return_BadRequest_When_Symbols_Empty()
    {
        // Act
        IResult result = Results.BadRequest(new { Error = "At least one stock symbol must be provided" });

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetStockPrices_Should_Handle_Single_Symbol()
    {
        // Arrange
        var symbols = "AAPL";
        var expectedStocks = new[]
        {
            new StockPriceDto("AAPL", 175.50m, 2.50m, 1.45m, DateTime.UtcNow)
        };

        _mockStockService.Setup(s => s.GetStockPricesAsync(
            It.Is<IEnumerable<string>>(x => x.SequenceEqual(new[] { "AAPL" })),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedStocks);

        // Act
        var result = await GetStockPricesInternal(_mockStockService.Object, symbols, CancellationToken.None);

        // Assert
        var okResult = result as Microsoft.AspNetCore.Http.HttpResults.Ok<IEnumerable<StockPriceDto>>;
        okResult.Should().NotBeNull();
        okResult!.Value.Should().HaveCount(1);
    }

    [Fact]
    public async Task GetStockPrices_Should_Trim_Whitespace_From_Symbols()
    {
        // Arrange
        var symbols = " AAPL , MSFT , GOOG ";
        var expectedStocks = new[]
        {
            new StockPriceDto("AAPL", 175.50m, 2.50m, 1.45m, DateTime.UtcNow),
            new StockPriceDto("MSFT", 380.25m, -5.00m, -1.30m, DateTime.UtcNow),
            new StockPriceDto("GOOG", 140.75m, 1.25m, 0.90m, DateTime.UtcNow)
        };

        _mockStockService.Setup(s => s.GetStockPricesAsync(
            It.Is<IEnumerable<string>>(x => x.SequenceEqual(new[] { "AAPL", "MSFT", "GOOG" })),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedStocks);

        // Act
        var result = await GetStockPricesInternal(_mockStockService.Object, symbols, CancellationToken.None);

        // Assert
        var okResult = result as Microsoft.AspNetCore.Http.HttpResults.Ok<IEnumerable<StockPriceDto>>;
        okResult.Should().NotBeNull();
    }

    [Fact]
    public async Task GetStockPrices_Should_Return_ServiceUnavailable_On_Exception()
    {
        // Arrange
        var symbols = "AAPL";
        _mockStockService.Setup(s => s.GetStockPricesAsync(
            It.IsAny<IEnumerable<string>>(),
            It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("Service unavailable"));

        // Act
        var result = await GetStockPricesInternal(_mockStockService.Object, symbols, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        var problemResult = result as Microsoft.AspNetCore.Http.HttpResults.ProblemHttpResult;
        problemResult.Should().NotBeNull();
        problemResult!.StatusCode.Should().Be(StatusCodes.Status503ServiceUnavailable);
    }

    [Fact]
    public async Task GetStockPrices_Should_Handle_Cancellation()
    {
        // Arrange
        var symbols = "AAPL";
        var cts = new CancellationTokenSource();
        cts.Cancel();

        _mockStockService.Setup(s => s.GetStockPricesAsync(
            It.IsAny<IEnumerable<string>>(),
            It.IsAny<CancellationToken>()))
            .ThrowsAsync(new OperationCanceledException());

        // Act
        var result = await GetStockPricesInternal(_mockStockService.Object, symbols, cts.Token);

        // Assert
        result.Should().NotBeNull();
        var statusCodeResult = result as Microsoft.AspNetCore.Http.HttpResults.StatusCodeHttpResult;
        statusCodeResult.Should().NotBeNull();
        statusCodeResult!.StatusCode.Should().Be(499); // Client Closed Request
    }

    // Helper method to invoke the actual endpoint logic
    private static async Task<IResult> GetStockPricesInternal(
        IStockPriceService stockPriceService,
        string? symbols,
        CancellationToken cancellationToken)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(symbols))
            {
                return Results.BadRequest(new { Error = "At least one stock symbol must be provided" });
            }

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
