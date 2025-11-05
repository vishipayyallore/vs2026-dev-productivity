using AspireApp.SharedLib.Models;
using FluentAssertions;

namespace AspireApp.SharedLib.UnitTests.Models;

/// <summary>
/// Comprehensive unit tests for the HurricaneAlert model
/// </summary>
public class HurricaneAlertTests
{
    #region Constructor Tests

    [Fact]
    public void HurricaneAlert_DefaultConstructor_Should_Initialize_With_Default_Values()
    {
        // Arrange & Act
        var alert = new HurricaneAlert();

        // Assert
        alert.Id.Should().Be(0);
        alert.Name.Should().BeEmpty();
        alert.Category.Should().Be(0);
        alert.WindSpeedMph.Should().Be(0);
        alert.Location.Should().BeEmpty();
        alert.Description.Should().BeEmpty();
        alert.Severity.Should().Be(SeverityLevel.Medium);
        alert.IsActive.Should().BeTrue();
        alert.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        alert.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        alert.ExpectedLandfall.Should().BeNull();
    }

    [Fact]
    public void HurricaneAlert_ParameterizedConstructor_Should_Initialize_With_Provided_Values()
    {
        // Arrange
        var name = "Hurricane Milton";
        var category = 4;
        var windSpeed = 150.0;
        var location = "Gulf of Mexico";
        var description = "Major hurricane with extremely dangerous winds";

        // Act
        var alert = new HurricaneAlert(name, category, windSpeed, location, description);

        // Assert
        alert.Name.Should().Be(name);
        alert.Category.Should().Be(category);
        alert.WindSpeedMph.Should().Be(windSpeed);
        alert.Location.Should().Be(location);
        alert.Description.Should().Be(description);
        alert.IsActive.Should().BeTrue();
        alert.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        alert.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    #endregion

    #region Property Tests

    [Fact]
    public void HurricaneAlert_Should_Set_All_Properties_Correctly()
    {
        // Arrange
        var now = DateTime.UtcNow;
        var landfall = now.AddDays(1);

        // Act
        var alert = new HurricaneAlert
        {
            Id = 1,
            Name = "Hurricane Test",
            Category = 3,
            WindSpeedMph = 125.5,
            Location = "Atlantic Ocean",
            Description = "Test Description",
            Severity = SeverityLevel.High,
            IsActive = true,
            CreatedAt = now,
            UpdatedAt = now,
            ExpectedLandfall = landfall
        };

        // Assert
        alert.Id.Should().Be(1);
        alert.Name.Should().Be("Hurricane Test");
        alert.Category.Should().Be(3);
        alert.WindSpeedMph.Should().Be(125.5);
        alert.Location.Should().Be("Atlantic Ocean");
        alert.Description.Should().Be("Test Description");
        alert.Severity.Should().Be(SeverityLevel.High);
        alert.IsActive.Should().BeTrue();
        alert.CreatedAt.Should().Be(now);
        alert.UpdatedAt.Should().Be(now);
        alert.ExpectedLandfall.Should().Be(landfall);
    }

    [Theory]
    [InlineData("")]
    [InlineData("Hurricane Alpha")]
    [InlineData("Tropical Storm Beta")]
    [InlineData("Hurricane with special chars !@#$%")]
    public void HurricaneAlert_Should_Accept_Various_Name_Values(string name)
    {
        // Arrange & Act
        var alert = new HurricaneAlert { Name = name };

        // Assert
        alert.Name.Should().Be(name);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    public void HurricaneAlert_Should_Accept_Valid_Category_Values(int category)
    {
        // Arrange & Act
        var alert = new HurricaneAlert { Category = category };

        // Assert
        alert.Category.Should().Be(category);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(75.5)]
    [InlineData(100)]
    [InlineData(150.25)]
    [InlineData(200)]
    public void HurricaneAlert_Should_Accept_Various_WindSpeed_Values(double windSpeed)
    {
        // Arrange & Act
        var alert = new HurricaneAlert { WindSpeedMph = windSpeed };

        // Assert
        alert.WindSpeedMph.Should().Be(windSpeed);
    }

    #endregion

    #region Calculated Property Tests

    [Theory]
    [InlineData(0, 0)]
    [InlineData(75, 120.7005)]
    [InlineData(100, 160.934)]
    [InlineData(150, 241.401)]
    [InlineData(200, 321.868)]
    public void WindSpeedKmh_Should_Calculate_Correctly_From_Mph(double mph, double expectedKmh)
    {
        // Arrange
        var alert = new HurricaneAlert { WindSpeedMph = mph };

        // Act
        var actualKmh = alert.WindSpeedKmh;

        // Assert
        actualKmh.Should().BeApproximately(expectedKmh, 0.001);
    }

    [Theory]
    [InlineData(1, "#74C0FC")]  // Light Blue
    [InlineData(2, "#FFE066")]  // Yellow
    [InlineData(3, "#FFB347")]  // Orange
    [InlineData(4, "#FF6B6B")]  // Red
    [InlineData(5, "#DA77F2")]  // Purple
    [InlineData(0, "#ADB5BD")]  // Gray (default)
    [InlineData(6, "#ADB5BD")]  // Gray (out of range)
    public void CategoryColor_Should_Return_Correct_Color_For_Category(int category, string expectedColor)
    {
        // Arrange
        var alert = new HurricaneAlert { Category = category };

        // Act
        var actualColor = alert.CategoryColor;

        // Assert
        actualColor.Should().Be(expectedColor);
    }

    [Theory]
    [InlineData(SeverityLevel.Low, "badge-success")]
    [InlineData(SeverityLevel.Medium, "badge-warning")]
    [InlineData(SeverityLevel.High, "badge-danger")]
    [InlineData(SeverityLevel.Critical, "badge-dark")]
    public void SeverityBadgeClass_Should_Return_Correct_Class_For_Severity(
        SeverityLevel severity, string expectedClass)
    {
        // Arrange
        var alert = new HurricaneAlert { Severity = severity };

        // Act
        var actualClass = alert.SeverityBadgeClass;

        // Assert
        actualClass.Should().Be(expectedClass);
    }

    #endregion

    #region Edge Case Tests

    [Fact]
    public void HurricaneAlert_Should_Allow_Null_ExpectedLandfall()
    {
        // Arrange & Act
        var alert = new HurricaneAlert { ExpectedLandfall = null };

        // Assert
        alert.ExpectedLandfall.Should().BeNull();
    }

    [Fact]
    public void HurricaneAlert_Should_Allow_Empty_Strings()
    {
        // Arrange & Act
        var alert = new HurricaneAlert
        {
            Name = "",
            Location = "",
            Description = ""
        };

        // Assert
        alert.Name.Should().BeEmpty();
        alert.Location.Should().BeEmpty();
        alert.Description.Should().BeEmpty();
    }

    [Fact]
    public void HurricaneAlert_IsActive_Should_Default_To_True()
    {
        // Arrange & Act
        var alert = new HurricaneAlert();

        // Assert
        alert.IsActive.Should().BeTrue();
    }

    [Fact]
    public void HurricaneAlert_Should_Allow_IsActive_Toggle()
    {
        // Arrange
        var alert = new HurricaneAlert { IsActive = true };

        // Act
        alert.IsActive = false;

        // Assert
        alert.IsActive.Should().BeFalse();
    }

    [Fact]
    public void HurricaneAlert_CreatedAt_And_UpdatedAt_Should_Be_Close_To_Now()
    {
        // Arrange
        var beforeCreation = DateTime.UtcNow.AddSeconds(-1);

        // Act
        var alert = new HurricaneAlert();
        var afterCreation = DateTime.UtcNow.AddSeconds(1);

        // Assert
        alert.CreatedAt.Should().BeAfter(beforeCreation);
        alert.CreatedAt.Should().BeBefore(afterCreation);
        alert.UpdatedAt.Should().BeAfter(beforeCreation);
        alert.UpdatedAt.Should().BeBefore(afterCreation);
    }

    #endregion

    #region Severity Level Tests

    [Fact]
    public void HurricaneAlert_Should_Accept_All_SeverityLevel_Values()
    {
        // Arrange & Act & Assert
        var lowAlert = new HurricaneAlert { Severity = SeverityLevel.Low };
        lowAlert.Severity.Should().Be(SeverityLevel.Low);

        var mediumAlert = new HurricaneAlert { Severity = SeverityLevel.Medium };
        mediumAlert.Severity.Should().Be(SeverityLevel.Medium);

        var highAlert = new HurricaneAlert { Severity = SeverityLevel.High };
        highAlert.Severity.Should().Be(SeverityLevel.High);

        var criticalAlert = new HurricaneAlert { Severity = SeverityLevel.Critical };
        criticalAlert.Severity.Should().Be(SeverityLevel.Critical);
    }

    [Fact]
    public void SeverityLevel_Enum_Should_Have_Correct_Integer_Values()
    {
        // Assert
        ((int)SeverityLevel.Low).Should().Be(1);
        ((int)SeverityLevel.Medium).Should().Be(2);
        ((int)SeverityLevel.High).Should().Be(3);
        ((int)SeverityLevel.Critical).Should().Be(4);
    }

    #endregion

    #region Real-World Scenario Tests

    [Fact]
    public void HurricaneAlert_Should_Represent_Realistic_Category5_Hurricane()
    {
        // Arrange & Act
        var alert = new HurricaneAlert
        {
            Id = 1,
            Name = "Hurricane Milton",
            Category = 5,
            WindSpeedMph = 180,
            Location = "Gulf of Mexico, approaching Florida West Coast",
            Description = "Catastrophic hurricane with life-threatening storm surge",
            Severity = SeverityLevel.Critical,
            IsActive = true,
            ExpectedLandfall = DateTime.UtcNow.AddDays(2)
        };

        // Assert
        alert.Name.Should().Be("Hurricane Milton");
        alert.Category.Should().Be(5);
        alert.WindSpeedMph.Should().Be(180);
        alert.WindSpeedKmh.Should().BeApproximately(289.68, 0.01);
        alert.CategoryColor.Should().Be("#DA77F2");
        alert.SeverityBadgeClass.Should().Be("badge-dark");
        alert.Severity.Should().Be(SeverityLevel.Critical);
        alert.IsActive.Should().BeTrue();
        alert.ExpectedLandfall.Should().NotBeNull();
    }

    [Fact]
    public void HurricaneAlert_Should_Represent_Realistic_TropicalStorm()
    {
        // Arrange & Act
        var alert = new HurricaneAlert
        {
            Name = "Tropical Storm Nadine",
            Category = 1,
            WindSpeedMph = 70,
            Location = "Atlantic Ocean, moving northwest",
            Description = "Tropical storm conditions expected",
            Severity = SeverityLevel.Low,
            IsActive = true
        };

        // Assert
        alert.Category.Should().Be(1);
        alert.WindSpeedMph.Should().Be(70);
        alert.CategoryColor.Should().Be("#74C0FC");
        alert.SeverityBadgeClass.Should().Be("badge-success");
        alert.Severity.Should().Be(SeverityLevel.Low);
    }

    #endregion
}
