var builder = DistributedApplication.CreateBuilder(args);

// Add PostgreSQL database
var postgres = builder.AddPostgres("postgres")
    .WithDataVolume()
    .WithPgAdmin();

var productDb = postgres.AddDatabase("productdb");

// Add the Minimal API project
var api = builder.AddProject<Projects.Aspire_MinimalApi>("aspire-minimalapi")
    .WithReference(productDb);

// Add the API Gateway
var gateway = builder.AddProject<Projects.Aspire_ApiGateway>("aspire-apigateway")
    .WithReference(api);

// Add the Blazor dashboard app
var blazorApp = builder.AddProject<Projects.Aspire_BlazorApp>("aspire-blazorapp")
    .WithReference(gateway);

builder.Build().Run();
