using AspireApp.SharedLib.Models;

namespace AspireApp.SharedLib.Dtos;

/// <summary>
/// Data Transfer Object for creating new hurricane alerts
/// </summary>
public class CreateHurricaneAlertDto
{
    public string Name { get; set; } = string.Empty;
    public int Category { get; set; } = 1;
    public double WindSpeedMph { get; set; }
    public string Location { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public SeverityLevel Severity { get; set; } = SeverityLevel.Medium;
    public DateTime? ExpectedLandfall { get; set; }
}

/// <summary>
/// Data Transfer Object for updating existing hurricane alerts
/// </summary>
public class UpdateHurricaneAlertDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Category { get; set; } = 1;
    public double WindSpeedMph { get; set; }
    public string Location { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public SeverityLevel Severity { get; set; } = SeverityLevel.Medium;
    public bool IsActive { get; set; } = true;
    public DateTime? ExpectedLandfall { get; set; }
}

/// <summary>
/// Data Transfer Object for hurricane alert responses with computed properties
/// </summary>
public record HurricaneAlertResponseDto(
    int Id,
    string Name,
    int Category,
    double WindSpeedMph,
    double WindSpeedKmh,
    string Location,
    string Description,
    SeverityLevel Severity,
    bool IsActive,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    DateTime? ExpectedLandfall,
    string CategoryColor,
    string SeverityBadgeClass
)
{
    /// <summary>
    /// Convert from HurricaneAlert entity to response DTO
    /// </summary>
    public static HurricaneAlertResponseDto FromEntity(HurricaneAlert alert)
    {
        ArgumentNullException.ThrowIfNull(alert);

        return new HurricaneAlertResponseDto(
            alert.Id,
            alert.Name,
            alert.Category,
            alert.WindSpeedMph,
            alert.WindSpeedKmh,
            alert.Location,
            alert.Description,
            alert.Severity,
            alert.IsActive,
            alert.CreatedAt,
            alert.UpdatedAt,
            alert.ExpectedLandfall,
            alert.CategoryColor,
            alert.SeverityBadgeClass
        );
    }
}

/// <summary>
/// Data Transfer Object for hurricane alert summary/list view
/// </summary>
public record HurricaneAlertSummaryDto(
    int Id,
    string Name,
    int Category,
    string Location,
    SeverityLevel Severity,
    bool IsActive,
    DateTime CreatedAt,
    string CategoryColor
);