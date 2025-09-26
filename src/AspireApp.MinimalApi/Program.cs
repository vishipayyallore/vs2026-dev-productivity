using Microsoft.EntityFrameworkCore;
using Aspire.MinimalApi.Data;
using Aspire.MinimalApi.Endpoints;
using AspireApp.SharedLib.Extensions;
using AspireApp.ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

// Add Aspire service defaults (logging, metrics, health checks, etc.)
builder.AddServiceDefaults();

// Add PostgreSQL with Entity Framework Core
builder.AddNpgsqlDbContext<ApplicationDbContext>("productdb");

// Add services to the container
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add shared services
builder.Services.AddSharedServices();

// Add problem details for better error handling
builder.Services.AddProblemDetails();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Use problem details middleware for better error handling
app.UseExceptionHandler();

app.UseHttpsRedirection();

// Map Aspire default endpoints (health checks, etc.)
app.MapDefaultEndpoints();

// Map product endpoints
app.MapProductEndpoints();

// Simple health check endpoint
app.MapGet("/", () => new { 
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
    var summaries = new[] { "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching" };

    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast(
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
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
