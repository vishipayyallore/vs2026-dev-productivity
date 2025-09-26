using AspireApp.BlazorWeb.Components;
using AspireApp.ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

// Add Aspire service defaults
builder.AddServiceDefaults();

// Add services to the container
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Configure HttpClient for API Gateway communication
builder.Services.AddHttpClient("ApiGateway", client =>
{
    // Read base address from configuration so the value isn't hard-coded in source
    var baseAddr = builder.Configuration["ApiGateway:BaseAddress"];
    if (!string.IsNullOrWhiteSpace(baseAddr) && Uri.TryCreate(baseAddr, UriKind.Absolute, out var parsed))
    {
        client.BaseAddress = parsed;
    }
    client.Timeout = TimeSpan.FromSeconds(30);
});

// Add the default HttpClient to use the ApiGateway
builder.Services.AddScoped(provider =>
    provider.GetRequiredService<IHttpClientFactory>().CreateClient("ApiGateway"));

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseAntiforgery();

// Map Aspire default endpoints
app.MapDefaultEndpoints();

app.UseStaticFiles();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

await app.RunAsync().ConfigureAwait(false);
