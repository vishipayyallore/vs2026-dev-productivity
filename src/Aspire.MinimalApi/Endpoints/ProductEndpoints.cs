using Microsoft.EntityFrameworkCore;
using Aspire.MinimalApi.Data;
using Aspire.Shared.DTOs;
using Aspire.Shared.Models;

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
                      .WithTags("Products")
                      .WithOpenApi();

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
        try
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
        catch (Exception ex)
        {
            return Results.Problem($"Error retrieving products: {ex.Message}");
        }
    }

    /// <summary>
    /// Get a specific product by ID
    /// </summary>
    private static async Task<IResult> GetProduct(ApplicationDbContext context, int id)
    {
        try
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
        catch (Exception ex)
        {
            return Results.Problem($"Error retrieving product: {ex.Message}");
        }
    }

    /// <summary>
    /// Create a new product
    /// </summary>
    private static async Task<IResult> CreateProduct(ApplicationDbContext context, CreateProductDto createProductDto)
    {
        try
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
            await context.SaveChangesAsync();

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
        catch (Exception ex)
        {
            return Results.Problem($"Error creating product: {ex.Message}");
        }
    }
}