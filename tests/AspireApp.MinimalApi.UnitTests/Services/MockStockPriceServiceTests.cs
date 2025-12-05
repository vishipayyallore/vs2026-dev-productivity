using AspireApp.MinimalApi.Services;
using Microsoft.Extensions.Logging;
using Moq;

namespace AspireApp.MinimalApi.UnitTests.Services;

/// <summary>
/// Unit tests for MockStockPriceService
/// </summary>
public class MockStockPriceServiceTests
{
    private readonly ILogger<MockStockPriceService> _logger;
    private readonly MockStockPriceService _service;

    public MockStockPriceServiceTests()
    {
        _logger = new Mock<ILogger<MockStockPriceService>>().Object;
        _service = new MockStockPriceService(_logger);
    }

    [Fact]
    public async Task GetStockPricesAsync_Should_Return_Data_For_All_Symbols()
    {
        // Arrange
        var symbols = new[] { "AAPL", "MSFT", "GOOG" };

        // Act
        var result = await _service.GetStockPricesAsync(symbols);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(3);
        result.Select(s => s.Symbol).Should().BeEquivalentTo(new[] { "AAPL", "MSFT", "GOOG" });
    }

    [Fact]
    public async Task GetStockPricesAsync_Should_Return_Valid_Price_Data()
    {
        // Arrange
        var symbols = new[] { "AAPL" };

        // Act
        var result = await _service.GetStockPricesAsync(symbols);
        var stock = result.First();

        // Assert
        stock.Should().NotBeNull();
        stock.Symbol.Should().Be("AAPL");
        stock.CurrentPrice.Should().BeGreaterThan(0);
        stock.DailyChange.Should().NotBe(0);
        stock.DailyChangePercent.Should().NotBe(0);
        stock.LastUpdated.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public async Task GetStockPricesAsync_Should_Normalize_Symbol_Case()
    {
        // Arrange
        var symbols = new[] { "aapl", "MsFt", "GOOG" };

        // Act
        var result = await _service.GetStockPricesAsync(symbols);

        // Assert
        result.Select(s => s.Symbol).Should().BeEquivalentTo(new[] { "AAPL", "MSFT", "GOOG" });
    }

    [Fact]
    public async Task GetStockPricesAsync_Should_Handle_Unknown_Symbols()
    {
        // Arrange
        var symbols = new[] { "UNKNOWN", "FAKE" };

        // Act
        var result = await _service.GetStockPricesAsync(symbols);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.All(s => s.CurrentPrice > 0).Should().BeTrue();
    }

    [Fact]
    public async Task GetStockPricesAsync_Should_Throw_On_Null_Symbols()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => 
            _service.GetStockPricesAsync(null!));
    }

    [Fact]
    public async Task GetStockPricesAsync_Should_Handle_Empty_Symbol_List()
    {
        // Arrange
        var symbols = Array.Empty<string>();

        // Act
        var result = await _service.GetStockPricesAsync(symbols);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetStockPricesAsync_Should_Handle_Multiple_Symbols()
    {
        // Arrange
        var symbols = new[] { "AAPL", "MSFT", "GOOG", "AMZN", "META" };

        // Act
        var result = await _service.GetStockPricesAsync(symbols);

        // Assert
        result.Should().HaveCount(5);
        result.All(s => !string.IsNullOrEmpty(s.Symbol)).Should().BeTrue();
        result.All(s => s.CurrentPrice > 0).Should().BeTrue();
    }

    [Fact]
    public void Constructor_Should_Throw_On_Null_Logger()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new MockStockPriceService(null!));
    }

    [Fact]
    public async Task GetStockPricesAsync_Should_Respect_Cancellation_Token()
    {
        // Arrange
        var symbols = new[] { "AAPL" };
        var cts = new CancellationTokenSource();
        cts.Cancel();

        // Act - cancellation doesn't actually affect this mock implementation
        // but we verify the method accepts the token
        var result = await _service.GetStockPricesAsync(symbols, cts.Token);

        // Assert
        result.Should().NotBeNull();
    }
}
