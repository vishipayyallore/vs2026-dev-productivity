using Microsoft.EntityFrameworkCore;
using Aspire.MinimalApi.Data;
using Aspire.MinimalApi.Endpoints;
using AspireApp.SharedLib.Extensions;
using AspireApp.ServiceDefaults;
/// <summary>
/// Weather forecast data model
/// </summary>
record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
