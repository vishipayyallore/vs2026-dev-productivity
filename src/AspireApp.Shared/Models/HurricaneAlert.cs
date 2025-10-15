namespace AspireApp.SharedLib.Models;

/// <summary>
/// Hurricane Alert model representing critical weather warnings and emergency information.
/// Used across the application for tracking and displaying hurricane threats.
/// </summary>
public class HurricaneAlert
{
    public HurricaneAlert()
    {
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
        IsActive = true;
    }

    public HurricaneAlert(string name, int category, double windSpeedMph, string location, string description)
        : this()
    {
        Name = name;
        Category = category;
        WindSpeedMph = windSpeedMph;
        Location = location;
        Description = description;
    }

    /// <summary>
    /// Unique identifier for the hurricane alert
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Hurricane name (e.g., "Hurricane Milton")
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Hurricane category (1-5 on Saffir-Simpson scale)
    /// </summary>
    public int Category { get; set; }

    /// <summary>
    /// Maximum sustained wind speed in miles per hour
    /// </summary>
    public double WindSpeedMph { get; set; }

    /// <summary>
    /// Current location or projected path of the hurricane
    /// </summary>
    public string Location { get; set; } = string.Empty;

    /// <summary>
    /// Detailed description of the hurricane threat and safety instructions
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Severity level for emergency response (Low, Medium, High, Critical)
    /// </summary>
    public SeverityLevel Severity { get; set; } = SeverityLevel.Medium;

    /// <summary>
    /// Whether this alert is currently active
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// When the alert was first created
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// When the alert was last updated
    /// </summary>
    public DateTime UpdatedAt { get; set; }

    /// <summary>
    /// Expected landfall date and time (if applicable)
    /// </summary>
    public DateTime? ExpectedLandfall { get; set; }

    /// <summary>
    /// Wind speed in kilometers per hour (calculated property)
    /// </summary>
    public double WindSpeedKmh => WindSpeedMph * 1.60934;

    /// <summary>
    /// Get category color for UI display
    /// </summary>
    public string CategoryColor => Category switch
    {
        1 => "#74C0FC", // Light Blue
        2 => "#FFE066", // Yellow
        3 => "#FFB347", // Orange
        4 => "#FF6B6B", // Red
        5 => "#DA77F2", // Purple
        _ => "#ADB5BD"  // Gray
    };

    /// <summary>
    /// Get severity badge class for UI styling
    /// </summary>
    public string SeverityBadgeClass => Severity switch
    {
        SeverityLevel.Low => "badge-success",
        SeverityLevel.Medium => "badge-warning",
        SeverityLevel.High => "badge-danger",
        SeverityLevel.Critical => "badge-dark",
        _ => "badge-secondary"
    };
}

/// <summary>
/// Enumeration for hurricane alert severity levels
/// </summary>
public enum SeverityLevel
{
    Low = 1,
    Medium = 2,
    High = 3,
    Critical = 4
}