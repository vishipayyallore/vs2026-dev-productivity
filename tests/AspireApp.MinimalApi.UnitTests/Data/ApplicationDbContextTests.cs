using Aspire.MinimalApi.Data;
using AspireApp.SharedLib.Models;

namespace AspireApp.MinimalApi.UnitTests.Data;

/// <summary>
/// Unit tests for ApplicationDbContext
/// </summary>
public class ApplicationDbContextTests : IDisposable
{
    private readonly ApplicationDbContext _context;
    private bool _disposed;

    public ApplicationDbContextTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: $"TestDb_{Guid.NewGuid()}")
            .Options;

        _context = new ApplicationDbContext(options);
    }

    #region DbSet Tests

    [Fact]
    public void Products_DbSet_Should_Not_Be_Null()
    {
        // Assert
        _context.Products.Should().NotBeNull();
    }

    [Fact]
    public void HurricaneAlerts_DbSet_Should_Not_Be_Null()
    {
        // Assert
        _context.HurricaneAlerts.Should().NotBeNull();
    }

    #endregion

    #region Product Tests

    [Fact]
    public async Task Should_Add_Product_To_Database()
    {
        // Arrange
        var product = new Product
        {
            Name = "Test Product",
            Description = "Test Description",
            Price = 99.99m,
            Stock = 50,
            CreatedAt = DateTime.UtcNow
        };

        // Act
        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        // Assert
        var savedProduct = await _context.Products.FirstOrDefaultAsync(p => p.Name == "Test Product");
        savedProduct.Should().NotBeNull();
        savedProduct!.Id.Should().BeGreaterThan(0);
        savedProduct.Name.Should().Be("Test Product");
        savedProduct.Price.Should().Be(99.99m);
    }

    [Fact]
    public async Task Should_Update_Product_In_Database()
    {
        // Arrange
        var product = new Product
        {
            Name = "Original Name",
            Price = 10m,
            Stock = 5,
            CreatedAt = DateTime.UtcNow
        };
        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        // Act
        product.Name = "Updated Name";
        product.Price = 20m;
        await _context.SaveChangesAsync();

        // Assert
        var updatedProduct = await _context.Products.FindAsync(product.Id);
        updatedProduct.Should().NotBeNull();
        updatedProduct!.Name.Should().Be("Updated Name");
        updatedProduct.Price.Should().Be(20m);
    }

    [Fact]
    public async Task Should_Delete_Product_From_Database()
    {
        // Arrange
        var product = new Product
        {
            Name = "To Delete",
            Price = 10m,
            Stock = 5,
            CreatedAt = DateTime.UtcNow
        };
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        var productId = product.Id;

        // Act
        _context.Products.Remove(product);
        await _context.SaveChangesAsync();

        // Assert
        var deletedProduct = await _context.Products.FindAsync(productId);
        deletedProduct.Should().BeNull();
    }

    [Fact]
    public async Task Should_Query_Products_With_Filter()
    {
        // Arrange
        var products = new[]
        {
            new Product { Name = "Product A", Price = 10m, Stock = 5, CreatedAt = DateTime.UtcNow },
            new Product { Name = "Product B", Price = 20m, Stock = 10, CreatedAt = DateTime.UtcNow },
            new Product { Name = "Product C", Price = 30m, Stock = 15, CreatedAt = DateTime.UtcNow }
        };
        _context.Products.AddRange(products);
        await _context.SaveChangesAsync();

        // Act
        var result = await _context.Products
            .Where(p => p.Price > 15m)
            .ToListAsync();

        // Assert
        result.Should().HaveCount(2);
        result.Should().Contain(p => p.Name == "Product B");
        result.Should().Contain(p => p.Name == "Product C");
    }

    #endregion

    #region HurricaneAlert Tests

    [Fact]
    public async Task Should_Add_HurricaneAlert_To_Database()
    {
        // Arrange
        var alert = new HurricaneAlert("Hurricane Test", 3, 120, "Atlantic", "Test description")
        {
            Severity = SeverityLevel.High,
            IsActive = true
        };

        // Act
        _context.HurricaneAlerts.Add(alert);
        await _context.SaveChangesAsync();

        // Assert
        var savedAlert = await _context.HurricaneAlerts.FirstOrDefaultAsync(a => a.Name == "Hurricane Test");
        savedAlert.Should().NotBeNull();
        savedAlert!.Id.Should().BeGreaterThan(0);
        savedAlert.Name.Should().Be("Hurricane Test");
        savedAlert.Category.Should().Be(3);
        savedAlert.WindSpeedMph.Should().Be(120);
    }

    [Fact]
    public async Task Should_Update_HurricaneAlert_In_Database()
    {
        // Arrange
        var alert = new HurricaneAlert("Hurricane Alpha", 2, 100, "Atlantic", "Description");
        _context.HurricaneAlerts.Add(alert);
        await _context.SaveChangesAsync();

        // Act
        alert.Category = 3;
        alert.WindSpeedMph = 125;
        alert.IsActive = false;
        await _context.SaveChangesAsync();

        // Assert
        var updatedAlert = await _context.HurricaneAlerts.FindAsync(alert.Id);
        updatedAlert.Should().NotBeNull();
        updatedAlert!.Category.Should().Be(3);
        updatedAlert.WindSpeedMph.Should().Be(125);
        updatedAlert.IsActive.Should().BeFalse();
    }

    [Fact]
    public async Task Should_Delete_HurricaneAlert_From_Database()
    {
        // Arrange
        var alert = new HurricaneAlert("To Delete", 1, 70, "Location", "Desc");
        _context.HurricaneAlerts.Add(alert);
        await _context.SaveChangesAsync();
        var alertId = alert.Id;

        // Act
        _context.HurricaneAlerts.Remove(alert);
        await _context.SaveChangesAsync();

        // Assert
        var deletedAlert = await _context.HurricaneAlerts.FindAsync(alertId);
        deletedAlert.Should().BeNull();
    }

    [Fact]
    public async Task Should_Query_HurricaneAlerts_By_IsActive()
    {
        // Arrange
        var alerts = new[]
        {
            new HurricaneAlert("Alpha", 2, 100, "Location 1", "Desc 1") { IsActive = true },
            new HurricaneAlert("Beta", 3, 120, "Location 2", "Desc 2") { IsActive = false },
            new HurricaneAlert("Gamma", 4, 140, "Location 3", "Desc 3") { IsActive = true }
        };
        _context.HurricaneAlerts.AddRange(alerts);
        await _context.SaveChangesAsync();

        // Act
        var activeAlerts = await _context.HurricaneAlerts
            .Where(a => a.IsActive)
            .ToListAsync();

        // Assert
        activeAlerts.Should().HaveCount(2);
        activeAlerts.Should().OnlyContain(a => a.IsActive);
    }

    [Fact]
    public async Task Should_Query_HurricaneAlerts_By_Severity()
    {
        // Arrange
        var alerts = new[]
        {
            new HurricaneAlert("Low", 1, 75, "Loc", "Desc") { Severity = SeverityLevel.Low },
            new HurricaneAlert("High", 4, 145, "Loc", "Desc") { Severity = SeverityLevel.High },
            new HurricaneAlert("Critical", 5, 170, "Loc", "Desc") { Severity = SeverityLevel.Critical }
        };
        _context.HurricaneAlerts.AddRange(alerts);
        await _context.SaveChangesAsync();

        // Act
        var criticalAlerts = await _context.HurricaneAlerts
            .Where(a => a.Severity == SeverityLevel.Critical)
            .ToListAsync();

        // Assert
        criticalAlerts.Should().HaveCount(1);
        criticalAlerts.First().Name.Should().Be("Critical");
    }

    #endregion

    #region Relationship and Complex Query Tests

    [Fact]
    public async Task Should_Support_Ordering_And_Paging()
    {
        // Arrange
        var products = Enumerable.Range(1, 10).Select(i => new Product
        {
            Name = $"Product {i}",
            Price = i * 10m,
            Stock = i,
            CreatedAt = DateTime.UtcNow.AddDays(-i)
        });
        _context.Products.AddRange(products);
        await _context.SaveChangesAsync();

        // Act
        var page1 = await _context.Products
            .OrderBy(p => p.Name)
            .Skip(0)
            .Take(5)
            .ToListAsync();

        var page2 = await _context.Products
            .OrderBy(p => p.Name)
            .Skip(5)
            .Take(5)
            .ToListAsync();

        // Assert
        page1.Should().HaveCount(5);
        page2.Should().HaveCount(5);
        page1.First().Name.Should().Be("Product 1");
        page2.Last().Name.Should().Be("Product 9");
    }

    [Fact]
    public async Task Should_Support_Count_Aggregation()
    {
        // Arrange
        var products = Enumerable.Range(1, 5).Select(i => new Product
        {
            Name = $"Product {i}",
            Price = i * 10m,
            Stock = i,
            CreatedAt = DateTime.UtcNow
        });
        _context.Products.AddRange(products);
        await _context.SaveChangesAsync();

        // Act
        var count = await _context.Products.CountAsync();

        // Assert
        count.Should().Be(5);
    }

    [Fact]
    public async Task Should_Support_Any_Query()
    {
        // Arrange
        var product = new Product
        {
            Name = "Unique Product",
            Price = 100m,
            Stock = 1,
            CreatedAt = DateTime.UtcNow
        };
        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        // Act
        var exists = await _context.Products.AnyAsync(p => p.Name == "Unique Product");
        var notExists = await _context.Products.AnyAsync(p => p.Name == "Non Existent");

        // Assert
        exists.Should().BeTrue();
        notExists.Should().BeFalse();
    }

    #endregion

    #region IDisposable Implementation

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _context?.Dispose();
            }
            _disposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    #endregion
}
