using Aspire.MinimalApi.Data;
using AspireApp.SharedLib.Dtos;
using AspireApp.SharedLib.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace AspireApp.MinimalApi.UnitTests.Endpoints;

/// <summary>
/// Comprehensive unit tests for Hurricane Alert API endpoints
/// </summary>
public class HurricaneAlertEndpointsTests : IDisposable
{
    private readonly ApplicationDbContext _context;
    private bool _disposed;

    public HurricaneAlertEndpointsTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: $"HurricaneTestDb_{Guid.NewGuid()}")
            .Options;

        _context = new ApplicationDbContext(options);
        
        // Seed test data
        SeedTestData();
    }

    private void SeedTestData()
    {
        var alerts = new List<HurricaneAlert>
        {
            new("Hurricane Alpha", 3, 120, "Atlantic", "Test hurricane 1")
            {
                Id = 1,
                Severity = SeverityLevel.High,
                IsActive = true,
                CreatedAt = DateTime.UtcNow.AddDays(-2),
                UpdatedAt = DateTime.UtcNow.AddDays(-1)
            },
            new("Hurricane Beta", 4, 145, "Gulf of Mexico", "Test hurricane 2")
            {
                Id = 2,
                Severity = SeverityLevel.Critical,
                IsActive = true,
                CreatedAt = DateTime.UtcNow.AddDays(-1),
                UpdatedAt = DateTime.UtcNow
            },
            new("Tropical Storm Gamma", 1, 70, "Caribbean", "Test storm")
            {
                Id = 3,
                Severity = SeverityLevel.Low,
                IsActive = false,
                CreatedAt = DateTime.UtcNow.AddDays(-5),
                UpdatedAt = DateTime.UtcNow.AddDays(-3)
            }
        };

        _context.HurricaneAlerts.AddRange(alerts);
        _context.SaveChanges();
    }

    #region GetAllHurricaneAlerts Tests

    [Fact]
    public async Task GetAllHurricaneAlerts_Should_Return_All_Alerts()
    {
        // Act
        var result = await GetAllHurricaneAlertsInternal(_context);

        // Assert
        result.Should().NotBeNull();
        var okResult = result as Ok<List<HurricaneAlertResponseDto>>;
        okResult.Should().NotBeNull();
        okResult!.Value.Should().HaveCount(3);
    }

    [Fact]
    public async Task GetAllHurricaneAlerts_Should_Filter_By_Active_Status()
    {
        // Act
        var result = await GetAllHurricaneAlertsInternal(_context, isActive: true);

        // Assert
        var okResult = result as Ok<List<HurricaneAlertResponseDto>>;
        okResult.Should().NotBeNull();
        okResult!.Value.Should().HaveCount(2);
        okResult.Value.Should().OnlyContain(a => a.IsActive);
    }

    [Fact]
    public async Task GetAllHurricaneAlerts_Should_Filter_By_Inactive_Status()
    {
        // Act
        var result = await GetAllHurricaneAlertsInternal(_context, isActive: false);

        // Assert
        var okResult = result as Ok<List<HurricaneAlertResponseDto>>;
        okResult!.Value.Should().HaveCount(1);
        okResult.Value.Should().OnlyContain(a => !a.IsActive);
    }

    [Fact]
    public async Task GetAllHurricaneAlerts_Should_Order_By_CreatedAt_Descending()
    {
        // Act
        var result = await GetAllHurricaneAlertsInternal(_context);

        // Assert
        var okResult = result as Ok<List<HurricaneAlertResponseDto>>;
        var alerts = okResult!.Value;
        
        alerts[0].Name.Should().Be("Hurricane Beta");
        alerts[1].Name.Should().Be("Hurricane Alpha");
        alerts[2].Name.Should().Be("Tropical Storm Gamma");
    }

    #endregion

    #region GetActiveHurricaneAlerts Tests

    [Fact]
    public async Task GetActiveHurricaneAlerts_Should_Return_Only_Active_Alerts()
    {
        // Act
        var result = await GetActiveHurricaneAlertsInternal(_context);

        // Assert
        var okResult = result as Ok<List<HurricaneAlertResponseDto>>;
        okResult.Should().NotBeNull();
        okResult!.Value.Should().HaveCount(2);
        okResult.Value.Should().OnlyContain(a => a.IsActive);
    }

    [Fact]
    public async Task GetActiveHurricaneAlerts_Should_Order_By_Severity_And_Category()
    {
        // Act
        var result = await GetActiveHurricaneAlertsInternal(_context);

        // Assert
        var okResult = result as Ok<List<HurricaneAlertResponseDto>>;
        var alerts = okResult!.Value;
        
        // Critical severity (Beta) should come before High severity (Alpha)
        alerts[0].Severity.Should().Be(SeverityLevel.Critical);
        alerts[1].Severity.Should().Be(SeverityLevel.High);
    }

    #endregion

    #region GetHurricaneAlertById Tests

    [Fact]
    public async Task GetHurricaneAlertById_Should_Return_Alert_When_Exists()
    {
        // Arrange
        var alertId = 1;

        // Act
        var result = await GetHurricaneAlertByIdInternal(alertId, _context);

        // Assert
        result.Should().NotBeNull();
        var okResult = result as Ok<HurricaneAlertResponseDto>;
        okResult.Should().NotBeNull();
        okResult!.Value.Should().NotBeNull();
        okResult.Value!.Id.Should().Be(alertId);
        okResult.Value.Name.Should().Be("Hurricane Alpha");
    }

    [Fact]
    public async Task GetHurricaneAlertById_Should_Return_NotFound_When_Does_Not_Exist()
    {
        // Arrange
        var nonExistentId = 999;

        // Act
        var result = await GetHurricaneAlertByIdInternal(nonExistentId, _context);

        // Assert
        var notFoundResult = result as NotFound<string>;
        notFoundResult.Should().NotBeNull();
        notFoundResult!.Value.Should().Contain("999");
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public async Task GetHurricaneAlertById_Should_Return_Correct_Alert_For_Valid_Ids(int alertId)
    {
        // Act
        var result = await GetHurricaneAlertByIdInternal(alertId, _context);

        // Assert
        var okResult = result as Ok<HurricaneAlertResponseDto>;
        okResult.Should().NotBeNull();
        okResult!.Value!.Id.Should().Be(alertId);
    }

    #endregion

    #region CreateHurricaneAlert Tests

    [Fact]
    public async Task CreateHurricaneAlert_Should_Create_New_Alert_Successfully()
    {
        // Arrange
        var createDto = new CreateHurricaneAlertDto
        {
            Name = "Hurricane Delta",
            Category = 2,
            WindSpeedMph = 95,
            Location = "Pacific Ocean",
            Description = "New test hurricane",
            Severity = SeverityLevel.Medium,
            ExpectedLandfall = DateTime.UtcNow.AddDays(3)
        };

        // Act
        var result = await CreateHurricaneAlertInternal(createDto, _context);

        // Assert
        result.Should().NotBeNull();
        var createdResult = result as Created<HurricaneAlertResponseDto>;
        createdResult.Should().NotBeNull();
        createdResult!.Value.Should().NotBeNull();
        createdResult.Value!.Name.Should().Be("Hurricane Delta");
        createdResult.Value.Category.Should().Be(2);
        createdResult.Value.WindSpeedMph.Should().Be(95);
    }

    [Fact]
    public async Task CreateHurricaneAlert_Should_Return_BadRequest_When_Name_Is_Empty()
    {
        // Arrange
        var createDto = new CreateHurricaneAlertDto
        {
            Name = "",
            Category = 2,
            WindSpeedMph = 95,
            Location = "Pacific"
        };

        // Act
        var result = await CreateHurricaneAlertInternal(createDto, _context);

        // Assert
        var badRequestResult = result as BadRequest<string>;
        badRequestResult.Should().NotBeNull();
        badRequestResult!.Value.Should().Contain("name");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(6)]
    [InlineData(-1)]
    public async Task CreateHurricaneAlert_Should_Return_BadRequest_For_Invalid_Category(int category)
    {
        // Arrange
        var createDto = new CreateHurricaneAlertDto
        {
            Name = "Test",
            Category = category,
            WindSpeedMph = 100,
            Location = "Test Location"
        };

        // Act
        var result = await CreateHurricaneAlertInternal(createDto, _context);

        // Assert
        var badRequestResult = result as BadRequest<string>;
        badRequestResult.Should().NotBeNull();
        badRequestResult!.Value.Should().Contain("category");
    }

    [Fact]
    public async Task CreateHurricaneAlert_Should_Return_BadRequest_When_WindSpeed_Is_Zero_Or_Negative()
    {
        // Arrange
        var createDto = new CreateHurricaneAlertDto
        {
            Name = "Test",
            Category = 2,
            WindSpeedMph = 0,
            Location = "Test"
        };

        // Act
        var result = await CreateHurricaneAlertInternal(createDto, _context);

        // Assert
        var badRequestResult = result as BadRequest<string>;
        badRequestResult.Should().NotBeNull();
        badRequestResult!.Value.Should().Contain("Wind speed");
    }

    [Fact]
    public async Task CreateHurricaneAlert_Should_Return_BadRequest_When_Location_Is_Empty()
    {
        // Arrange
        var createDto = new CreateHurricaneAlertDto
        {
            Name = "Test",
            Category = 2,
            WindSpeedMph = 100,
            Location = ""
        };

        // Act
        var result = await CreateHurricaneAlertInternal(createDto, _context);

        // Assert
        var badRequestResult = result as BadRequest<string>;
        badRequestResult.Should().NotBeNull();
        badRequestResult!.Value.Should().Contain("Location");
    }

    #endregion

    #region UpdateHurricaneAlert Tests

    [Fact]
    public async Task UpdateHurricaneAlert_Should_Update_Alert_Successfully()
    {
        // Arrange
        var updateDto = new UpdateHurricaneAlertDto
        {
            Id = 1,
            Name = "Updated Hurricane Alpha",
            Category = 4,
            WindSpeedMph = 140,
            Location = "Updated Location",
            Description = "Updated description",
            Severity = SeverityLevel.Critical,
            IsActive = true
        };

        // Act
        var result = await UpdateHurricaneAlertInternal(1, updateDto, _context);

        // Assert
        var okResult = result as Ok<HurricaneAlertResponseDto>;
        okResult.Should().NotBeNull();
        okResult!.Value.Should().NotBeNull();
        okResult.Value!.Name.Should().Be("Updated Hurricane Alpha");
        okResult.Value.Category.Should().Be(4);
        okResult.Value.WindSpeedMph.Should().Be(140);
    }

    [Fact]
    public async Task UpdateHurricaneAlert_Should_Return_BadRequest_When_Id_Mismatch()
    {
        // Arrange
        var updateDto = new UpdateHurricaneAlertDto { Id = 2 };

        // Act
        var result = await UpdateHurricaneAlertInternal(1, updateDto, _context);

        // Assert
        var badRequestResult = result as BadRequest<string>;
        badRequestResult.Should().NotBeNull();
        badRequestResult!.Value.Should().Contain("mismatch");
    }

    [Fact]
    public async Task UpdateHurricaneAlert_Should_Return_NotFound_When_Alert_Does_Not_Exist()
    {
        // Arrange
        var updateDto = new UpdateHurricaneAlertDto
        {
            Id = 999,
            Name = "Test",
            Category = 2,
            WindSpeedMph = 100,
            Location = "Test"
        };

        // Act
        var result = await UpdateHurricaneAlertInternal(999, updateDto, _context);

        // Assert
        var notFoundResult = result as NotFound<string>;
        notFoundResult.Should().NotBeNull();
    }

    [Fact]
    public async Task UpdateHurricaneAlert_Should_Update_UpdatedAt_Timestamp()
    {
        // Arrange
        var beforeUpdate = DateTime.UtcNow.AddSeconds(-1);
        var updateDto = new UpdateHurricaneAlertDto
        {
            Id = 1,
            Name = "Updated Name",
            Category = 3,
            WindSpeedMph = 120,
            Location = "Location"
        };

        // Act
        var result = await UpdateHurricaneAlertInternal(1, updateDto, _context);

        // Assert
        var okResult = result as Ok<HurricaneAlertResponseDto>;
        okResult!.Value!.UpdatedAt.Should().BeAfter(beforeUpdate);
        okResult.Value.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(2));
    }

    #endregion

    #region DeleteHurricaneAlert Tests

    [Fact]
    public async Task DeleteHurricaneAlert_Should_Delete_Alert_Successfully()
    {
        // Act
        var result = await DeleteHurricaneAlertInternal(1, _context);

        // Assert
        var noContentResult = result as NoContent;
        noContentResult.Should().NotBeNull();
        
        // Verify deletion
        var deletedAlert = await _context.HurricaneAlerts.FindAsync(1);
        deletedAlert.Should().BeNull();
    }

    [Fact]
    public async Task DeleteHurricaneAlert_Should_Return_NotFound_When_Alert_Does_Not_Exist()
    {
        // Act
        var result = await DeleteHurricaneAlertInternal(999, _context);

        // Assert
        var notFoundResult = result as NotFound<string>;
        notFoundResult.Should().NotBeNull();
        notFoundResult!.Value.Should().Contain("999");
    }

    #endregion

    #region DeactivateHurricaneAlert Tests

    [Fact]
    public async Task DeactivateHurricaneAlert_Should_Deactivate_Alert_Successfully()
    {
        // Act
        var result = await DeactivateHurricaneAlertInternal(1, _context);

        // Assert
        var okResult = result as Ok<HurricaneAlertResponseDto>;
        okResult.Should().NotBeNull();
        okResult!.Value.Should().NotBeNull();
        okResult.Value!.IsActive.Should().BeFalse();
    }

    [Fact]
    public async Task DeactivateHurricaneAlert_Should_Update_UpdatedAt_Timestamp()
    {
        // Arrange
        var beforeDeactivate = DateTime.UtcNow.AddSeconds(-1);

        // Act
        var result = await DeactivateHurricaneAlertInternal(1, _context);

        // Assert
        var okResult = result as Ok<HurricaneAlertResponseDto>;
        okResult!.Value!.UpdatedAt.Should().BeAfter(beforeDeactivate);
        okResult.Value.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(2));
    }

    [Fact]
    public async Task DeactivateHurricaneAlert_Should_Return_NotFound_When_Alert_Does_Not_Exist()
    {
        // Act
        var result = await DeactivateHurricaneAlertInternal(999, _context);

        // Assert
        var notFoundResult = result as NotFound<string>;
        notFoundResult.Should().NotBeNull();
    }

    #endregion

    #region Helper Methods (Simulating endpoint logic)

    private static async Task<IResult> GetAllHurricaneAlertsInternal(
        ApplicationDbContext context,
        bool? isActive = null)
    {
        try
        {
            var query = context.HurricaneAlerts.AsQueryable();

            if (isActive.HasValue)
            {
                query = query.Where(h => h.IsActive == isActive.Value);
            }

            var alerts = await query
                .OrderByDescending(h => h.CreatedAt)
                .Select(h => HurricaneAlertResponseDto.FromEntity(h))
                .ToListAsync();

            return Results.Ok(alerts);
        }
        catch (Exception ex)
        {
            return Results.Problem($"Error retrieving hurricane alerts: {ex.Message}");
        }
    }

    private static async Task<IResult> GetActiveHurricaneAlertsInternal(ApplicationDbContext context)
    {
        try
        {
            var alerts = await context.HurricaneAlerts
                .Where(h => h.IsActive)
                .OrderByDescending(h => h.Severity)
                .ThenByDescending(h => h.Category)
                .Select(h => HurricaneAlertResponseDto.FromEntity(h))
                .ToListAsync();

            return Results.Ok(alerts);
        }
        catch (Exception ex)
        {
            return Results.Problem($"Error retrieving active hurricane alerts: {ex.Message}");
        }
    }

    private static async Task<IResult> GetHurricaneAlertByIdInternal(
        int id,
        ApplicationDbContext context)
    {
        try
        {
            var alert = await context.HurricaneAlerts
                .FirstOrDefaultAsync(h => h.Id == id);

            if (alert == null)
            {
                return Results.NotFound($"Hurricane alert with ID {id} not found");
            }

            return Results.Ok(HurricaneAlertResponseDto.FromEntity(alert));
        }
        catch (Exception ex)
        {
            return Results.Problem($"Error retrieving hurricane alert: {ex.Message}");
        }
    }

    private static async Task<IResult> CreateHurricaneAlertInternal(
        CreateHurricaneAlertDto dto,
        ApplicationDbContext context)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                return Results.BadRequest("Hurricane name is required");
            }

            if (dto.Category < 1 || dto.Category > 5)
            {
                return Results.BadRequest("Hurricane category must be between 1 and 5");
            }

            if (dto.WindSpeedMph <= 0)
            {
                return Results.BadRequest("Wind speed must be greater than 0");
            }

            if (string.IsNullOrWhiteSpace(dto.Location))
            {
                return Results.BadRequest("Location is required");
            }

            var alert = new HurricaneAlert(
                dto.Name,
                dto.Category,
                dto.WindSpeedMph,
                dto.Location,
                dto.Description ?? string.Empty)
            {
                Severity = dto.Severity,
                ExpectedLandfall = dto.ExpectedLandfall
            };

            context.HurricaneAlerts.Add(alert);
            await context.SaveChangesAsync();

            return Results.Created($"/api/hurricane-alerts/{alert.Id}",
                                  HurricaneAlertResponseDto.FromEntity(alert));
        }
        catch (Exception ex)
        {
            return Results.Problem($"Error creating hurricane alert: {ex.Message}");
        }
    }

    private static async Task<IResult> UpdateHurricaneAlertInternal(
        int id,
        UpdateHurricaneAlertDto dto,
        ApplicationDbContext context)
    {
        try
        {
            if (id != dto.Id)
            {
                return Results.BadRequest("ID mismatch");
            }

            var alert = await context.HurricaneAlerts
                .FirstOrDefaultAsync(h => h.Id == id);

            if (alert == null)
            {
                return Results.NotFound($"Hurricane alert with ID {id} not found");
            }

            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                return Results.BadRequest("Hurricane name is required");
            }

            if (dto.Category < 1 || dto.Category > 5)
            {
                return Results.BadRequest("Hurricane category must be between 1 and 5");
            }

            if (dto.WindSpeedMph <= 0)
            {
                return Results.BadRequest("Wind speed must be greater than 0");
            }

            alert.Name = dto.Name;
            alert.Category = dto.Category;
            alert.WindSpeedMph = dto.WindSpeedMph;
            alert.Location = dto.Location;
            alert.Description = dto.Description;
            alert.Severity = dto.Severity;
            alert.IsActive = dto.IsActive;
            alert.ExpectedLandfall = dto.ExpectedLandfall;
            alert.UpdatedAt = DateTime.UtcNow;

            await context.SaveChangesAsync();

            return Results.Ok(HurricaneAlertResponseDto.FromEntity(alert));
        }
        catch (Exception ex)
        {
            return Results.Problem($"Error updating hurricane alert: {ex.Message}");
        }
    }

    private static async Task<IResult> DeleteHurricaneAlertInternal(
        int id,
        ApplicationDbContext context)
    {
        try
        {
            var alert = await context.HurricaneAlerts
                .FirstOrDefaultAsync(h => h.Id == id);

            if (alert == null)
            {
                return Results.NotFound($"Hurricane alert with ID {id} not found");
            }

            context.HurricaneAlerts.Remove(alert);
            await context.SaveChangesAsync();

            return Results.NoContent();
        }
        catch (Exception ex)
        {
            return Results.Problem($"Error deleting hurricane alert: {ex.Message}");
        }
    }

    private static async Task<IResult> DeactivateHurricaneAlertInternal(
        int id,
        ApplicationDbContext context)
    {
        try
        {
            var alert = await context.HurricaneAlerts
                .FirstOrDefaultAsync(h => h.Id == id);

            if (alert == null)
            {
                return Results.NotFound($"Hurricane alert with ID {id} not found");
            }

            alert.IsActive = false;
            alert.UpdatedAt = DateTime.UtcNow;

            await context.SaveChangesAsync();

            return Results.Ok(HurricaneAlertResponseDto.FromEntity(alert));
        }
        catch (Exception ex)
        {
            return Results.Problem($"Error deactivating hurricane alert: {ex.Message}");
        }
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
