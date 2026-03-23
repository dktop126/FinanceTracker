using FinanceTracker.Application.DTOs;
using FinanceTracker.Application.Interfaces;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Interfaces;
using FinanceTracker.Domain.Enums;

namespace FinanceTracker.Application.Services;

public class FinanceService : IFinanceService
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IAccountRepository _accountRepository;

    public FinanceService(ITransactionRepository transactionRepository, ICategoryRepository categoryRepository,
        IAccountRepository accountRepository)
    {
        _transactionRepository = transactionRepository;
        _categoryRepository = categoryRepository;
        _accountRepository = accountRepository;
    }

    public async Task<Guid> CreateAccountAsync(CreateAccountDto dto)
    {
        var account = new Account
        {
            Name = dto.Name,
            Balance = dto.Balance,
            Currency = dto.Currency
        };

        await _accountRepository.AddAsync(account);
        return account.Id;
    }

    public async Task<AccountDto?> GetAccountAsync(Guid accountId)
    {
        var account = await _accountRepository.GetByIdAsync(accountId);
        if (account == null) throw new KeyNotFoundException("Account not found");
        return new AccountDto
        {
            Id = account.Id,
            Name = account.Name,
            Balance = account.Balance,
            Currency = account.Currency
        };
    }

    public async Task<IEnumerable<AccountDto>> GetAllAccountsAsync()
    {
        var accounts = await _accountRepository.GetAllAsync();
        return accounts.Select(x => new AccountDto
            {
                Id = x.Id,
                Name = x.Name,
                Balance = x.Balance,
                Currency = x.Currency
            }
        );
    }

    public async Task<decimal> GetAccountBalanceAsync(Guid accountId)
    {
        var account = await _accountRepository.GetByIdAsync(accountId);
        if (account == null) throw new KeyNotFoundException("Account not found");
        return account.Balance;
    }

    public async Task<Guid> CreateTransactionAsync(CreateTransactionDto dto)
    {
        var account = await _accountRepository.GetByIdAsync(dto.AccountId);
        if (account == null) throw new KeyNotFoundException("Account not found");

        var transaction = new Transaction
        {
            Amount = dto.Amount,
            Description = dto.Description,
            AccountId = dto.AccountId,
            CategoryId = dto.CategoryId,
            Type = (TransactionType)dto.TransactionType,
            Account = account,
            Category = await _categoryRepository.GetByIdAsync(dto.CategoryId)
        };

        if (transaction.Type == TransactionType.Income)
            account.Balance += transaction.Amount;
        else if (transaction.Type == TransactionType.Expense)
            account.Balance -= transaction.Amount;

        await _transactionRepository.AddAsync(transaction);
        await _accountRepository.UpdateAsync(account);

        return transaction.Id;
    }

    public async Task<IEnumerable<TransactionDto>> GetAllTransactionsAsync()
    {
        var transactions = await _transactionRepository.GetAllAsync();
        var transactionDtos = transactions.Select(x => new TransactionDto
        {
            AccountId = x.AccountId,
            AccountName = x.Account?.Name,
            Amount = x.Amount,
            CategoryId = x.CategoryId,
            CategoryName = x.Category?.Name,
            Currency = x.Currency,
            Date = x.CreatedOn,
            TransactionId = x.Id,
            TransactionType = x.Type
        }).OrderByDescending(x => x.Date);
        return transactionDtos;
    }

    public async Task DeleteTransactionAsync(Guid transactionId)
    {
        var transaction = await _transactionRepository.GetByIdAsync(transactionId);
        if (transaction == null) throw new KeyNotFoundException("Transaction not found");
        var account = await _accountRepository.GetByIdAsync(transaction.AccountId);
        if (account == null) throw new KeyNotFoundException("Account not found");
        if (transaction.Type == TransactionType.Income)
            account.Balance -= transaction.Amount;
        else if (transaction.Type == TransactionType.Expense)
            account.Balance += transaction.Amount;

        await _accountRepository.UpdateAsync(account);
        await _transactionRepository.DeleteAsync(transactionId);
    }

    public async Task<Guid> CreateCategoryAsync(CreateCategoryDto dto)
    {
        var category = new Category
        {
            Name = dto.Name,
            Icon = dto.Icon,
            Type = dto.Type
        };
        await _categoryRepository.AddAsync(category);

        return category.Id;
    }

    public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync()
    {
        var categories = await _categoryRepository.GetAllAsync();
        var categoriesDtos = categories.Select(x => new CategoryDto
            {
                Id = x.Id,
                Name = x.Name,
                Icon = x.Icon,
                Type = x.Type
            }
        );
        return categoriesDtos;
    }

    public async Task DeleteCategoryAsync(Guid id)
    {
        var category = await _categoryRepository.GetByIdAsync(id);
        if (category == null) throw new KeyNotFoundException("Category not found");
        await _categoryRepository.DeleteAsync(id);
    }
}