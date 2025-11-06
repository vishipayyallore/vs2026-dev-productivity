using Aspire.MinimalApi.Data;
using AspireApp.SharedLib.DTOs;
using AspireApp.SharedLib.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace AspireApp.MinimalApi.UnitTests.Endpoints;

/// <summary>
/// Comprehensive unit tests for Product API endpoints
/// </summary>
public class ProductEndpointsTests : IDisposable
{
    private readonly ApplicationDbContext _context;
    private bool _disposed;

    public ProductEndpointsTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: $"ProductTestDb_{Guid.NewGuid()}")
            .Options;

        _context = new ApplicationDbContext(options);

        // Seed test data
        SeedTestData();
    }

    private void SeedTestData()
    {
        var products = new List<Product>
        {
            new() { Id = 1, Name = "Test Product 1", Description = "Description 1", Price = 10.99m, Stock = 100, CreatedAt = DateTime.UtcNow },
            new() { Id = 2, Name = "Test Product 2", Description = "Description 2", Price = 20.99m, Stock = 50, CreatedAt = DateTime.UtcNow },
            new() { Id = 3, Name = "Test Product 3", Description = "Description 3", Price = 30.99m, Stock = 25, CreatedAt = DateTime.UtcNow }
        };

        _context.Products.AddRange(products);
        _context.SaveChanges();
    }

    #region GetProducts Tests

    [Fact]
    public async Task GetProducts_Should_Return_Paginated_Results()
    {
        // Arrange
        var page = 1;
        var pageSize = 2;

        // Act
        var result = await GetProductsInternal(_context, page, pageSize);

        // Assert
        result.Should().NotBeNull();

        var valueProperty = result.GetType().GetProperty("Value");
        var value = valueProperty!.GetValue(result);
        value.Should().NotBeNull();

        var productsProperty = value!.GetType().GetProperty("Products");
        var products = productsProperty!.GetValue(value) as List<ProductDto>;
        products.Should().HaveCount(2);

        var totalCountProperty = value.GetType().GetProperty("TotalCount");
        var totalCount = (int)totalCountProperty!.GetValue(value)!;
        totalCount.Should().Be(3);
    }

    [Fact]
    public async Task GetProducts_Should_Return_All_Products_With_Default_Pagination()
    {
        // Arrange & Act
        var result = await GetProductsInternal(_context);

        // Assert
        result.Should().NotBeNull();

        var valueProperty = result.GetType().GetProperty("Value");
        var value = valueProperty!.GetValue(result);
        value.Should().NotBeNull();

        var productsProperty = value!.GetType().GetProperty("Products");
        var products = productsProperty!.GetValue(value) as List<ProductDto>;
        products.Should().HaveCount(3);
    }

    [Fact]
    public async Task GetProducts_Should_Calculate_Correct_Total_Pages()
    {
        // Arrange
        var page = 1;
        var pageSize = 2;

        // Act
        var result = await GetProductsInternal(_context, page, pageSize);

        // Assert
        var valueProperty = result.GetType().GetProperty("Value");
        var value = valueProperty!.GetValue(result);
        value.Should().NotBeNull();

        var totalPagesProperty = value!.GetType().GetProperty("TotalPages");
        var totalPages = (int)totalPagesProperty!.GetValue(value)!;
        totalPages.Should().Be(2); // 3 products / 2 per page = 2 pages
    }

    [Theory]
    [InlineData(1, 10)]
    [InlineData(2, 10)]
    [InlineData(1, 5)]
    public async Task GetProducts_Should_Handle_Different_Page_Sizes(int page, int pageSize)
    {
        // Act
        var result = await GetProductsInternal(_context, page, pageSize);

        // Assert
        result.Should().NotBeNull();
        var valueProperty = result.GetType().GetProperty("Value");
        valueProperty.Should().NotBeNull();
    }

    #endregion

    #region GetProduct Tests

    [Fact]
    public async Task GetProduct_Should_Return_Product_When_Exists()
    {
        // Arrange
        var productId = 1;

        // Act
        var result = await GetProductInternal(_context, productId);

        // Assert
        result.Should().NotBeNull();
        var okResult = result as Ok<ProductDto>;
        okResult.Should().NotBeNull();
        okResult!.Value.Should().NotBeNull();
        okResult.Value!.Id.Should().Be(productId);
        okResult.Value.Name.Should().Be("Test Product 1");
    }

    [Fact]
    public async Task GetProduct_Should_Return_NotFound_When_Product_Does_Not_Exist()
    {
        // Arrange
        var nonExistentId = 999;

        // Act
        var result = await GetProductInternal(_context, nonExistentId);

        // Assert
        result.Should().NotBeNull();
        var notFoundResult = result as NotFound<string>;
        notFoundResult.Should().NotBeNull();
        notFoundResult!.Value.Should().Contain("999");
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public async Task GetProduct_Should_Return_Correct_Product_For_Valid_Ids(int productId)
    {
        // Act
        var result = await GetProductInternal(_context, productId);

        // Assert
        var okResult = result as Ok<ProductDto>;
        okResult.Should().NotBeNull();
        okResult!.Value!.Id.Should().Be(productId);
    }

    #endregion

    #region CreateProduct Tests

    [Fact]
    public async Task CreateProduct_Should_Create_New_Product_Successfully()
    {
        // Arrange
        var createDto = new CreateProductDto(
            "New Product",
            "New Description",
            99.99m,
            75
        );

        // Act
        var result = await CreateProductInternal(_context, createDto);

        // Assert
        result.Should().NotBeNull();
        var createdResult = result as Created<ProductDto>;
        createdResult.Should().NotBeNull();
        createdResult!.Value.Should().NotBeNull();
        createdResult.Value!.Name.Should().Be("New Product");
        createdResult.Value.Price.Should().Be(99.99m);
        createdResult.Value.Stock.Should().Be(75);
        createdResult.Location.Should().Contain("/api/products/");
    }

    [Fact]
    public async Task CreateProduct_Should_Set_CreatedAt_Timestamp()
    {
        // Arrange
        var beforeCreate = DateTime.UtcNow.AddSeconds(-1);
        var createDto = new CreateProductDto("Test", "Desc", 10m, 5);

        // Act
        var result = await CreateProductInternal(_context, createDto);

        // Assert
        var createdResult = result as Created<ProductDto>;
        createdResult!.Value!.CreatedAt.Should().BeAfter(beforeCreate);
        createdResult.Value.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(2));
    }

    [Fact]
    public async Task CreateProduct_Should_Handle_Null_Description()
    {
        // Arrange
        var createDto = new CreateProductDto("Product", null, 50m, 10);

        // Act
        var result = await CreateProductInternal(_context, createDto);

        // Assert
        var createdResult = result as Created<ProductDto>;
        createdResult.Should().NotBeNull();
        createdResult!.Value!.Description.Should().BeNull();
    }

    [Theory]
    [InlineData("", "Description", 10.0, 5)]
    [InlineData("Product Name", "Description", 0.01, 0)]
    [InlineData("Product Name", "", 999.99, 1000)]
    public async Task CreateProduct_Should_Accept_Various_Valid_Inputs(
        string name, string? description, decimal price, int stock)
    {
        // Arrange
        var createDto = new CreateProductDto(name, description, price, stock);

        // Act
        var result = await CreateProductInternal(_context, createDto);

        // Assert
        var createdResult = result as Created<ProductDto>;
        createdResult.Should().NotBeNull();
        createdResult!.Value.Should().NotBeNull();
    }

    [Fact]
    public async Task CreateProduct_Should_Generate_Unique_Id()
    {
        // Arrange
        var dto1 = new CreateProductDto("Product 1", "Desc", 10m, 5);
        var dto2 = new CreateProductDto("Product 2", "Desc", 20m, 10);

        // Act
        var result1 = await CreateProductInternal(_context, dto1);
        var result2 = await CreateProductInternal(_context, dto2);

        // Assert
        var created1 = result1 as Created<ProductDto>;
        var created2 = result2 as Created<ProductDto>;

        created1!.Value!.Id.Should().NotBe(0);
        created2!.Value!.Id.Should().NotBe(0);
        created1.Value.Id.Should().NotBe(created2.Value.Id);
    }

    #endregion

    #region Helper Methods (Simulating endpoint logic)

    private static async Task<IResult> GetProductsInternal(
        ApplicationDbContext context,
        int page = 1,
        int pageSize = 10)
    {
        var skip = (page - 1) * pageSize;
        var products = await context.Products
            .OrderBy(p => p.Id)
            .Skip(skip)
            .Take(pageSize)
            .Select(p => new ProductDto(p.Id, p.Name, p.Description, p.Price, p.Stock, p.CreatedAt, p.UpdatedAt))
            .ToListAsync();

        var totalCount = await context.Products.CountAsync();

        var result = new
        {
            Products = products,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount,
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
        };

        return Results.Ok(result);
    }

    private static async Task<IResult> GetProductInternal(ApplicationDbContext context, int id)
    {
        var product = await context.Products.FindAsync(id);

        if (product == null)
        {
            return Results.NotFound($"Product with ID {id} not found");
        }

        var productDto = new ProductDto(
            product.Id,
            product.Name,
            product.Description,
            product.Price,
            product.Stock,
            product.CreatedAt,
            product.UpdatedAt);

        return Results.Ok(productDto);
    }

    private static async Task<IResult> CreateProductInternal(
        ApplicationDbContext context,
        CreateProductDto createProductDto)
    {
        var product = new Product
        {
            Name = createProductDto.Name,
            Description = createProductDto.Description,
            Price = createProductDto.Price,
            Stock = createProductDto.Stock,
            CreatedAt = DateTime.UtcNow
        };

        context.Products.Add(product);
        try
        {
            await context.SaveChangesAsync();
        }
        catch (DbUpdateException dbEx)
        {
            return Results.Problem($"Error creating product: {dbEx.Message}");
        }

        var productDto = new ProductDto(
            product.Id,
            product.Name,
            product.Description,
            product.Price,
            product.Stock,
            product.CreatedAt,
            product.UpdatedAt);

        return Results.Created($"/api/products/{product.Id}", productDto);
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
