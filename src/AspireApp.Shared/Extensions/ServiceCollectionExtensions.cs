using Microsoft.Extensions.DependencyInjection;

namespace AspireApp.SharedLib.Extensions;

/// <summary>
/// Extensions for configuring shared services
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds common shared services to the dependency injection container
    /// </summary>
    public static IServiceCollection AddSharedServices(this IServiceCollection services)
    {
        // Add any shared services, validators, mappers, etc. here
        return services;
    }
}