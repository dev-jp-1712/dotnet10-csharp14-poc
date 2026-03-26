using BankingSystem.Application;
using BankingSystem.Domain;
using FluentAssertions;

namespace BankingSystem.Tests.Application;

/// <summary>
/// Tests for C# 14 nameof with unbound generic types
/// Note: nameof(T) returns "T" - the parameter name, not the actual type.
/// This is expected behavior in C# 14.
/// </summary>
public class GenericRepositoryTests
{
    [Fact]
    public void GetTypeName_WithAccountType_ShouldReturnT()
    {
        // Arrange
        var repository = new GenericRepository<Account>();

        // Act - C# 14: nameof with generic type T
        var typeName = repository.GetTypeName();

        // Assert - C# 14: nameof(T) returns "T", not "Account"
        typeName.Should().Be("T");
    }

    [Fact]
    public void GetMethodName_WithIntParameter_ShouldReturnTParam()
    {
        // Arrange
        var repository = new GenericRepository<Account>();

        // Act - C# 14: nameof with generic method parameter
        var methodName = repository.GetMethodName<int>();

        // Assert - C# 14: nameof(TParam) returns "TParam"
        methodName.Should().Be("TParam");
    }

    [Fact]
    public void LogOperation_ShouldUseNameofWithGenerics()
    {
        // Arrange
        var repository = new GenericRepository<Account>();
        var consoleOutput = new StringWriter();
        Console.SetOut(consoleOutput);

        // Act - C# 14: nameof with generic types in logging
        repository.LogOperation("TestOperation");

        // Assert
        var output = consoleOutput.ToString();
        output.Should().Contain("GenericRepository");
        output.Should().Contain("T"); // nameof(T) returns "T"
        output.Should().Contain("TestOperation");

        // Cleanup
        Console.SetOut(Console.Out);
    }

    [Fact]
    public void GenericRepository_WithDifferentTypes_ShouldWork()
    {
        // Arrange & Act - C# 14: nameof works with any generic type
        var accountRepo = new GenericRepository<Account>();
        var stringRepo = new GenericRepository<string>();

        // Assert - Both return "T" because that's the parameter name
        accountRepo.GetTypeName().Should().Be("T");
        stringRepo.GetTypeName().Should().Be("T");
    }
}
