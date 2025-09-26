namespace Aspire.Shared.DTOs;

/// <summary>
/// Data Transfer Object for Product API operations
/// </summary>
public record ProductDto(
    int Id,
    string Name,
    string? Description,
    decimal Price,
    int Stock,
    DateTime CreatedAt,
    DateTime? UpdatedAt);

/// <summary>
/// DTO for creating a new product
/// </summary>
public record CreateProductDto(
    string Name,
    string? Description,
    decimal Price,
    int Stock);

/// <summary>
/// DTO for updating an existing product
/// </summary>
public record UpdateProductDto(
    string? Name,
    string? Description,
    decimal? Price,
    int? Stock);