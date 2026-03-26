# Banking System - C# 14 & .NET 10 POC

## 📋 Overview

This repository demonstrates a **Proof of Concept (POC)** showcasing **ONLY** the latest features of **C# 14** and **.NET 10** through a practical Banking System API. The project follows clean architecture with proper separation of concerns and coding standards.

## 🎯 Purpose

This POC is designed to help developers:
- Understand and explore **actual new features** in C# 14
- Learn about .NET 10 capabilities and improvements
- See practical implementations with minimal, real-world code
- Follow proper project structure and coding standards

## 🏗️ Project Structure

The solution follows **Clean Architecture** with proper folder organization:

```
BankingSystem/
├── BankingSystem.Domain/
│   └── Account.cs (C# 14: field keyword)
├── BankingSystem.Application/
│   ├── IAccountRepository.cs
│   ├── AccountService.cs (C# 14: null-conditional assignment)
│   ├── AccountFilters.cs (C# 14: lambda modifiers)
│   ├── GenericRepository.cs (C# 14: nameof with generics)
│   └── SpanHelpers.cs (C# 14: implicit Span conversions)
├── BankingSystem.Infrastructure/
│   ├── BankingDbContext.cs (EF Core 10)
│   └── AccountRepository.cs
├── BankingSystem.Api/
│   ├── Controllers/
│   │   └── AccountsController.cs
│   ├── DTOs/
│   │   ├── CreateAccountRequest.cs ✅
│   │   └── TransactionRequest.cs ✅
│   └── Program.cs (.NET 10: Native OpenAPI, Health Checks)
└── BankingSystem.Tests/
    └── Unit tests
```

### Key Structure Improvements
- ✅ **DTOs in separate folder** (not in controllers)
- ✅ **Clean separation** of domain, application, infrastructure
- ✅ **No unnecessary middleware** (removed PerformanceMiddleware)
- ✅ **Focused on demonstrating features** (minimal code)

## ✨ C# 14 Features Demonstrated

### 1. **field Keyword** (Field-backed properties)
**What:** Compiler-generated backing field with `field` keyword for direct access in property accessors.

**Location:** [`BankingSystem.Domain\Account.cs`](BankingSystem.Domain/Account.cs)
```csharp
// C# 14: field keyword - no manual backing field needed
public string AccountNumber 
{ 
    get => field;
    set => field = string.IsNullOrWhiteSpace(value) 
        ? throw new ArgumentException("Account number cannot be empty") 
        : value.Trim().ToUpper();
} = string.Empty;
```

**Test:** `POST /api/accounts` with empty account number - validation triggers

---

### 2. **Null-conditional Assignment** (obj?.Prop = value)
**What:** Assign to a property only if the object is not null.

**Location:** [`BankingSystem.Application\AccountService.cs`](BankingSystem.Application/AccountService.cs)
```csharp
var account = await _repository.GetByIdAsync(accountId);

// C# 14: Only assigns if account is not null
account?.Balance += amount;
```

**Test:** `POST /api/accounts/1/deposit {"amount": 100}`

---

### 3. **nameof with Unbound Generic Types**
**What:** Use `nameof` with generic type parameters directly.

**Location:** [`BankingSystem.Application\GenericRepository.cs`](BankingSystem.Application/GenericRepository.cs)
```csharp
public class GenericRepository<T> where T : class
{
    // C# 14: nameof with generic type T
    public string GetTypeName() => nameof(T);

    public void LogOperation(string operation)
    {
        Console.WriteLine($"[{nameof(GenericRepository<T>)}] Operation on {nameof(T)}");
    }
}
```

---

### 4. **Lambda Parameter Modifiers Without Explicit Types**
**What:** Lambda expressions can infer parameter types from delegate signatures.

**Location:** [`BankingSystem.Application\AccountFilters.cs`](BankingSystem.Application/AccountFilters.cs)
```csharp
// C# 14: Lambda infers types automatically
public static List<decimal> FilterByAmount(List<decimal> amounts, Func<decimal, bool> predicate)
{
    return amounts.Where(x => predicate(x)).ToList();
}
```

---

### 5. **Implicit Conversions to Span<T> / ReadOnlySpan<T>**
**What:** Arrays automatically convert to Span types for zero-allocation performance.

**Location:** [`BankingSystem.Application\SpanHelpers.cs`](BankingSystem.Application/SpanHelpers.cs)
```csharp
// C# 14: Array implicitly converts to ReadOnlySpan<decimal>
public static decimal CalculateTotal(ReadOnlySpan<decimal> amounts)
{
    decimal total = 0;
    foreach (var amount in amounts)
        total += amount;
    return total;
}

// Usage:
decimal[] balances = { 100, 200, 300 };
var total = CalculateTotal(balances); // Implicit conversion!
```

**Test:** `POST /api/accounts/calculate-total [100, 200, 300]`

---

## 🚀 .NET 10 Features Demonstrated

### 1. **Native OpenAPI Support (v10.0.5)**
**What:** Built-in OpenAPI document generation without Swashbuckle.

**Location:** [`Program.cs`](BankingSystem.Api/Program.cs)
```csharp
// .NET 10: Native OpenAPI
builder.Services.AddOpenApi();
app.MapOpenApi();
```

**Package:** `Microsoft.AspNetCore.OpenApi v10.0.5`

**Test:** `GET /openapi/v1.json` or `GET /scalar/v1`

---

### 2. **Entity Framework Core 10 (v10.0.0)**
**What:** Latest EF Core with performance improvements.

**Packages:**
- `Microsoft.EntityFrameworkCore.InMemory v10.0.0`
- `Microsoft.EntityFrameworkCore.Sqlite v10.0.0`

**Features:**
- Improved query performance
- Better async support
- Enhanced change tracking
- Precision configuration for decimals

---

### 3. **Enhanced Health Checks (v10.0.0)**
**What:** Built-in health monitoring with EF Core integration.

**Location:** [`Program.cs`](BankingSystem.Api/Program.cs)
```csharp
// .NET 10: Enhanced Health Checks
builder.Services.AddHealthChecks().AddDbContextCheck<BankingDbContext>();
app.MapHealthChecks("/health");
```

**Package:** `Microsoft.Extensions.Diagnostics.HealthChecks.EntityFrameworkCore v10.0.0`

**Test:** `GET /health`

---

### 4. **.NET 10 Runtime Improvements**
**What:** Automatic performance improvements by targeting .NET 10.

**Benefits:**
- Better JIT compilation (improved inlining)
- Enhanced struct handling
- Optimized codegen
- Reduced memory allocations

All projects target:
```xml
<TargetFramework>net10.0</TargetFramework>
```

---

## 🛠️ Technologies Used

- **.NET 10** - Latest .NET version
- **C# 14** - Latest C# language version
- **ASP.NET Core** - Web API framework
- **Entity Framework Core 10** - ORM for data access
- **InMemory Database** - For testing without external dependencies
- **Serilog** - Structured logging
- **Scalar** - Modern API documentation
- **xUnit** - Testing framework

## 📦 NuGet Packages

| Package | Version | Purpose |
|---------|---------|---------|
| Microsoft.AspNetCore.OpenApi | 10.0.5 | Native OpenAPI support |
| Microsoft.EntityFrameworkCore.InMemory | 10.0.0 | EF Core 10 InMemory provider |
| Microsoft.Extensions.Diagnostics.HealthChecks.EntityFrameworkCore | 10.0.0 | Health checks |
| Scalar.AspNetCore | 1.2.45 | API documentation UI |
| Serilog.AspNetCore | 9.0.0 | Structured logging |

## 🚀 Getting Started

### Prerequisites

- **.NET 10 SDK** installed
- **Visual Studio 2026 (18.4.0+)** or Visual Studio Code
- Basic understanding of C# and ASP.NET Core

### Running the Application

1. **Clone the repository:**
   ```powershell
   git clone <repository-url>
   cd BankingSystem
   ```

2. **Restore dependencies:**
   ```powershell
   dotnet restore
   ```

3. **Build the solution:**
   ```powershell
   dotnet build
   ```

4. **Run the API:**
   ```powershell
   cd BankingSystem.Api
   dotnet run
   ```

5. **Access the application:**
   - **API Root:** `https://localhost:<port>/`
   - **Health Check:** `https://localhost:<port>/health`
   - **API Docs (Scalar):** `https://localhost:<port>/scalar/v1`
   - **OpenAPI Spec:** `https://localhost:<port>/openapi/v1.json`

## 📡 API Endpoints

### Account Management

| Method | Endpoint | Description | C# 14 Feature |
|--------|----------|-------------|---------------|
| GET | `/api/accounts` | Get all accounts | - |
| GET | `/api/accounts/{id}` | Get account by ID | - |
| GET | `/api/accounts/by-number/{number}` | Get by account number | - |
| POST | `/api/accounts` | Create new account | **field keyword** validation |
| POST | `/api/accounts/{id}/deposit` | Deposit money | **Null-conditional assignment** |
| POST | `/api/accounts/{id}/withdraw` | Withdraw money | **Null-conditional assignment** |
| POST | `/api/accounts/calculate-total` | Calculate total | **Implicit Span conversion** |

### System Endpoints

| Method | Endpoint | Description | .NET 10 Feature |
|--------|----------|-------------|-----------------|
| GET | `/` | API info & features | - |
| GET | `/health` | Health check | **Enhanced Health Checks** |
| GET | `/openapi/v1.json` | OpenAPI spec | **Native OpenAPI** |
| GET | `/scalar/v1` | API documentation | **Scalar UI** |

## 📝 Sample API Requests

### 1. Create Account (field keyword validation)
```json
POST /api/accounts
{
  "accountNumber": "ACC001",
  "accountHolder": "John Doe",
  "initialBalance": 1000.00
}
```

**Try with empty accountNumber to see field keyword validation!**

---

### 2. Deposit Money (null-conditional assignment)
```json
POST /api/accounts/1/deposit
{
  "amount": 500.00
}
```

---

### 3. Withdraw Money
```json
POST /api/accounts/1/withdraw
{
  "amount": 200.00
}
```

---

### 4. Calculate Total (Implicit Span conversion)
```json
POST /api/accounts/calculate-total
[100, 200, 300, 400, 500]
```

**Response:**
```json
{
  "total": 1500,
  "count": 5
}
```

---

## 🧪 Testing

Run tests using:
```powershell
dotnet test
```

## 📊 Feature Summary

### ✅ C# 14 Features (5 Features)
1. **field keyword** - Field-backed properties
2. **Null-conditional assignment** - `obj?.Prop = value`
3. **nameof with unbound generics** - `nameof(T)`
4. **Lambda parameter modifiers** - Type inference
5. **Implicit Span conversions** - Zero-allocation arrays

### ✅ .NET 10 Features (4 Features)
1. **Native OpenAPI** - v10.0.5
2. **Entity Framework Core 10** - v10.0.0
3. **Enhanced Health Checks** - v10.0.0
4. **Runtime improvements** - JIT, GC, codegen

### 📁 Proper Structure
- ✅ DTOs in `DTOs/` folder
- ✅ Clean controller (no DTOs inside)
- ✅ No unnecessary middleware
- ✅ Focused, minimal code

## 📊 Architecture Highlights

### Clean Architecture Benefits
- **Separation of Concerns** - Each layer has distinct responsibilities
- **Dependency Inversion** - Dependencies point inward toward domain
- **Testability** - Easy to unit test business logic
- **Maintainability** - Changes isolated to specific layers

### Design Patterns Used
- **Repository Pattern** - Data access abstraction
- **Dependency Injection** - Loose coupling between components
- **Service Layer Pattern** - Business logic encapsulation
- **DTO Pattern** - Data transfer objects in separate folder

## 📚 Learning Resources

- [C# 14 - What's New](https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-14)
- [.NET 10 - What's New](https://learn.microsoft.com/en-us/dotnet/core/whats-new/dotnet-10/overview)
- [ASP.NET Core in .NET 10](https://learn.microsoft.com/en-us/aspnet/core/release-notes/aspnetcore-10.0)
- [EF Core 10](https://learn.microsoft.com/en-us/ef/core/what-is-new/ef-core-10.0/whatsnew)

## 🎯 What Makes This POC Different

- ✅ **ONLY** C# 14 and .NET 10 features (no earlier versions)
- ✅ **Proper structure** - DTOs in correct location
- ✅ **Clean code** - No unnecessary components
- ✅ **Minimal** - Only essential code to demonstrate features
- ✅ **Practical** - Real-world banking domain
- ✅ **Well-documented** - Clear explanations with line numbers

## 🤝 Contributing

This is a POC/demonstration project. Feel free to fork and experiment!

## 📄 License

This project is for educational purposes.

---

**Note:** This POC uses an InMemory database, so all data is lost when the application stops. This is intentional for demonstration purposes.
