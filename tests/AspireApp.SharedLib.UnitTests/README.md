# AspireApp.SharedLib.UnitTests

This project contains comprehensive unit tests for the AspireApp.SharedLib project.

## Test Coverage

### Models
- **Product**: Tests for property initialization, validation, and default values
- **WeatherForecast**: Tests for temperature conversion, constructors, and property handling

### DTOs
- **ProductDto**: Tests for record equality, property mapping, and immutability
- **CreateProductDto**: Tests for creation scenarios and validation
- **UpdateProductDto**: Tests for partial updates and null handling

### Extensions
- **ServiceCollectionExtensions**: Tests for dependency injection configuration

## Test Framework

- **Testing Framework**: xUnit
- **Assertion Library**: FluentAssertions
- **Test Runner**: Visual Studio Test Explorer / dotnet test
- **Code Coverage**: Coverlet

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

# Run specific test class
dotnet test --filter "FullyQualifiedName~ProductTests"

# Run tests in verbose mode
dotnet test --verbosity normal
```

## Test Organization

```
AspireApp.SharedLib.UnitTests/
??? DTOs/
?   ??? ProductDtoTests.cs
??? Extensions/
?   ??? ServiceCollectionExtensionsTests.cs
??? Models/
?   ??? ProductTests.cs
?   ??? WeatherForecastTests.cs
??? GlobalUsings.cs
??? AspireApp.SharedLib.UnitTests.csproj
```

## Best Practices

- Each test method follows the Arrange-Act-Assert pattern
- Test names clearly describe what is being tested
- FluentAssertions provide readable and descriptive assertions
- Theory tests are used for testing multiple input scenarios
- Tests cover edge cases, null values, and boundary conditions

## Code Coverage Goals

- Aim for 90%+ code coverage
- Focus on testing business logic and edge cases
- Ensure all public APIs are tested
- Cover both positive and negative test scenarios