using Aspire.MinimalApi; // for DemoHelpers
using Aspire.MinimalApi.Data;
using Aspire.MinimalApi.Endpoints;
using AspireApp.MinimalApi;
using AspireApp.MinimalApi.Endpoints;
using AspireApp.MinimalApi.Services;
using AspireApp.ServiceDefaults;
using AspireApp.SharedLib.Extensions;
using AspireApp.SharedLib.Models;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add Aspire service defaults (logging, metrics, health checks, etc.)
builder.AddServiceDefaults();

// Add PostgreSQL with Entity Framework Core
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("productdb")));

// Add services to the container
builder.Services.AddEndpointsApiExplorer();

// Add shared services
builder.Services.AddSharedServices();

// Add stock price service
builder.Services.AddScoped<IStockPriceService, MockStockPriceService>();

// Add problem details for better error handling
builder.Services.AddProblemDetails();

var app = builder.Build();

// Apply pending migrations automatically in Development
// For Production, migrations should be run as part of deployment pipeline
if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await db.Database.MigrateAsync();
    }
}

// Configure the HTTP request pipeline
// Swagger has been removed; user prefers Scalar UI instead

// Log the resolved productdb connection string in Development (masked)
if (app.Environment.IsDevelopment())
{
    try
    {
        var config = app.Configuration;
        var conn = config.GetConnectionString("productdb");
        if (!string.IsNullOrEmpty(conn))
        {
            // Mask password-ish parts for safe logging
            string Mask(string s)
            {
                if (string.IsNullOrEmpty(s)) return s;
                if (s.Length <= 8) return new string('*', s.Length);
                return string.Concat(s.AsSpan(0, 4), new string('*', Math.Min(8, s.Length - 8)), s.AsSpan(s.Length - 4));
            }

            // Attempt to mask the password parameter if present
            var masked = conn;
            try
            {
                var parts = conn.Split(';', StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < parts.Length; i++)
                {
                    var p = parts[i];
                    var idx = p.IndexOf('=', StringComparison.Ordinal);
                    if (idx > 0)
                    {
                        var key = p.AsSpan(0, idx).ToString().Trim();
                        var val = p.AsSpan(idx + 1).ToString();
                        if (key.Equals("Password", StringComparison.OrdinalIgnoreCase) || key.Equals("Pwd", StringComparison.OrdinalIgnoreCase))
                        {
                            parts[i] = key + "=" + Mask(val);
                        }
                    }
                }
                masked = string.Join(';', parts);
            }
            catch (ArgumentException)
            {
                masked = Mask(conn);
            }
            catch (InvalidOperationException)
            {
                masked = Mask(conn);
            }

            var logger = app.Services.GetService<ILogger<Program>>();
            if (logger != null)
            {
                LogMessages.LogConnectionStringResolved(logger, masked);
            }
        }
        else
        {
            var logger = app.Services.GetService<ILogger<Program>>();
            if (logger != null)
            {
                LogMessages.LogConnectionStringNotFound(logger);
            }
        }
    }
    catch (ArgumentException ex)
    {
        var logger = app.Services.GetService<ILogger<Program>>();
        if (logger != null)
        {
            LogMessages.LogConnectionStringError(logger, ex);
        }
    }
    catch (InvalidOperationException ex)
    {
        var logger = app.Services.GetService<ILogger<Program>>();
        if (logger != null)
        {
            LogMessages.LogConnectionStringError(logger, ex);
        }
    }
}

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

// Map hurricane alert endpoints
app.MapHurricaneAlertEndpoints();

// Map stock price endpoints
app.MapStockPriceEndpoints();

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

// Database is set up via migrations - see DATABASE-README.md for setup instructions
// Run: dotnet ef database update --project src/AspireApp.MinimalApi

await app.RunAsync().ConfigureAwait(false);
