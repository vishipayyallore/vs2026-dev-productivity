var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.AspireApp_BlazorWeb>("aspireapp-blazorweb");

builder.AddProject<Projects.AspireApp_MinimalApi>("aspireapp-minimalapi");

await builder.Build().RunAsync().ConfigureAwait(false);
