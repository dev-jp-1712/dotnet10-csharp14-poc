namespace BankingSystem.Application;

// C# 14: Implicit conversions to Span<T> / ReadOnlySpan<T>
public class SpanHelpers
{
    // C# 14: Implicit conversion from array to ReadOnlySpan<T>
    public static decimal CalculateTotal(ReadOnlySpan<decimal> amounts)
    {
        decimal total = 0;
        foreach (var amount in amounts)
        {
            total += amount;
        }
        return total;
    }
    
    // C# 14: Can pass array directly, implicitly converts to Span
    public static void ModifyBalances(Span<decimal> balances, decimal multiplier)
    {
        for (int i = 0; i < balances.Length; i++)
        {
            balances[i] *= multiplier;
        }
    }
    
    // Usage example:
    public static decimal GetTotalBalance(decimal[] accountBalances)
    {
        // C# 14: Array implicitly converts to ReadOnlySpan<decimal>
        return CalculateTotal(accountBalances);
    }
}
