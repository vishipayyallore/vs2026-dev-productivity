namespace AspireApp.SharedLib.DTOs;

/// <summary>
/// DTO for updating an existing product
/// </summary>
public record UpdateProductDto(
    string? Name,
    string? Description,
    decimal? Price,
    int? Stock);