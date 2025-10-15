namespace AspireApp.SharedLib.Models;

/// <summary>
/// Shared WeatherForecast DTO used by multiple projects.
/// Made public so it can be returned from API controllers.
/// </summary>
public class WeatherForecast
{
    public WeatherForecast()
    {
    }

    public WeatherForecast(DateOnly date, int temperatureC, string? summary)
    {
        Date = date;
        TemperatureC = temperatureC;
        Summary = summary;
    }

    public DateOnly Date { get; set; }

    public int TemperatureC { get; set; }

    public string? Summary { get; set; }

    public int TemperatureF => 32 + (int)(TemperatureC * 9.0 / 5.0);
}
