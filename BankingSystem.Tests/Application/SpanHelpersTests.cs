using BankingSystem.Application;
using FluentAssertions;

namespace BankingSystem.Tests.Application;

/// <summary>
/// Tests for C# 14 Implicit Span Conversions
/// </summary>
public class SpanHelpersTests
{
    [Fact]
    public void CalculateTotal_WithValidArray_ShouldReturnCorrectSum()
    {
        // Arrange
        decimal[] balances = { 100m, 200m, 300m, 400m, 500m };

        // Act - C# 14: Implicit conversion from array to ReadOnlySpan<decimal>
        var total = SpanHelpers.CalculateTotal(balances);

        // Assert
        total.Should().Be(1500m);
    }

    [Fact]
    public void CalculateTotal_WithEmptyArray_ShouldReturnZero()
    {
        // Arrange
        decimal[] balances = Array.Empty<decimal>();

        // Act - C# 14: Implicit conversion
        var total = SpanHelpers.CalculateTotal(balances);

        // Assert
        total.Should().Be(0m);
    }

    [Fact]
    public void CalculateTotal_WithSingleValue_ShouldReturnThatValue()
    {
        // Arrange
        decimal[] balances = { 1234.56m };

        // Act
        var total = SpanHelpers.CalculateTotal(balances);

        // Assert
        total.Should().Be(1234.56m);
    }

    [Fact]
    public void ModifyBalances_WithMultiplier_ShouldModifyAllValues()
    {
        // Arrange
        decimal[] balances = { 100m, 200m, 300m };
        var multiplier = 1.1m; // 10% increase

        // Act - C# 14: Implicit conversion from array to Span<decimal>
        SpanHelpers.ModifyBalances(balances, multiplier);

        // Assert
        balances[0].Should().Be(110m);
        balances[1].Should().Be(220m);
        balances[2].Should().Be(330m);
    }

    [Fact]
    public void GetTotalBalance_WithArrayParameter_ShouldUseImplicitConversion()
    {
        // Arrange
        decimal[] accountBalances = { 1000m, 2000m, 3000m, 4000m };

        // Act - C# 14: Tests the implicit Span conversion wrapper
        var total = SpanHelpers.GetTotalBalance(accountBalances);

        // Assert
        total.Should().Be(10000m);
    }

    [Fact]
    public void CalculateTotal_WithTwoValues_ShouldReturnCorrectSum()
    {
        // Arrange
        decimal[] values = { 100m, 200m };

        // Act - C# 14: Implicit Span conversion
        var result = SpanHelpers.CalculateTotal(values);

        // Assert
        result.Should().Be(300m);
    }

    [Fact]
    public void CalculateTotal_WithFiveValues_ShouldReturnCorrectSum()
    {
        // Arrange
        decimal[] values = { 1m, 2m, 3m, 4m, 5m };

        // Act
        var result = SpanHelpers.CalculateTotal(values);

        // Assert
        result.Should().Be(15m);
    }

    [Fact]
    public void ModifyBalances_WithZeroMultiplier_ShouldSetAllToZero()
    {
        // Arrange
        decimal[] balances = { 100m, 200m, 300m };

        // Act
        SpanHelpers.ModifyBalances(balances, 0m);

        // Assert
        foreach (var balance in balances)
        {
            balance.Should().Be(0m);
        }
    }

    [Fact]
    public void CalculateTotal_WithLargeArray_ShouldHandleCorrectly()
    {
        // Arrange
        var balances = Enumerable.Range(1, 1000).Select(i => (decimal)i).ToArray();

        // Act - C# 14: Implicit Span conversion with large array
        var total = SpanHelpers.CalculateTotal(balances);

        // Assert
        total.Should().Be(500500m); // Sum of 1 to 1000
    }
}
