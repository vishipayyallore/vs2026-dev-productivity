using AspireApp.ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

// Add Aspire service defaults
builder.AddServiceDefaults();

// Add YARP reverse proxy and integrate with Aspire service discovery
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

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
