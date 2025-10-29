using AspireApp.ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

// Add Aspire service defaults (includes service discovery)
builder.AddServiceDefaults();

// Configure HttpClient with service discovery for YARP
builder.Services.ConfigureHttpClientDefaults(http =>
{
    http.AddStandardResilienceHandler();
    http.AddServiceDiscovery();
});

// Configure YARP reverse proxy
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"))
    .AddServiceDiscoveryDestinationResolver();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

// Map Aspire default endpoints
app.MapDefaultEndpoints();

// Use YARP reverse proxy
app.MapReverseProxy();

// Health check endpoint
app.MapGet("/", () => new
{
    Service = "Aspire.ApiGateway",
    Status = "Running",
    Timestamp = DateTime.UtcNow
});

await app.RunAsync().ConfigureAwait(false);
