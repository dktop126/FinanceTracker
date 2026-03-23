using FinanceTracker.Application.DTOs;
using FinanceTracker.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FinanceTracker.API.Controllers;
[ApiController]
[Route("api/[controller]")]
public class FinanceController : ControllerBase
{
    private readonly IAccountService _accountService;
    private readonly ICategoryService _categoryService;
    private readonly ITransactionService _transactionService;
    
    public FinanceController (IAccountService accountService, ICategoryService categoryService, ITransactionService transactionService)
        {
            _accountService = accountService;
            _categoryService = categoryService;
            _transactionService = transactionService;
        }

    [HttpPost("transactions")]
    public async Task<IActionResult> CreateTransaction([FromBody] CreateTransactionDto dto)
    {
        var id = await _transactionService.CreateTransactionAsync(dto);
        return Ok(new { TransactionId = id });
    }

    [HttpGet("transactions")]
    public async Task<IActionResult> GetTransactions()
    {
        var transactions = await _transactionService.GetAllTransactionsAsync();
        return Ok(transactions);
    }

    [HttpDelete("transactions/{id}")]
    public async Task<IActionResult> DeleteTransactionAsync(Guid id)
    {
        await _transactionService.DeleteTransactionAsync(id);
        return NoContent();
    }

    [HttpPost("categories")]
    public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryDto dto)
    {
        var id = await _categoryService.CreateCategoryAsync(dto);
        return Ok(new { CategoryId = id });
    }

    [HttpGet("categories")]
    public async Task<IActionResult> GetCategories()
    {
        var categories = await _categoryService.GetAllCategoriesAsync();
        return Ok(categories);
    }

    [HttpDelete("categories/{id}")]
    public async Task<IActionResult> DeleteCategoryAsync(Guid id)
    {
        await _categoryService.DeleteCategoryAsync(id);
        return NoContent();
    }

    [HttpPost("accounts")]
    public async Task<IActionResult> CreateAccount([FromBody] CreateAccountDto dto)
    {
        var id = await _accountService.CreateAccountAsync(dto);
        return Ok(new { Id = id });
    }

    [HttpGet("accounts/{id}")]
    public async Task<IActionResult> GetAccount(Guid id)
    {
        var account = await _accountService.GetAccountAsync(id);
        return Ok(account);
    }

    [HttpGet("accounts")]
    public async Task<IActionResult> GetAccounts()
    {
        var accounts = await _accountService.GetAllAccountsAsync();
        return Ok(accounts);
    }
    
}