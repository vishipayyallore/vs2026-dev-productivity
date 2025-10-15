using AspireApp.SharedLib.DTOs;
using FluentAssertions;

namespace AspireApp.SharedLib.UnitTests.DTOs;

/// <summary>
/// Unit tests for Product DTOs
/// </summary>
public class ProductDtoTests
{
    [Fact]
    public void ProductDto_Should_Initialize_With_All_Properties()
    {
        // Arrange
        var id = 1;
        var name = "Test Product";
        var description = "Test Description";
        var price = 99.99m;
        var stock = 10;
        var createdAt = DateTime.UtcNow;
        var updatedAt = DateTime.UtcNow.AddDays(1);

        // Act
        var dto = new ProductDto(id, name, description, price, stock, createdAt, updatedAt);

        // Assert
        dto.Id.Should().Be(id);
        dto.Name.Should().Be(name);
        dto.Description.Should().Be(description);
        dto.Price.Should().Be(price);
        dto.Stock.Should().Be(stock);
        dto.CreatedAt.Should().Be(createdAt);
        dto.UpdatedAt.Should().Be(updatedAt);
    }

    [Fact]
    public void ProductDto_Should_Handle_Null_Description_And_UpdatedAt()
    {
        // Arrange
        var id = 1;
        var name = "Test Product";
        var price = 99.99m;
        var stock = 10;
        var createdAt = DateTime.UtcNow;

        // Act
        var dto = new ProductDto(id, name, null, price, stock, createdAt, null);

        // Assert
        dto.Description.Should().BeNull();
        dto.UpdatedAt.Should().BeNull();
    }

    [Fact]
    public void ProductDto_Records_Should_Be_Equal_With_Same_Values()
    {
        // Arrange
        var createdAt = DateTime.UtcNow;
        var updatedAt = createdAt.AddDays(1);

        var dto1 = new ProductDto(1, "Product", "Description", 99.99m, 10, createdAt, updatedAt);
        var dto2 = new ProductDto(1, "Product", "Description", 99.99m, 10, createdAt, updatedAt);

        // Act & Assert
        dto1.Should().Be(dto2);
        dto1.GetHashCode().Should().Be(dto2.GetHashCode());
    }

    [Fact]
    public void ProductDto_Records_Should_Not_Be_Equal_With_Different_Values()
    {
        // Arrange
        var createdAt = DateTime.UtcNow;
        var dto1 = new ProductDto(1, "Product", "Description", 99.99m, 10, createdAt, null);
        var dto2 = new ProductDto(2, "Product", "Description", 99.99m, 10, createdAt, null);

        // Act & Assert
        dto1.Should().NotBe(dto2);
    }

    [Fact]
    public void CreateProductDto_Should_Initialize_With_All_Properties()
    {
        // Arrange
        var name = "New Product";
        var description = "New Description";
        var price = 49.99m;
        var stock = 5;

        // Act
        var dto = new CreateProductDto(name, description, price, stock);

        // Assert
        dto.Name.Should().Be(name);
        dto.Description.Should().Be(description);
        dto.Price.Should().Be(price);
        dto.Stock.Should().Be(stock);
    }

    [Fact]
    public void CreateProductDto_Should_Handle_Null_Description()
    {
        // Arrange & Act
        var dto = new CreateProductDto("Product", null, 99.99m, 10);

        // Assert
        dto.Description.Should().BeNull();
    }

    [Fact]
    public void CreateProductDto_Records_Should_Be_Equal_With_Same_Values()
    {
        // Arrange
        var dto1 = new CreateProductDto("Product", "Description", 99.99m, 10);
        var dto2 = new CreateProductDto("Product", "Description", 99.99m, 10);

        // Act & Assert
        dto1.Should().Be(dto2);
        dto1.GetHashCode().Should().Be(dto2.GetHashCode());
    }

    [Fact]
    public void UpdateProductDto_Should_Initialize_With_All_Properties()
    {
        // Arrange
        var name = "Updated Product";
        var description = "Updated Description";
        var price = 149.99m;
        var stock = 15;

        // Act
        var dto = new UpdateProductDto(name, description, price, stock);

        // Assert
        dto.Name.Should().Be(name);
        dto.Description.Should().Be(description);
        dto.Price.Should().Be(price);
        dto.Stock.Should().Be(stock);
    }

    [Fact]
    public void UpdateProductDto_Should_Handle_All_Null_Values()
    {
        // Arrange & Act
        var dto = new UpdateProductDto(null, null, null, null);

        // Assert
        dto.Name.Should().BeNull();
        dto.Description.Should().BeNull();
        dto.Price.Should().BeNull();
        dto.Stock.Should().BeNull();
    }

    [Fact]
    public void UpdateProductDto_Should_Handle_Partial_Updates()
    {
        // Arrange & Act
        var dto = new UpdateProductDto("New Name", null, 199.99m, null);

        // Assert
        dto.Name.Should().Be("New Name");
        dto.Description.Should().BeNull();
        dto.Price.Should().Be(199.99m);
        dto.Stock.Should().BeNull();
    }

    [Fact]
    public void UpdateProductDto_Records_Should_Be_Equal_With_Same_Values()
    {
        // Arrange
        var dto1 = new UpdateProductDto("Product", "Description", 99.99m, 10);
        var dto2 = new UpdateProductDto("Product", "Description", 99.99m, 10);

        // Act & Assert
        dto1.Should().Be(dto2);
        dto1.GetHashCode().Should().Be(dto2.GetHashCode());
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("ValidProductName")]
    [InlineData("Product with spaces and numbers 123")]
    public void CreateProductDto_Should_Accept_Various_Name_Values(string name)
    {
        // Arrange & Act
        var dto = new CreateProductDto(name, "Description", 99.99m, 10);

        // Assert
        dto.Name.Should().Be(name);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(0.01)]
    [InlineData(999999.99)]
    public void CreateProductDto_Should_Accept_Various_Price_Values(decimal price)
    {
        // Arrange & Act
        var dto = new CreateProductDto("Product", "Description", price, 10);

        // Assert
        dto.Price.Should().Be(price);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(1000)]
    [InlineData(int.MaxValue)]
    public void CreateProductDto_Should_Accept_Various_Stock_Values(int stock)
    {
        // Arrange & Act
        var dto = new CreateProductDto("Product", "Description", 99.99m, stock);

        // Assert
        dto.Stock.Should().Be(stock);
    }
}