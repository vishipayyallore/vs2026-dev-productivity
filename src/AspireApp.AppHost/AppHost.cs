var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.AspireApp_BlazorWeb>("aspireapp-blazorweb");

await builder.Build().RunAsync().ConfigureAwait(false);
