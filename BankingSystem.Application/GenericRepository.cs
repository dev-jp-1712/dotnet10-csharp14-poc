namespace BankingSystem.Application;

// C# 14: nameof with unbound generic types
public class GenericRepository<T> where T : class
{
    // C# 14: Can use nameof with unbound generic type T
    public string GetTypeName() => nameof(T);
    
    // C# 14: nameof with generic method parameter
    public string GetMethodName<TParam>() => nameof(TParam);
    
    // Example: Log generic type information
    public void LogOperation(string operation)
    {
        Console.WriteLine($"[{nameof(GenericRepository<T>)}] Operation '{operation}' on type {nameof(T)}");
    }
}
