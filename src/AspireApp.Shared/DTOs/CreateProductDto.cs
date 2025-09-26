namespace AspireApp.SharedLib.DTOs;

/// <summary>
/// DTO for creating a new product
/// </summary>
public record CreateProductDto(
    string Name,
    string? Description,
    decimal Price,
    int Stock);
