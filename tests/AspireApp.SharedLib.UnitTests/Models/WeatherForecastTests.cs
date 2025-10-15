using AspireApp.SharedLib.Models;
using FluentAssertions;

namespace AspireApp.SharedLib.UnitTests.Models;

/// <summary>
/// Unit tests for the WeatherForecast model
/// </summary>
public class WeatherForecastTests
{
    [Fact]
    public void WeatherForecast_Should_Initialize_With_Default_Constructor()
    {
        // Arrange & Act
        var forecast = new WeatherForecast();

        // Assert
        forecast.Date.Should().Be(default(DateOnly));
        forecast.TemperatureC.Should().Be(0);
        forecast.Summary.Should().BeNull();
        forecast.TemperatureF.Should().Be(32); // 0°C = 32°F
    }

    [Fact]
    public void WeatherForecast_Should_Initialize_With_Parameterized_Constructor()
    {
        // Arrange
        var date = new DateOnly(2024, 10, 15);
        var temperatureC = 25;
        var summary = "Sunny";

        // Act
        var forecast = new WeatherForecast(date, temperatureC, summary);

        // Assert
        forecast.Date.Should().Be(date);
        forecast.TemperatureC.Should().Be(temperatureC);
        forecast.Summary.Should().Be(summary);
    }

    [Theory]
    [InlineData(0, 32)]
    [InlineData(10, 50)]
    [InlineData(20, 68)]
    [InlineData(25, 77)]
    [InlineData(30, 86)]
    [InlineData(100, 212)]
    [InlineData(-10, 14)]
    [InlineData(-20, -4)]
    public void TemperatureF_Should_Calculate_Correctly_From_Celsius(int celsius, int expectedFahrenheit)
    {
        // Arrange & Act
        var forecast = new WeatherForecast
        {
            TemperatureC = celsius
        };

        // Assert
        forecast.TemperatureF.Should().Be(expectedFahrenheit);
    }

    [Fact]
    public void WeatherForecast_Should_Set_Properties_Correctly()
    {
        // Arrange
        var date = new DateOnly(2024, 12, 25);
        var temperatureC = -5;
        var summary = "Snowy";

        // Act
        var forecast = new WeatherForecast
        {
            Date = date,
            TemperatureC = temperatureC,
            Summary = summary
        };

        // Assert
        forecast.Date.Should().Be(date);
        forecast.TemperatureC.Should().Be(temperatureC);
        forecast.Summary.Should().Be(summary);
        forecast.TemperatureF.Should().Be(23); // -5°C = 23°F
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("Sunny")]
    [InlineData("Partly Cloudy")]
    [InlineData("Rainy")]
    [InlineData("Stormy")]
    [InlineData("Very long weather description with special characters !@#$%")]
    public void WeatherForecast_Should_Accept_Various_Summary_Values(string? summary)
    {
        // Arrange & Act
        var forecast = new WeatherForecast
        {
            Summary = summary
        };

        // Assert
        forecast.Summary.Should().Be(summary);
    }

    [Fact]
    public void WeatherForecast_Should_Handle_Extreme_Temperatures()
    {
        // Arrange & Act
        var hotForecast = new WeatherForecast { TemperatureC = 50 }; // Very hot
        var coldForecast = new WeatherForecast { TemperatureC = -40 }; // Very cold

        // Assert
        hotForecast.TemperatureF.Should().Be(122);
        coldForecast.TemperatureF.Should().Be(-40); // -40°C = -40°F (special case)
    }

    [Fact]
    public void WeatherForecast_Parameterized_Constructor_Should_Handle_Null_Summary()
    {
        // Arrange
        var date = new DateOnly(2024, 6, 15);
        var temperatureC = 22;

        // Act
        var forecast = new WeatherForecast(date, temperatureC, null);

        // Assert
        forecast.Date.Should().Be(date);
        forecast.TemperatureC.Should().Be(temperatureC);
        forecast.Summary.Should().BeNull();
    }

    [Fact]
    public void WeatherForecast_Should_Handle_DateOnly_Boundaries()
    {
        // Arrange
        var minDate = DateOnly.MinValue;
        var maxDate = DateOnly.MaxValue;

        // Act
        var minForecast = new WeatherForecast { Date = minDate };
        var maxForecast = new WeatherForecast { Date = maxDate };

        // Assert
        minForecast.Date.Should().Be(minDate);
        maxForecast.Date.Should().Be(maxDate);
    }
}