var builder = DistributedApplication.CreateBuilder(args);

// Add PostgreSQL database with named volume for persistence
var postgres = builder.AddPostgres("postgres")
    .WithDataVolume("aspireapp-postgres-data")
    .WithImage("postgres:16")
    .WithPgAdmin();

var productDb = postgres.AddDatabase("productdb");

// Add the Minimal API project
var api = builder.AddProject<Projects.AspireApp_MinimalApi>("aspireapp-minimalapi")
    .WithReference(productDb);

// Add the API Gateway
var gateway = builder.AddProject<Projects.AspireApp_ApiGateway>("aspireapp-apigateway")
    .WithReference(api);

// Add the Blazor dashboard app
_ = builder.AddProject<Projects.AspireApp_BlazorWeb>("aspireapp-blazorweb")
    .WithReference(gateway);

await builder.Build().RunAsync().ConfigureAwait(false);
