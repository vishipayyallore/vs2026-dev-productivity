using Aspire.BlazorApp.Components;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add Aspire service defaults
builder.AddServiceDefaults();

// Add services to the container
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Configure HttpClient for API Gateway communication
builder.Services.AddHttpClient("ApiGateway", client =>
{
    // This will be configured via service discovery in the Host project
    client.BaseAddress = new Uri("http://localhost:5000");
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

app.Run();
