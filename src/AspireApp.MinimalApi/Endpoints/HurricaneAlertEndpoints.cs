using Aspire.MinimalApi.Data;
using AspireApp.SharedLib.Dtos;
using AspireApp.SharedLib.Models;
using Microsoft.EntityFrameworkCore;

namespace AspireApp.MinimalApi.Endpoints;

/// <summary>
/// Hurricane Alert API endpoints for managing emergency weather alerts
/// </summary>
public static class HurricaneAlertEndpoints
{
    /// <summary>
    /// Configure Hurricane Alert endpoints
    /// </summary>
    public static void MapHurricaneAlertEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/hurricane-alerts")
                      .WithTags("Hurricane Alerts")
                      .WithOpenApi();

        // GET /api/hurricane-alerts - Get all hurricane alerts
        group.MapGet("/", GetAllHurricaneAlerts)
             .WithName("GetAllHurricaneAlerts")
             .WithSummary("Get all hurricane alerts")
             .WithDescription("Retrieve all hurricane alerts with optional filtering by active status")
             .Produces<IEnumerable<HurricaneAlertResponseDto>>()
             .Produces(404);

        // GET /api/hurricane-alerts/active - Get only active alerts
        group.MapGet("/active", GetActiveHurricaneAlerts)
             .WithName("GetActiveHurricaneAlerts")
             .WithSummary("Get active hurricane alerts")
             .WithDescription("Retrieve only currently active hurricane alerts")
             .Produces<IEnumerable<HurricaneAlertResponseDto>>()
             .Produces(404);

        // GET /api/hurricane-alerts/{id} - Get specific hurricane alert
        group.MapGet("/{id:int}", GetHurricaneAlertById)
             .WithName("GetHurricaneAlertById")
             .WithSummary("Get hurricane alert by ID")
             .WithDescription("Retrieve a specific hurricane alert by its ID")
             .Produces<HurricaneAlertResponseDto>()
             .Produces(404);

        // POST /api/hurricane-alerts - Create new hurricane alert
        group.MapPost("/", CreateHurricaneAlert)
             .WithName("CreateHurricaneAlert")
             .WithSummary("Create new hurricane alert")
             .WithDescription("Create a new hurricane alert with the provided information")
             .Produces<HurricaneAlertResponseDto>(201)
             .Produces(400);

        // PUT /api/hurricane-alerts/{id} - Update hurricane alert
        group.MapPut("/{id:int}", UpdateHurricaneAlert)
             .WithName("UpdateHurricaneAlert")
             .WithSummary("Update hurricane alert")
             .WithDescription("Update an existing hurricane alert")
             .Produces<HurricaneAlertResponseDto>()
             .Produces(400)
             .Produces(404);

        // DELETE /api/hurricane-alerts/{id} - Delete hurricane alert
        group.MapDelete("/{id:int}", DeleteHurricaneAlert)
             .WithName("DeleteHurricaneAlert")
             .WithSummary("Delete hurricane alert")
             .WithDescription("Delete a hurricane alert by ID")
             .Produces(204)
             .Produces(404);

        // PATCH /api/hurricane-alerts/{id}/deactivate - Deactivate alert
        group.MapPatch("/{id:int}/deactivate", DeactivateHurricaneAlert)
             .WithName("DeactivateHurricaneAlert")
             .WithSummary("Deactivate hurricane alert")
             .WithDescription("Mark a hurricane alert as inactive")
             .Produces<HurricaneAlertResponseDto>()
             .Produces(404);
    }

    /// <summary>
    /// Get all hurricane alerts with optional filtering
    /// </summary>
    private static async Task<IResult> GetAllHurricaneAlerts(
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

    /// <summary>
    /// Get only active hurricane alerts
    /// </summary>
    private static async Task<IResult> GetActiveHurricaneAlerts(ApplicationDbContext context)
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

    /// <summary>
    /// Get hurricane alert by ID
    /// </summary>
    private static async Task<IResult> GetHurricaneAlertById(
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

    /// <summary>
    /// Create new hurricane alert
    /// </summary>
    private static async Task<IResult> CreateHurricaneAlert(
        CreateHurricaneAlertDto dto,
        ApplicationDbContext context)
    {
        try
        {
            // Validate input
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

    /// <summary>
    /// Update hurricane alert
    /// </summary>
    private static async Task<IResult> UpdateHurricaneAlert(
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

            // Validate input
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

            // Update properties
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

    /// <summary>
    /// Delete hurricane alert
    /// </summary>
    private static async Task<IResult> DeleteHurricaneAlert(
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

    /// <summary>
    /// Deactivate hurricane alert
    /// </summary>
    private static async Task<IResult> DeactivateHurricaneAlert(
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
}