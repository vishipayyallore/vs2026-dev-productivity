using Aspire.MinimalApi.Data;
using AspireApp.SharedLib.DTOs;
using AspireApp.SharedLib.Models;
using Microsoft.EntityFrameworkCore;

namespace Aspire.MinimalApi.Endpoints;

/// <summary>
/// Product API endpoints with full CRUD operations
/// </summary>
public static class ProductEndpoints
{
    /// <summary>
    /// Maps product endpoints to the application
    /// </summary>
    public static void MapProductEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/products")
                  .WithTags("Products");

        // GET /api/products
        group.MapGet("", GetProducts)
             .WithName("GetProducts")
             .WithSummary("Get all products")
             .WithDescription("Retrieve a paginated list of all products");

        // GET /api/products/{id}
        group.MapGet("{id:int}", GetProduct)
             .WithName("GetProduct")
             .WithSummary("Get product by ID")
             .WithDescription("Retrieve a specific product by its ID");

        // POST /api/products
        group.MapPost("", CreateProduct)
             .WithName("CreateProduct")
             .WithSummary("Create a new product")
             .WithDescription("Create a new product with the provided details");
    }

    /// <summary>
    /// Get all products with pagination
    /// </summary>
    private static async Task<IResult> GetProducts(
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
            .ToListAsync().ConfigureAwait(false);

        var totalCount = await context.Products.CountAsync().ConfigureAwait(false);

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

    /// <summary>
    /// Get a specific product by ID
    /// </summary>
    private static async Task<IResult> GetProduct(ApplicationDbContext context, int id)
    {
        var product = await context.Products.FindAsync(id).AsTask().ConfigureAwait(false);

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

    /// <summary>
    /// Create a new product
    /// </summary>
    private static async Task<IResult> CreateProduct(ApplicationDbContext context, CreateProductDto createProductDto)
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
            await context.SaveChangesAsync().ConfigureAwait(false);
        }
        catch (DbUpdateException dbEx)
        {
            // Database-specific error; return a problem response with info
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
}