using BankingSystem.Domain;

namespace BankingSystem.Application;

public class AccountService
{
    private readonly IAccountRepository _repository;

    public AccountService(IAccountRepository repository)
    {
        _repository = repository;
    }

    public async Task<Account?> GetAccountAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task<Account?> GetAccountByNumberAsync(string accountNumber)
    {
        return await _repository.GetByAccountNumberAsync(accountNumber);
    }

    public async Task<IEnumerable<Account>> GetAllAccountsAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<Account> CreateAccountAsync(string accountNumber, string accountHolder, decimal initialBalance)
    {
        if (await _repository.ExistsAsync(accountNumber))
        {
            throw new InvalidOperationException($"Account with number {accountNumber} already exists.");
        }

        var account = new Account
        {
            AccountNumber = accountNumber,
            AccountHolder = accountHolder,
            Balance = initialBalance
        };

        return await _repository.CreateAsync(account);
    }

    public async Task DepositAsync(int accountId, decimal amount)
    {
        if (amount <= 0)
        {
            throw new ArgumentException("Deposit amount must be positive.", nameof(amount));
        }

        var account = await _repository.GetByIdAsync(accountId);

        // C# 14: Null-conditional assignment (obj?.Prop = value)
        // Only assigns if account is not null
        account?.Balance += amount;

        if (account != null)
        {
            await _repository.UpdateAsync(account);
        }
        else
        {
            throw new InvalidOperationException($"Account with ID {accountId} not found.");
        }
    }

    public async Task WithdrawAsync(int accountId, decimal amount)
    {
        if (amount <= 0)
        {
            throw new ArgumentException("Withdrawal amount must be positive.", nameof(amount));
        }

        var account = await _repository.GetByIdAsync(accountId);
        if (account == null)
        {
            throw new InvalidOperationException($"Account with ID {accountId} not found.");
        }

        if (account.Balance < amount)
        {
            throw new InvalidOperationException("Insufficient funds.");
        }

        // C# 14: Null-conditional assignment
        account?.Balance -= amount;

        await _repository.UpdateAsync(account);
    }
}
