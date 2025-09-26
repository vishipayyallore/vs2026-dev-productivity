var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.AspireApp_BlazorWeb>("aspireapp-blazorweb");

builder.AddProject<Projects.AspireApp_MinimalApi>("aspireapp-minimalapi");

builder.AddProject<Projects.AspireApp_ApiGateway>("aspireapp-apigateway");

await builder.Build().RunAsync().ConfigureAwait(false);
