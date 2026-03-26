using BankingSystem.Application;
using FluentAssertions;

namespace BankingSystem.Tests.Application;

/// <summary>
/// Tests for C# 14 Lambda parameter modifiers without explicit types
/// </summary>
public class AccountFiltersTests
{
    [Fact]
    public void FilterByAmount_WithGreaterThanPredicate_ShouldFilterCorrectly()
    {
        // Arrange
        var amounts = new List<decimal> { 100m, 200m, 300m, 400m, 500m };

        // Act - C# 14: Lambda with inferred types
        var result = AccountFilters.FilterByAmount(amounts, x => x > 250m);

        // Assert
        result.Should().HaveCount(3);
        result.Should().Contain(300m);
        result.Should().Contain(400m);
        result.Should().Contain(500m);
    }

    [Fact]
    public void FilterByAmount_WithLessThanPredicate_ShouldFilterCorrectly()
    {
        // Arrange
        var amounts = new List<decimal> { 100m, 200m, 300m, 400m, 500m };

        // Act - C# 14: Lambda without explicit type
        var result = AccountFilters.FilterByAmount(amounts, x => x < 300m);

        // Assert
        result.Should().HaveCount(2);
        result.Should().Contain(100m);
        result.Should().Contain(200m);
    }

    [Fact]
    public void FilterByAmount_WithEqualPredicate_ShouldReturnMatchingItems()
    {
        // Arrange
        var amounts = new List<decimal> { 100m, 200m, 100m, 300m, 100m };

        // Act
        var result = AccountFilters.FilterByAmount(amounts, x => x == 100m);

        // Assert
        result.Should().HaveCount(3);
        result.Should().OnlyContain(x => x == 100m);
    }

    [Fact]
    public void FilterByAmount_WithEmptyList_ShouldReturnEmptyList()
    {
        // Arrange
        var amounts = new List<decimal>();

        // Act
        var result = AccountFilters.FilterByAmount(amounts, x => x > 0);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void FilterByAmount_WithNoMatches_ShouldReturnEmptyList()
    {
        // Arrange
        var amounts = new List<decimal> { 100m, 200m, 300m };

        // Act
        var result = AccountFilters.FilterByAmount(amounts, x => x > 1000m);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void ApplyOperation_WithAddition_ShouldReturnSum()
    {
        // Arrange
        var a = 100m;
        var b = 200m;

        // Act - C# 14: Lambda with inferred types from Func signature
        var result = AccountFilters.ApplyOperation(a, b, (x, y) => x + y);

        // Assert
        result.Should().Be(300m);
    }

    [Fact]
    public void ApplyOperation_WithSubtraction_ShouldReturnDifference()
    {
        // Arrange
        var a = 500m;
        var b = 200m;

        // Act - C# 14: Lambda without explicit parameter types
        var result = AccountFilters.ApplyOperation(a, b, (x, y) => x - y);

        // Assert
        result.Should().Be(300m);
    }

    [Fact]
    public void ApplyOperation_WithMultiplication_ShouldReturnProduct()
    {
        // Arrange
        var a = 10m;
        var b = 5m;

        // Act
        var result = AccountFilters.ApplyOperation(a, b, (x, y) => x * y);

        // Assert
        result.Should().Be(50m);
    }

    [Fact]
    public void ApplyOperation_WithDivision_ShouldReturnQuotient()
    {
        // Arrange
        var a = 100m;
        var b = 4m;

        // Act
        var result = AccountFilters.ApplyOperation(a, b, (x, y) => x / y);

        // Assert
        result.Should().Be(25m);
    }

    [Theory]
    [InlineData(100, 200, 300)]
    [InlineData(0, 0, 0)]
    [InlineData(-50, 50, 0)]
    [InlineData(1000, 2000, 3000)]
    public void ApplyOperation_WithAdditionVariousValues_ShouldReturnCorrectSum(decimal a, decimal b, decimal expected)
    {
        // Act - C# 14: Lambda parameter type inference
        var result = AccountFilters.ApplyOperation(a, b, (x, y) => x + y);

        // Assert
        result.Should().Be(expected);
    }

    [Fact]
    public void FilterByAmount_WithComplexPredicate_ShouldWork()
    {
        // Arrange
        var amounts = new List<decimal> { 100m, 250m, 300m, 450m, 600m };

        // Act - C# 14: Complex lambda with inferred types
        var result = AccountFilters.FilterByAmount(amounts, x => x >= 250m && x <= 500m);

        // Assert
        result.Should().HaveCount(3);
        result.Should().Contain(250m);
        result.Should().Contain(300m);
        result.Should().Contain(450m);
    }
}
