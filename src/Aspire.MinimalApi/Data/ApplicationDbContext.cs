using Microsoft.EntityFrameworkCore;
using Aspire.Shared.Models;

namespace Aspire.MinimalApi.Data;

/// <summary>
/// Database context for the application with PostgreSQL support
/// </summary>
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// Products table
    /// </summary>
    public DbSet<Product> Products => Set<Product>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure Product entity
        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name)
                  .IsRequired()
                  .HasMaxLength(200);
            entity.Property(e => e.Description)
                  .HasMaxLength(1000);
            entity.Property(e => e.Price)
                  .HasPrecision(18, 2);
            entity.Property(e => e.CreatedAt)
                  .HasDefaultValueSql("NOW()");
            entity.HasIndex(e => e.Name);
        });

        // Seed some initial data
        modelBuilder.Entity<Product>().HasData(
            new Product { Id = 1, Name = "Sample Product 1", Description = "A sample product for testing", Price = 29.99m, Stock = 100 },
            new Product { Id = 2, Name = "Sample Product 2", Description = "Another sample product", Price = 49.99m, Stock = 50 }
        );
    }
}