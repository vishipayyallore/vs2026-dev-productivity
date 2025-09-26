var builder = DistributedApplication.CreateBuilder(args);

// Add PostgreSQL database
var postgres = builder.AddPostgres("postgres")
    .WithDataVolume()
    .WithPgAdmin();

var productDb = postgres.AddDatabase("productdb");

// Add the Minimal API project
var api = builder.AddProject("aspire-minimalapi")
    .WithReference(productDb);

// Add the API Gateway
var gateway = builder.AddProject("aspire-apigateway")
    .WithReference(api);

// Add the Blazor dashboard app
var blazorApp = builder.AddProject("aspire-blazorapp")
    .WithReference(gateway);

builder.Build().Run();
