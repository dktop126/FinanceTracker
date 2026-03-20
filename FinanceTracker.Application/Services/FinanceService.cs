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

    public FinanceService(ITransactionRepository transactionRepository, ICategoryRepository categoryRepository, IAccountRepository accountRepository)
    {
        _transactionRepository = transactionRepository;
        _categoryRepository = categoryRepository;
        _accountRepository = accountRepository;
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
            Type = (TransactionType)dto.TransactionType
        };
        
        if (transaction.Type == TransactionType.Income)
            account.Balance += transaction.Amount;
        else if (transaction.Type == TransactionType.Expense)
            account.Balance -= transaction.Amount;
        
        await _transactionRepository.AddAsync(transaction);
        await _accountRepository.UpdateAsync(account);

        return transaction.Id;
    }

    public async Task<Guid> CreateCategoryAsync(CreateCategoryDto dto)
    {
        var category = new Category
        {
            Name = dto.Name,
            Icon = dto.Icon
        };
        await _categoryRepository.AddAsync(category);
        
        return category.Id;

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

    public async Task<IEnumerable<TransactionDto>> GetAllTransactionsAsync()
    {
        var transactions = await _transactionRepository.GetAllAsync();
        return transactions.Select(x => new TransactionDto
        {
            Account = x.Account,
            Amount = x.Amount,
            Category = x.Category,
            Currency = x.Currency,
            Date = x.CreatedOn.ToLongDateString(),
            TransactionId = x.Id,
            TransactionType = x.Type
        });
    }
    public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
    {
        return await _categoryRepository.GetAllAsync();
    }

    public async Task DeleteCategoryAsync(Guid id)
    {
        var category = await _categoryRepository.GetByIdAsync(id);
        if (category == null) throw new KeyNotFoundException("Category not found");
        await _categoryRepository.DeleteAsync(id);
    }

    public async Task<AccountDto?> GetAccountAsync(Guid accountId)
    {
        var account = await _accountRepository.GetByIdAsync(accountId);
        if (account == null) throw new KeyNotFoundException("Account not found");
        return new AccountDto
        {
            AccountId = account.Id,
            AccountName = account.Name,
            Balance = account.Balance,
            Currency = account.Currency
        };
    }

    public async Task<IEnumerable<AccountDto>> GetAllAccountsAsync()
    {
        var accounts = await _accountRepository.GetAllAsync();
        return accounts.Select(x => new AccountDto
            {
                AccountId = x.Id,
                AccountName = x.Name,
                Balance = x.Balance,
                Currency = x.Currency
            }
        );
    }

    public Task<decimal> GetAccountBalanceAsync(Guid accountId)
    {
        //todo тело метода
        return null;
    }
}