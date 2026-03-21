using FinanceTracker.Application.DTOs;
using FinanceTracker.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FinanceTracker.API.Controllers;
[ApiController]
[Route("api/[controller]")]
public class FinanceController : ControllerBase
{
    private readonly IFinanceService _financeService;
    
    public FinanceController (IFinanceService financeService)
        {
            _financeService = financeService;
        }

    [HttpPost("transactions")]
    public async Task<IActionResult> CreateTransaction([FromBody] CreateTransactionDto dto)
    {
        var id = await _financeService.CreateTransactionAsync(dto);
        return Ok(new { TransactionId = id });
    }

    [HttpGet("transactions")]
    public async Task<IActionResult> GetTransactions()
    {
        var transactions = await _financeService.GetAllTransactionsAsync();
        return Ok(transactions);
    }

    [HttpDelete("transactions/{id}")]
    public async Task<IActionResult> DeleteTransactionAsync(Guid id)
    {
        await _financeService.DeleteTransactionAsync(id);
        return NoContent();
    }

    [HttpPost("categories")]
    public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryDto dto)
    {
        var id = await _financeService.CreateCategoryAsync(dto);
        return Ok(new { CategoryId = id });
    }

    [HttpGet("categories")]
    public async Task<IActionResult> GetCategories()
    {
        var categories = await _financeService.GetAllCategoriesAsync();
        return Ok(categories);
    }

    [HttpDelete("categories/{id}")]
    public async Task<IActionResult> DeleteCategoryAsync(Guid id)
    {
        await _financeService.DeleteCategoryAsync(id);
        return NoContent();
    }

    [HttpPost("accounts")]
    public async Task<IActionResult> CreateAccount([FromBody] CreateAccountDto dto)
    {
        var id = await _financeService.CreateAccountAsync(dto);
        return Ok(new { Id = id });
    }

    [HttpGet("accounts/{id}")]
    public async Task<IActionResult> GetAccount(Guid id)
    {
        var account = await _financeService.GetAccountAsync(id);
        return Ok(account);
    }

    [HttpGet("accounts")]
    public async Task<IActionResult> GetAccounts()
    {
        var accounts = await _financeService.GetAllAccountsAsync();
        return Ok(accounts);
    }
    
}