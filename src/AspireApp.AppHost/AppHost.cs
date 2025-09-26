var builder = DistributedApplication.CreateBuilder(args);

// Add PostgreSQL database
var postgres = builder.AddPostgres("postgres")
    .WithDataVolume()
    .WithPgAdmin();

var productDb = postgres.AddDatabase("productdb");

// Add the Minimal API project
var api = builder.AddProject<Projects.AspireApp_MinimalApi>("aspireapp-minimalapi")
    .WithReference(productDb);

// Add the API Gateway
var gateway = builder.AddProject<Projects.AspireApp_ApiGateway>("aspireapp-apigateway")
    .WithReference(api);

// Add the Blazor dashboard app
var blazorApp = builder.AddProject<Projects.AspireApp_BlazorWeb>("aspireapp-blazorweb")
    .WithReference(gateway);

await builder.Build().RunAsync().ConfigureAwait(false);
