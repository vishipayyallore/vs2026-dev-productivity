using AspireApp.SharedLib.Models;
using FluentAssertions;

namespace AspireApp.SharedLib.UnitTests.Models;

/// <summary>
/// Unit tests for the Product model
/// </summary>
public class ProductTests
{
    [Fact]
    public void Product_Should_Initialize_With_Default_Values()
    {
        // Arrange & Act
        var product = new Product { Name = "Test Product" };

        // Assert
        product.Id.Should().Be(0);
        product.Name.Should().Be("Test Product");
        product.Description.Should().BeNull();
        product.Price.Should().Be(0);
        product.Stock.Should().Be(0);
        product.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        product.UpdatedAt.Should().BeNull();
    }

    [Fact]
    public void Product_Should_Set_All_Properties_Correctly()
    {
        // Arrange
        var now = DateTime.UtcNow;
        var updatedAt = now.AddDays(1);

        // Act
        var product = new Product
        {
            Id = 1,
            Name = "Test Product",
            Description = "Test Description",
            Price = 99.99m,
            Stock = 10,
            CreatedAt = now,
            UpdatedAt = updatedAt
        };

        // Assert
        product.Id.Should().Be(1);
        product.Name.Should().Be("Test Product");
        product.Description.Should().Be("Test Description");
        product.Price.Should().Be(99.99m);
        product.Stock.Should().Be(10);
        product.CreatedAt.Should().Be(now);
        product.UpdatedAt.Should().Be(updatedAt);
    }

    [Theory]
    [InlineData("")]
    [InlineData("Valid Product Name")]
    [InlineData("Product with special chars !@#$%")]
    public void Product_Should_Accept_Valid_Names(string name)
    {
        // Arrange & Act
        var product = new Product { Name = name };

        // Assert
        product.Name.Should().Be(name);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(100)]
    [InlineData(999999)]
    public void Product_Should_Accept_Any_Stock_Value(int stock)
    {
        // Arrange & Act
        var product = new Product
        {
            Name = "Test Product",
            Stock = stock
        };

        // Assert
        product.Stock.Should().Be(stock);
    }

    [Theory]
    [InlineData(0.00)]
    [InlineData(0.01)]
    [InlineData(99.99)]
    [InlineData(999999.99)]
    public void Product_Should_Accept_Valid_Prices(decimal price)
    {
        // Arrange & Act
        var product = new Product
        {
            Name = "Test Product",
            Price = price
        };

        // Assert
        product.Price.Should().Be(price);
    }

    [Fact]
    public void Product_Should_Allow_Null_Description()
    {
        // Arrange & Act
        var product = new Product
        {
            Name = "Test Product",
            Description = null
        };

        // Assert
        product.Description.Should().BeNull();
    }

    [Fact]
    public void Product_Should_Allow_Empty_Description()
    {
        // Arrange & Act
        var product = new Product
        {
            Name = "Test Product",
            Description = ""
        };

        // Assert
        product.Description.Should().BeEmpty();
    }

    [Fact]
    public void Product_CreatedAt_Should_Be_Set_To_Current_Time_By_Default()
    {
        // Arrange
        var beforeCreation = DateTime.UtcNow.AddSeconds(-1);

        // Act
        var product = new Product { Name = "Test Product" };
        var afterCreation = DateTime.UtcNow.AddSeconds(1);

        // Assert
        product.CreatedAt.Should().BeAfter(beforeCreation);
        product.CreatedAt.Should().BeBefore(afterCreation);
    }
}