using FinanceTracker.Application.DTOs;
using FinanceTracker.Domain.Entities;

namespace FinanceTracker.Application.Interfaces;

public interface IFinanceService
{
    Task<Guid> CreateTransactionAsync (CreateTransactionDto dto);
    Task<Guid> CreateCategoryAsync (CreateCategoryDto dto);
    Task<Guid> CreateAccountAsync (CreateAccountDto dto);
    Task<IEnumerable<TransactionDto>> GetAllTransactionsAsync();
    Task<AccountDto?> GetAccountAsync (Guid accountId);
    Task<IEnumerable<AccountDto>> GetAllAccountsAsync();
    Task<decimal> GetAccountBalanceAsync (Guid accountId);
    Task<IEnumerable<Category>> GetAllCategoriesAsync ();
    Task DeleteCategoryAsync (Guid categoryId);
    Task DeleteTransactionAsync (Guid transactionId);
    
    
    
}