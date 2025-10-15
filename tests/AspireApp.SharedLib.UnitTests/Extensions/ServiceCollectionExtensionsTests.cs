using AspireApp.SharedLib.Extensions;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace AspireApp.SharedLib.UnitTests.Extensions;

/// <summary>
/// Unit tests for ServiceCollectionExtensions
/// </summary>
public class ServiceCollectionExtensionsTests
{
    [Fact]
    public void AddSharedServices_Should_Return_ServiceCollection()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        var result = services.AddSharedServices();

        // Assert
        result.Should().BeSameAs(services);
        result.Should().NotBeNull();
    }

    [Fact]
    public void AddSharedServices_Should_Not_Throw_Exception()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act & Assert
        var act = () => services.AddSharedServices();
        act.Should().NotThrow();
    }

    [Fact]
    public void AddSharedServices_Should_Work_With_Empty_ServiceCollection()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        var result = services.AddSharedServices();

        // Assert
        result.Should().NotBeNull();
        result.Count.Should().Be(0); // Currently no services are added
    }

    [Fact]
    public void AddSharedServices_Should_Work_With_Existing_Services()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddTransient<string>(_ => "test");
        var initialCount = services.Count;

        // Act
        var result = services.AddSharedServices();

        // Assert
        result.Should().NotBeNull();
        result.Count.Should().Be(initialCount); // No additional services currently added
    }

    [Fact]
    public void AddSharedServices_Should_Be_Chainable()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act & Assert
        var act = () => services
            .AddSharedServices()
            .AddTransient<string>(_ => "test")
            .AddSharedServices();

        act.Should().NotThrow();
    }

    [Fact]
    public void AddSharedServices_Should_Allow_Multiple_Calls()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddSharedServices();
        services.AddSharedServices();
        var result = services.AddSharedServices();

        // Assert
        result.Should().NotBeNull();
        // Multiple calls should not cause issues
    }

    [Fact]
    public void AddSharedServices_Should_Build_ServiceProvider_Successfully()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddSharedServices();
        var act = () => services.BuildServiceProvider();

        // Assert
        act.Should().NotThrow();
        using var serviceProvider = services.BuildServiceProvider();
        serviceProvider.Should().NotBeNull();
    }
}