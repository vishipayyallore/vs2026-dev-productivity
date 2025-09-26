using Microsoft.EntityFrameworkCore;
using Aspire.MinimalApi.Data;
using Aspire.MinimalApi.Endpoints;
using AspireApp.SharedLib.Extensions;
using AspireApp.ServiceDefaults;
using AspireApp.SharedLib.Models;
using Microsoft.AspNetCore.Http;
using Scalar.AspNetCore;
using Aspire.MinimalApi; // for DemoHelpers

var builder = WebApplication.CreateBuilder(args);

// Add Aspire service defaults (logging, metrics, health checks, etc.)
builder.AddServiceDefaults();

// Add PostgreSQL with Entity Framework Core
builder.AddNpgsqlDbContext<ApplicationDbContext>("productdb");

// Add services to the container
builder.Services.AddEndpointsApiExplorer();

// Add shared services
builder.Services.AddSharedServices();

// Add problem details for better error handling
builder.Services.AddProblemDetails();

var app = builder.Build();

// Configure the HTTP request pipeline
// Swagger has been removed; user prefers Scalar UI instead

// Serve static files (including optional static assets)
app.UseStaticFiles();

// Register Scalar UI only in Development and when enabled in configuration
var scalarConfig = builder.Configuration.GetSection("Scalar");
if (app.Environment.IsDevelopment() && scalarConfig.GetValue<bool>("Enabled"))
{
    // Configure Scalar UI at /scalar/v1
    app.MapScalarApiReference(options =>
    {
        options.WithTitle(scalarConfig.GetValue<string>("Title") ?? "TraceMind RCA API")
               .WithTheme(ScalarTheme.BluePlanet)
               .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
    });
}

// Use problem details middleware for better error handling
app.UseExceptionHandler();

app.UseHttpsRedirection();

// Map Aspire default endpoints (health checks, etc.)
app.MapDefaultEndpoints();

// Map product endpoints
app.MapProductEndpoints();

// Simple health check endpoint
app.MapGet("/", () => new
{
    Service = "Aspire.MinimalApi",
    Status = "Running",
    Timestamp = DateTime.UtcNow
})
.WithName("GetApiStatus")
.WithSummary("Get API status")
.WithTags("Status");

// Weather forecast endpoint (keep as example)
app.MapGet("/api/weather", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast(
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            DemoHelpers.GetSecureRandomInt(-20, 55),
            DemoHelpers.Summaries[DemoHelpers.GetSecureRandomInt(0, DemoHelpers.Summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithSummary("Get weather forecast")
.WithTags("Weather");

// Apply database migrations on startup in development
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await context.Database.EnsureCreatedAsync().ConfigureAwait(false);
}

await app.RunAsync().ConfigureAwait(false);
