using AspireApp.SharedLib.Dtos;
using AspireApp.SharedLib.Models;
using FluentAssertions;

namespace AspireApp.SharedLib.UnitTests.DTOs;

/// <summary>
/// Unit tests for HurricaneAlert DTOs
/// </summary>
public class HurricaneAlertDtoTests
{
    #region CreateHurricaneAlertDto Tests

    [Fact]
    public void CreateHurricaneAlertDto_Should_Initialize_With_Default_Values()
    {
        // Arrange & Act
        var dto = new CreateHurricaneAlertDto();

        // Assert
        dto.Name.Should().BeEmpty();
        dto.Category.Should().Be(1);
        dto.WindSpeedMph.Should().Be(0);
        dto.Location.Should().BeEmpty();
        dto.Description.Should().BeEmpty();
        dto.Severity.Should().Be(SeverityLevel.Medium);
        dto.ExpectedLandfall.Should().BeNull();
    }

    [Fact]
    public void CreateHurricaneAlertDto_Should_Set_All_Properties()
    {
        // Arrange
        var landfall = DateTime.UtcNow.AddDays(2);

        // Act
        var dto = new CreateHurricaneAlertDto
        {
            Name = "Hurricane Test",
            Category = 4,
            WindSpeedMph = 145,
            Location = "Gulf of Mexico",
            Description = "Major hurricane approaching",
            Severity = SeverityLevel.Critical,
            ExpectedLandfall = landfall
        };

        // Assert
        dto.Name.Should().Be("Hurricane Test");
        dto.Category.Should().Be(4);
        dto.WindSpeedMph.Should().Be(145);
        dto.Location.Should().Be("Gulf of Mexico");
        dto.Description.Should().Be("Major hurricane approaching");
        dto.Severity.Should().Be(SeverityLevel.Critical);
        dto.ExpectedLandfall.Should().Be(landfall);
    }

    #endregion

    #region UpdateHurricaneAlertDto Tests

    [Fact]
    public void UpdateHurricaneAlertDto_Should_Initialize_With_Default_Values()
    {
        // Arrange & Act
        var dto = new UpdateHurricaneAlertDto();

        // Assert
        dto.Id.Should().Be(0);
        dto.Name.Should().BeEmpty();
        dto.Category.Should().Be(1);
        dto.WindSpeedMph.Should().Be(0);
        dto.Location.Should().BeEmpty();
        dto.Description.Should().BeEmpty();
        dto.Severity.Should().Be(SeverityLevel.Medium);
        dto.IsActive.Should().BeTrue();
        dto.ExpectedLandfall.Should().BeNull();
    }

    [Fact]
    public void UpdateHurricaneAlertDto_Should_Set_All_Properties()
    {
        // Arrange
        var landfall = DateTime.UtcNow.AddDays(1);

        // Act
        var dto = new UpdateHurricaneAlertDto
        {
            Id = 1,
            Name = "Hurricane Updated",
            Category = 3,
            WindSpeedMph = 120,
            Location = "Atlantic Ocean",
            Description = "Updated description",
            Severity = SeverityLevel.High,
            IsActive = false,
            ExpectedLandfall = landfall
        };

        // Assert
        dto.Id.Should().Be(1);
        dto.Name.Should().Be("Hurricane Updated");
        dto.Category.Should().Be(3);
        dto.WindSpeedMph.Should().Be(120);
        dto.Location.Should().Be("Atlantic Ocean");
        dto.Description.Should().Be("Updated description");
        dto.Severity.Should().Be(SeverityLevel.High);
        dto.IsActive.Should().BeFalse();
        dto.ExpectedLandfall.Should().Be(landfall);
    }

    #endregion

    #region HurricaneAlertResponseDto Tests

    [Fact]
    public void HurricaneAlertResponseDto_FromEntity_Should_Convert_Correctly()
    {
        // Arrange
        var entity = new HurricaneAlert
        {
            Id = 1,
            Name = "Hurricane Milton",
            Category = 4,
            WindSpeedMph = 150,
            Location = "Gulf of Mexico",
            Description = "Major hurricane",
            Severity = SeverityLevel.Critical,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            ExpectedLandfall = DateTime.UtcNow.AddDays(2)
        };

        // Act
        var dto = HurricaneAlertResponseDto.FromEntity(entity);

        // Assert
        dto.Id.Should().Be(entity.Id);
        dto.Name.Should().Be(entity.Name);
        dto.Category.Should().Be(entity.Category);
        dto.WindSpeedMph.Should().Be(entity.WindSpeedMph);
        dto.WindSpeedKmh.Should().Be(entity.WindSpeedKmh);
        dto.Location.Should().Be(entity.Location);
        dto.Description.Should().Be(entity.Description);
        dto.Severity.Should().Be(entity.Severity);
        dto.IsActive.Should().Be(entity.IsActive);
        dto.CreatedAt.Should().Be(entity.CreatedAt);
        dto.UpdatedAt.Should().Be(entity.UpdatedAt);
        dto.ExpectedLandfall.Should().Be(entity.ExpectedLandfall);
        dto.CategoryColor.Should().Be(entity.CategoryColor);
        dto.SeverityBadgeClass.Should().Be(entity.SeverityBadgeClass);
    }

    [Fact]
    public void HurricaneAlertResponseDto_FromEntity_Should_Throw_On_Null()
    {
        // Arrange
        HurricaneAlert? nullAlert = null;

        // Act
        var act = () => HurricaneAlertResponseDto.FromEntity(nullAlert!);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void HurricaneAlertResponseDto_Should_Be_Immutable_Record()
    {
        // Arrange
        var now = DateTime.UtcNow;
        var dto1 = new HurricaneAlertResponseDto(
            1, "Hurricane Test", 4, 150, 241.401, "Gulf", "Description",
            SeverityLevel.Critical, true, now, now, now.AddDays(2),
            "#FF6B6B", "badge-dark");

        var dto2 = new HurricaneAlertResponseDto(
            1, "Hurricane Test", 4, 150, 241.401, "Gulf", "Description",
            SeverityLevel.Critical, true, now, now, now.AddDays(2),
            "#FF6B6B", "badge-dark");

        // Assert
        dto1.Should().Be(dto2);
        dto1.GetHashCode().Should().Be(dto2.GetHashCode());
    }

    #endregion

    #region HurricaneAlertSummaryDto Tests

    [Fact]
    public void HurricaneAlertSummaryDto_Should_Initialize_Correctly()
    {
        // Arrange
        var createdAt = DateTime.UtcNow;

        // Act
        var dto = new HurricaneAlertSummaryDto(
            1, "Hurricane Test", 3, "Atlantic", SeverityLevel.High,
            true, createdAt, "#FFB347");

        // Assert
        dto.Id.Should().Be(1);
        dto.Name.Should().Be("Hurricane Test");
        dto.Category.Should().Be(3);
        dto.Location.Should().Be("Atlantic");
        dto.Severity.Should().Be(SeverityLevel.High);
        dto.IsActive.Should().BeTrue();
        dto.CreatedAt.Should().Be(createdAt);
        dto.CategoryColor.Should().Be("#FFB347");
    }

    [Fact]
    public void HurricaneAlertSummaryDto_Records_Should_Be_Equal_With_Same_Values()
    {
        // Arrange
        var createdAt = DateTime.UtcNow;
        var dto1 = new HurricaneAlertSummaryDto(
            1, "Test", 2, "Location", SeverityLevel.Medium,
            true, createdAt, "#FFE066");

        var dto2 = new HurricaneAlertSummaryDto(
            1, "Test", 2, "Location", SeverityLevel.Medium,
            true, createdAt, "#FFE066");

        // Act & Assert
        dto1.Should().Be(dto2);
        dto1.GetHashCode().Should().Be(dto2.GetHashCode());
    }

    #endregion
}
