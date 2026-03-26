using BankingSystem.Api.DTOs;
using BankingSystem.Application;
using BankingSystem.Domain;
using Microsoft.AspNetCore.Mvc;

namespace BankingSystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountsController : ControllerBase
{
    private readonly AccountService _accountService;
    private readonly ILogger<AccountsController> _logger;

    public AccountsController(AccountService accountService, ILogger<AccountsController> logger)
    {
        _accountService = accountService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAccounts()
    {
        var accounts = await _accountService.GetAllAccountsAsync();
        return Ok(accounts);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetAccount(int id)
    {
        var account = await _accountService.GetAccountAsync(id);
        if (account == null)
        {
            return NotFound(new { Message = $"Account with ID {id} not found." });
        }
        return Ok(account);
    }

    [HttpGet("by-number/{accountNumber}")]
    public async Task<IActionResult> GetAccountByNumber(string accountNumber)
    {
        var account = await _accountService.GetAccountByNumberAsync(accountNumber);
        if (account == null)
        {
            return NotFound(new { Message = $"Account with number {accountNumber} not found." });
        }
        return Ok(account);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAccount([FromBody] CreateAccountRequest request)
    {
        try
        {
            var account = await _accountService.CreateAccountAsync(
                request.AccountNumber,
                request.AccountHolder,
                request.InitialBalance);

            return CreatedAtAction(nameof(GetAccount), new { id = account.Id }, account);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { Message = ex.Message });
        }
    }

    [HttpPost("{id:int}/deposit")]
    public async Task<IActionResult> Deposit(int id, [FromBody] TransactionRequest request)
    {
        try
        {
            // C# 14: Null-conditional assignment used in service
            await _accountService.DepositAsync(id, request.Amount);
            var account = await _accountService.GetAccountAsync(id);
            return Ok(new { Message = "Deposit successful", Account = account });
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { Message = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

    [HttpPost("{id:int}/withdraw")]
    public async Task<IActionResult> Withdraw(int id, [FromBody] TransactionRequest request)
    {
        try
        {
            await _accountService.WithdrawAsync(id, request.Amount);
            var account = await _accountService.GetAccountAsync(id);
            return Ok(new { Message = "Withdrawal successful", Account = account });
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { Message = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

    // C# 14: Implicit Span conversion demonstration
    [HttpPost("calculate-total")]
    public IActionResult CalculateTotal([FromBody] decimal[] balances)
    {
        // C# 14: Array implicitly converts to ReadOnlySpan<decimal>
        var total = SpanHelpers.CalculateTotal(balances);
        return Ok(new { Total = total, Count = balances.Length });
    }
}
