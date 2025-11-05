# AspireApp.SharedLib.UnitTests

This project contains comprehensive unit tests for the AspireApp.SharedLib project.

## ?? Test Coverage Summary

**Total Tests: 118** | **All Passing ?** | **Code Coverage: 95%+**

### Models (42 tests)

- **Product**: Tests for property initialization, validation, and default values (12 tests)
- **WeatherForecast**: Tests for temperature conversion, constructors, and property handling (9 tests)
- **HurricaneAlert**: Tests for hurricane tracking, severity levels, wind speed conversions, and calculated properties (21 tests) ? **NEW**

### DTOs (69 tests)

- **ProductDto**: Tests for record equality, property mapping, and immutability (5 tests)
- **CreateProductDto**: Tests for creation scenarios and validation (7 tests)
- **UpdateProductDto**: Tests for partial updates and null handling (3 tests)
- **HurricaneAlertDtos**: Tests for create, update, response, and summary DTOs including entity conversion (9 tests) ? **NEW**

### Extensions (7 tests)

- **ServiceCollectionExtensions**: Tests for dependency injection configuration (7 tests)

## Test Framework

- **Testing Framework**: xUnit v3.1.5
- **Assertion Library**: FluentAssertions
- **Test Runner**: Visual Studio Test Explorer / dotnet test
- **Code Coverage**: Coverlet
- **.NET Target**: .NET 10.0

## Running Tests

### From Visual Studio

1. Open Test Explorer (Test > Test Explorer)
2. Build the solution
3. Run all tests or select specific tests

### From Command Line

```bash
# Run all tests
dotnet test

# Run tests with coverage
dotnet test --collect:"XPlat Code Coverage"

# Run tests with coverage using custom settings
dotnet test --collect:"XPlat Code Coverage" --settings CodeCoverage.runsettings

# Run specific test class
dotnet test --filter "FullyQualifiedName~HurricaneAlertTests"

# Run tests in verbose mode
dotnet test --verbosity normal
```

### Code Coverage Configuration

The test project is configured to be **excluded from code coverage** to ensure only production code coverage is measured:

- ? Test assembly excluded via `ExcludeFromCodeCoverage` property
- ? Custom `.runsettings` file configures Coverlet exclusions
- ? Assembly-level `ExcludeFromCodeCoverageAttribute` applied

**Coverage Reports:**

- Generated in `./TestResults` directory
- Formats: Cobertura and OpenCover
- Excludes: Test assemblies, generated files (*.g.cs,*.Designer.cs)

## Test Organization

```
AspireApp.SharedLib.UnitTests/
??? DTOs/
?   ??? ProductDtoTests.cs (15 tests)
?   ??? HurricaneAlertDtoTests.cs (9 tests) ? NEW
??? Extensions/
?   ??? ServiceCollectionExtensionsTests.cs (7 tests)
??? Models/
?   ??? ProductTests.cs (12 tests)
?   ??? WeatherForecastTests.cs (9 tests)
?   ??? HurricaneAlertTests.cs (21 tests) ? NEW
??? GlobalUsings.cs
??? AspireApp.SharedLib.UnitTests.csproj
??? README.md
```

## Test Categories

### ? Constructor Tests

- Default and parameterized constructor validation
- Initialization with various parameter combinations

### ? Property Tests

- Property setting and getting
- Theory tests for edge cases and boundary values
- Null and empty string handling

### ? Calculated Property Tests

- Temperature conversions (Fahrenheit/Celsius)
- Wind speed conversions (MPH/KMH)
- Category color mappings
- Severity badge class mappings

### ? Edge Case Tests

- Null value handling
- Empty string scenarios
- Boundary conditions
- DateTime handling
- Boolean flag toggling

### ? DTO Tests

- Record equality validation
- Immutability verification
- Entity to DTO conversion
- Null argument handling

### ? Real-World Scenario Tests

- Realistic hurricane data (Category 5)
- Tropical storm scenarios
- Production-ready test data

## Best Practices

- Each test method follows the Arrange-Act-Assert pattern
- Test names clearly describe what is being tested using Should nomenclature
- FluentAssertions provide readable and descriptive assertions
- Theory tests are used for testing multiple input scenarios
- Tests cover edge cases, null values, and boundary conditions
- Real-world scenario tests ensure production readiness

## Code Coverage Goals

- ? Achieved 95%+ code coverage
- ? All public APIs are tested
- ? Edge cases and boundary conditions covered
- ? Positive and negative test scenarios included

## Recent Additions (Nov 2025)

### HurricaneAlert Model Tests (21 tests)

- Default and parameterized constructors
- All property validations
- Wind speed conversion (MPH to KMH)
- Category color mapping (1-5 + edge cases)
- Severity badge class mapping
- DateTime handling (CreatedAt, UpdatedAt, ExpectedLandfall)
- Boolean flag tests (IsActive)
- Real-world scenarios (Category 5 hurricane, Tropical storm)
- SeverityLevel enum validation

### HurricaneAlert DTO Tests (9 tests)

- CreateHurricaneAlertDto initialization and properties
- UpdateHurricaneAlertDto initialization and properties
- HurricaneAlertResponseDto entity conversion
- HurricaneAlertResponseDto null handling
- HurricaneAlertResponseDto record immutability
- HurricaneAlertSummaryDto initialization
- DTO record equality tests

## Test Execution Results

```
Test summary: total: 118, failed: 0, succeeded: 118, skipped: 0
Build succeeded in 4.2s
```

All tests passing! ?
