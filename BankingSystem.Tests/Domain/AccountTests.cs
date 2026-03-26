using BankingSystem.Domain;
using FluentAssertions;

namespace BankingSystem.Tests.Domain;

/// <summary>
/// Tests for C# 14 'field' keyword feature in Account entity
/// </summary>
public class AccountTests
{
    [Fact]
    public void Account_Constructor_ShouldInitializeProperties()
    {
        // Arrange
        var accountNumber = "ACC001";
        var accountHolder = "John Doe";
        var initialBalance = 1000m;

        // Act
        var account = new Account(accountNumber, accountHolder, initialBalance);

        // Assert
        account.AccountNumber.Should().Be("ACC001");
        account.AccountHolder.Should().Be("John Doe");
        account.Balance.Should().Be(1000m);
        account.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void AccountNumber_Setter_WithValidValue_ShouldTrimAndUpperCase()
    {
        // Arrange
        var account = new Account();

        // Act - C# 14: field keyword with validation
        account.AccountNumber = "  acc123  ";

        // Assert
        account.AccountNumber.Should().Be("ACC123");
    }

    [Fact]
    public void AccountNumber_Setter_WithEmptyValue_ShouldThrowException()
    {
        // Arrange
        var account = new Account();

        // Act & Assert - C# 14: field keyword validation
        var act = () => account.AccountNumber = "";

        act.Should().Throw<ArgumentException>()
            .WithMessage("Account number cannot be empty");
    }

    [Fact]
    public void AccountNumber_Setter_WithWhitespace_ShouldThrowException()
    {
        // Arrange
        var account = new Account();

        // Act & Assert
        var act = () => account.AccountNumber = "   ";

        act.Should().Throw<ArgumentException>()
            .WithMessage("Account number cannot be empty");
    }

    [Fact]
    public void AccountHolder_Setter_WithValidValue_ShouldTrim()
    {
        // Arrange
        var account = new Account();

        // Act - C# 14: field keyword with validation
        account.AccountHolder = "  John Doe  ";

        // Assert
        account.AccountHolder.Should().Be("John Doe");
    }

    [Fact]
    public void AccountHolder_Setter_WithEmptyValue_ShouldThrowException()
    {
        // Arrange
        var account = new Account();

        // Act & Assert - C# 14: field keyword validation
        var act = () => account.AccountHolder = "";

        act.Should().Throw<ArgumentException>()
            .WithMessage("Account holder cannot be empty");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(100.50)]
    [InlineData(1000000)]
    public void Balance_Setter_WithValidValue_ShouldSetCorrectly(decimal balance)
    {
        // Arrange
        var account = new Account();

        // Act
        account.Balance = balance;

        // Assert
        account.Balance.Should().Be(balance);
    }

    [Fact]
    public void Account_DefaultConstructor_ShouldSetCreatedAtToUtcNow()
    {
        // Act
        var account = new Account();

        // Assert
        account.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        account.CreatedAt.Kind.Should().Be(DateTimeKind.Utc);
    }
}
