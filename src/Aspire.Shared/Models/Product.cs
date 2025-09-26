namespace Aspire.Shared.Models;

/// <summary>
/// Represents a product entity with basic information
/// </summary>
public class Product
{
    public int Id { get; set; }
    
    public required string Name { get; set; }
    
    public string? Description { get; set; }
    
    public decimal Price { get; set; }
    
    public int Stock { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime? UpdatedAt { get; set; }
}
