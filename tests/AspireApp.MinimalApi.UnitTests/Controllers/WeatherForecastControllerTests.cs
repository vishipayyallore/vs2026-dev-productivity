using AspireApp.MinimalApi.Controllers;
using AspireApp.SharedLib.Models;

namespace AspireApp.MinimalApi.UnitTests.Controllers;

/// <summary>
/// Unit tests for WeatherForecastController
/// </summary>
public class WeatherForecastControllerTests
{
    private readonly WeatherForecastController _controller;

    public WeatherForecastControllerTests()
    {
        _controller = new WeatherForecastController();
    }

    #region Get Tests

    [Fact]
    public void Get_Should_Return_Five_Forecasts()
    {
        // Act
        var result = _controller.Get();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(5);
    }

    [Fact]
    public void Get_Should_Return_WeatherForecast_Objects()
    {
        // Act
        var result = _controller.Get();

        // Assert
        result.Should().AllBeOfType<WeatherForecast>();
    }

    [Fact]
    public void Get_Should_Return_Future_Dates()
    {
        // Arrange
        var today = DateOnly.FromDateTime(DateTime.Now);

        // Act
        var result = _controller.Get().ToList();

        // Assert
        result.Should().OnlyContain(f => f.Date > today);
    }

    [Fact]
    public void Get_Should_Return_Valid_Temperature_Range()
    {
        // Act
        var result = _controller.Get().ToList();

        // Assert
        result.Should().OnlyContain(f => f.TemperatureC >= -20 && f.TemperatureC < 55);
    }

    [Fact]
    public void Get_Should_Return_Valid_Summaries()
    {
        // Arrange
        var validSummaries = new[] { "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching" };

        // Act
        var result = _controller.Get().ToList();

        // Assert
        result.Should().OnlyContain(f => validSummaries.Contains(f.Summary));
    }

    [Fact]
    public void Get_Should_Have_Sequential_Dates()
    {
        // Act
        var result = _controller.Get().OrderBy(f => f.Date).ToList();

        // Assert
        for (int i = 0; i < result.Count - 1; i++)
        {
            var daysDifference = result[i + 1].Date.DayNumber - result[i].Date.DayNumber;
            daysDifference.Should().Be(1);
        }
    }

    [Fact]
    public void Get_Should_Calculate_TemperatureF_Correctly()
    {
        // Act
        var result = _controller.Get().ToList();

        // Assert
        foreach (var forecast in result)
        {
            var expectedF = 32 + (int)(forecast.TemperatureC * 9.0 / 5.0);
            forecast.TemperatureF.Should().Be(expectedF);
        }
    }

    [Fact]
    public void Get_Multiple_Calls_Should_Return_Different_Results()
    {
        // Act
        var result1 = _controller.Get().ToList();
        var result2 = _controller.Get().ToList();

        // Assert
        // Due to randomization, it's extremely unlikely to get identical results
        var identicalCount = result1.Zip(result2, (f1, f2) =>
            f1.TemperatureC == f2.TemperatureC && f1.Summary == f2.Summary)
            .Count(match => match);

        // At least one should be different
        identicalCount.Should().BeLessThan(5);
    }

    [Fact]
    public void Get_Should_Not_Return_Null_Summaries()
    {
        // Act
        var result = _controller.Get().ToList();

        // Assert
        result.Should().OnlyContain(f => f.Summary != null);
    }

    [Fact]
    public void Get_Should_Start_From_Tomorrow()
    {
        // Arrange
        var tomorrow = DateOnly.FromDateTime(DateTime.Now.AddDays(1));

        // Act
        var result = _controller.Get().OrderBy(f => f.Date).First();

        // Assert
        result.Date.Should().Be(tomorrow);
    }

    #endregion
}
