namespace BankingSystem.Api.DTOs;

public class CreateAccountRequest
{
    public string AccountNumber { get; set; } = string.Empty;
    public string AccountHolder { get; set; } = string.Empty;
    public decimal InitialBalance { get; set; }
}
