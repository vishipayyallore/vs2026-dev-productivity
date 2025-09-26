namespace AspireApp.SharedLib.DTOs;

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
