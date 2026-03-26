namespace BankingSystem.Application;

// C# 14: Lambda parameter modifiers without explicit types
public class AccountFilters
{
    // C# 14: Lambda with parameter modifier (simplified version)
    public static void ModifyBalances(List<decimal> balances, Action<decimal> modifier)
    {
        for (int i = 0; i < balances.Count; i++)
        {
            var balance = balances[i];
            modifier(balance);
        }
    }

    // Practical example: Filter accounts by criteria
    public static List<decimal> FilterByAmount(List<decimal> amounts, Func<decimal, bool> predicate)
    {
        // C# 14: Lambda without explicit type - compiler infers
        return amounts.Where(x => predicate(x)).ToList();
    }

    // C# 14: Multiple lambda parameters without types
    public static decimal ApplyOperation(decimal a, decimal b, Func<decimal, decimal, decimal> operation)
    {
        // Lambda infers types from Func signature
        return operation(a, b);
    }
}
