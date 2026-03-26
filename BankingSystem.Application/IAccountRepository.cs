using BankingSystem.Domain;

namespace BankingSystem.Application;

public interface IAccountRepository
{
    Task<Account?> GetByIdAsync(int id);
    Task<Account?> GetByAccountNumberAsync(string accountNumber);
    Task<IEnumerable<Account>> GetAllAsync();
    Task<Account> CreateAsync(Account account);
    Task UpdateAsync(Account account);
    Task<bool> ExistsAsync(string accountNumber);
}
