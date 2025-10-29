using AspireApp.BlazorWeb.Components;
using AspireApp.ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

// Add Aspire service defaults
builder.AddServiceDefaults();

// Add services to the container
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Configure HttpClient for API Gateway communication using Aspire service discovery
builder.Services.AddHttpClient("ApiGateway", client =>
{
    client.BaseAddress = new Uri("http://aspireapp-apigateway");
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
