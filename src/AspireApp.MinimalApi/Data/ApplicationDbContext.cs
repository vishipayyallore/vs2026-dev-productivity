using AspireApp.SharedLib.Models;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

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

    /// <summary>
    /// Hurricane Alerts table
    /// </summary>
    public DbSet<HurricaneAlert> HurricaneAlerts => Set<HurricaneAlert>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);

        base.OnModelCreating(modelBuilder);

        // Configure PostgreSQL value generation strategy to use Identity instead of HiLo
        // This avoids the NullButNotEmpty sequence name issue
        modelBuilder.HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

        // Configure Product entity
        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id);

            // Explicitly configure Id to use Identity generation (not HiLo)
            entity.Property(e => e.Id)
                  .UseIdentityByDefaultColumn();

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

        // Configure HurricaneAlert entity
        modelBuilder.Entity<HurricaneAlert>(entity =>
        {
            entity.HasKey(e => e.Id);

            // Explicitly configure Id to use Identity generation
            entity.Property(e => e.Id)
                  .UseIdentityByDefaultColumn();

            entity.Property(e => e.Name)
                  .IsRequired()
                  .HasMaxLength(100);
            entity.Property(e => e.Location)
                  .IsRequired()
                  .HasMaxLength(200);
            entity.Property(e => e.Description)
                  .HasMaxLength(1000);
            entity.Property(e => e.Category)
                  .IsRequired();
            entity.Property(e => e.WindSpeedMph)
                  .HasPrecision(10, 2);
            entity.Property(e => e.Severity)
                  .HasConversion<int>();
            entity.Property(e => e.CreatedAt)
                  .HasDefaultValueSql("NOW()");
            entity.Property(e => e.UpdatedAt)
                  .HasDefaultValueSql("NOW()");

            entity.HasIndex(e => e.Name);
            entity.HasIndex(e => e.IsActive);
            entity.HasIndex(e => e.Category);
            entity.HasIndex(e => e.Severity);
        });

        // Seed some initial data
        modelBuilder.Entity<Product>().HasData(
            new Product { Id = 1, Name = "Sample Product 1", Description = "A sample product for testing", Price = 29.99m, Stock = 100, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Product { Id = 2, Name = "Sample Product 2", Description = "Another sample product", Price = 49.99m, Stock = 50, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) }
        );

        // Seed initial hurricane alert data
        modelBuilder.Entity<HurricaneAlert>().HasData(
            new HurricaneAlert
            {
                Id = 1,
                Name = "Hurricane Milton",
                Category = 4,
                WindSpeedMph = 150,
                Location = "Gulf of Mexico, approaching Florida West Coast",
                Description = "Major hurricane with extremely dangerous winds. Prepare for life-threatening storm surge and destructive winds.",
                Severity = SeverityLevel.Critical,
                IsActive = true,
                CreatedAt = new DateTime(2024, 10, 14, 12, 0, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2024, 10, 14, 12, 0, 0, DateTimeKind.Utc),
                ExpectedLandfall = new DateTime(2024, 10, 15, 6, 0, 0, DateTimeKind.Utc)
            },
            new HurricaneAlert
            {
                Id = 2,
                Name = "Tropical Storm Nadine",
                Category = 1,
                WindSpeedMph = 85,
                Location = "Atlantic Ocean, moving northwest",
                Description = "Tropical storm conditions expected. Monitor for potential strengthening.",
                Severity = SeverityLevel.Medium,
                IsActive = true,
                CreatedAt = new DateTime(2024, 10, 13, 8, 0, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2024, 10, 14, 8, 0, 0, DateTimeKind.Utc)
            }
        );
    }
}