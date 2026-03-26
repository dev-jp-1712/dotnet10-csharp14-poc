namespace BankingSystem.Domain;

// C# 14: field keyword demonstration
public class Account
{
    public int Id { get; set; }

    // C# 14: field keyword - compiler generates backing field automatically
    // 'field' keyword refers to the backing field
    public string AccountNumber 
    { 
        get => field;
        set => field = string.IsNullOrWhiteSpace(value) 
            ? throw new ArgumentException("Account number cannot be empty") 
            : value.Trim().ToUpper();
    } = string.Empty;

    // C# 14: Another field keyword example with validation
    public string AccountHolder 
    { 
        get => field;
        set => field = string.IsNullOrWhiteSpace(value)
            ? throw new ArgumentException("Account holder cannot be empty")
            : value.Trim();
    } = string.Empty;

    public decimal Balance { get; set; }
    public DateTime CreatedAt { get; set; }

    public Account()
    {
        CreatedAt = DateTime.UtcNow;
    }

    public Account(string accountNumber, string accountHolder, decimal initialBalance)
    {
        AccountNumber = accountNumber;
        AccountHolder = accountHolder;
        Balance = initialBalance;
        CreatedAt = DateTime.UtcNow;
    }
}
