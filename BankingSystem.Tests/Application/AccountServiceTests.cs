using BankingSystem.Application;
using BankingSystem.Domain;
using FluentAssertions;
using Moq;

namespace BankingSystem.Tests.Application;

/// <summary>
/// Tests for AccountService with C# 14 null-conditional assignment
/// </summary>
public class AccountServiceTests
{
    private readonly Mock<IAccountRepository> _repositoryMock;
    private readonly AccountService _service;

    public AccountServiceTests()
    {
        _repositoryMock = new Mock<IAccountRepository>();
        _service = new AccountService(_repositoryMock.Object);
    }

    [Fact]
    public async Task GetAccountAsync_WithValidId_ShouldReturnAccount()
    {
        // Arrange
        var account = new Account("ACC001", "John Doe", 1000m) { Id = 1 };
        _repositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(account);

        // Act
        var result = await _service.GetAccountAsync(1);

        // Assert
        result.Should().NotBeNull();
        result!.AccountNumber.Should().Be("ACC001");
        result.AccountHolder.Should().Be("John Doe");
        result.Balance.Should().Be(1000m);
    }

    [Fact]
    public async Task GetAccountAsync_WithInvalidId_ShouldReturnNull()
    {
        // Arrange
        _repositoryMock.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Account?)null);

        // Act
        var result = await _service.GetAccountAsync(999);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task CreateAccountAsync_WithValidData_ShouldCreateAccount()
    {
        // Arrange
        _repositoryMock.Setup(r => r.ExistsAsync("ACC001")).ReturnsAsync(false);
        _repositoryMock.Setup(r => r.CreateAsync(It.IsAny<Account>()))
            .ReturnsAsync((Account a) => { a.Id = 1; return a; });

        // Act
        var result = await _service.CreateAccountAsync("ACC001", "John Doe", 1000m);

        // Assert
        result.Should().NotBeNull();
        result.AccountNumber.Should().Be("ACC001");
        result.AccountHolder.Should().Be("John Doe");
        result.Balance.Should().Be(1000m);
        _repositoryMock.Verify(r => r.CreateAsync(It.IsAny<Account>()), Times.Once);
    }

    [Fact]
    public async Task CreateAccountAsync_WithDuplicateAccountNumber_ShouldThrowException()
    {
        // Arrange
        _repositoryMock.Setup(r => r.ExistsAsync("ACC001")).ReturnsAsync(true);

        // Act
        var act = async () => await _service.CreateAccountAsync("ACC001", "John Doe", 1000m);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Account with number ACC001 already exists.");
        _repositoryMock.Verify(r => r.CreateAsync(It.IsAny<Account>()), Times.Never);
    }

    [Fact]
    public async Task DepositAsync_WithValidAmount_ShouldIncreaseBalance()
    {
        // Arrange - C# 14: null-conditional assignment test
        var account = new Account("ACC001", "John Doe", 1000m) { Id = 1 };
        _repositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(account);
        _repositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Account>())).Returns(Task.CompletedTask);

        // Act
        await _service.DepositAsync(1, 500m);

        // Assert
        account.Balance.Should().Be(1500m);
        _repositoryMock.Verify(r => r.UpdateAsync(account), Times.Once);
    }

    [Fact]
    public async Task DepositAsync_WithNegativeAmount_ShouldThrowException()
    {
        // Arrange
        var account = new Account("ACC001", "John Doe", 1000m) { Id = 1 };
        _repositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(account);

        // Act
        var act = async () => await _service.DepositAsync(1, -500m);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("Deposit amount must be positive.*");
        _repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Account>()), Times.Never);
    }

    [Fact]
    public async Task DepositAsync_WithNonExistentAccount_ShouldThrowException()
    {
        // Arrange - C# 14: null-conditional assignment handles null
        _repositoryMock.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Account?)null);

        // Act
        var act = async () => await _service.DepositAsync(999, 500m);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Account with ID 999 not found.");
    }

    [Fact]
    public async Task WithdrawAsync_WithValidAmount_ShouldDecreaseBalance()
    {
        // Arrange - C# 14: null-conditional assignment test
        var account = new Account("ACC001", "John Doe", 1000m) { Id = 1 };
        _repositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(account);
        _repositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Account>())).Returns(Task.CompletedTask);

        // Act
        await _service.WithdrawAsync(1, 300m);

        // Assert
        account.Balance.Should().Be(700m);
        _repositoryMock.Verify(r => r.UpdateAsync(account), Times.Once);
    }

    [Fact]
    public async Task WithdrawAsync_WithInsufficientFunds_ShouldThrowException()
    {
        // Arrange
        var account = new Account("ACC001", "John Doe", 500m) { Id = 1 };
        _repositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(account);

        // Act
        var act = async () => await _service.WithdrawAsync(1, 1000m);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Insufficient funds.");
        account.Balance.Should().Be(500m); // Balance unchanged
    }

    [Fact]
    public async Task WithdrawAsync_WithNonExistentAccount_ShouldThrowException()
    {
        // Arrange
        _repositoryMock.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Account?)null);

        // Act
        var act = async () => await _service.WithdrawAsync(999, 500m);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Account with ID 999 not found.");
    }

    [Fact]
    public async Task GetAllAccountsAsync_ShouldReturnAllAccounts()
    {
        // Arrange
        var accounts = new List<Account>
        {
            new("ACC001", "John Doe", 1000m) { Id = 1 },
            new("ACC002", "Jane Smith", 2000m) { Id = 2 }
        };
        _repositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(accounts);

        // Act
        var result = await _service.GetAllAccountsAsync();

        // Assert
        result.Should().HaveCount(2);
        result.Should().Contain(a => a.AccountNumber == "ACC001");
        result.Should().Contain(a => a.AccountNumber == "ACC002");
    }
}
