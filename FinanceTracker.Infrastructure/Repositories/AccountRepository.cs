using FinanceTracker.Domain.Interfaces;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Enums;
using FinanceTracker.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Infrastructure.Repositories;

/// <summary>
/// Репозиторий для работы с финансовыми счетами в базе данных.
/// Реализует доступ к данным с использованием Entity Framework Core.
/// </summary>
public class AccountRepository : IAccountRepository
{
    private readonly ApplicationDbContext _dbContext;
    
    /// <summary>
    /// Инициализирует новый экземпляр репозитория с контекстом базы данных.
    /// </summary>
    /// <param name="dbContext">Контекст базы данных приложения.</param>
    public AccountRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Сохраняет новый счет в базе данных.
    /// </summary>
    /// <param name="account">Сущность нового счета.</param>
    public async Task AddAsync(Account account)
    {
        await _dbContext.Accounts.AddAsync(account);
        await _dbContext.SaveChangesAsync();
    }

    /// <summary>
    /// Находит счет по его уникальному идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор счета.</param>
    /// <returns>Экземпляр счета или null, если запись не найдена.</returns>
    public async Task<Account?> GetByIdAsync(Guid id) 
        => await _dbContext.Accounts.FindAsync(id);

    /// <summary>
    /// Получает список всех активных счетов из базы данных.
    /// </summary>
    public async Task<IEnumerable<Account>> GetAllAsync()
        => await _dbContext.Accounts.ToListAsync();

    /// <summary>
    /// Обновляет существующую запись счета.
    /// </summary>
    /// <param name="account">Сущность счета с обновленными данными.</param>
    public async Task UpdateAsync(Account account) 
    { 
        _dbContext.Accounts.Update(account); 
        await _dbContext.SaveChangesAsync(); 
    }

    /// <summary>
    /// Выполняет атомарное обновление баланса счета в зависимости от типа транзакции.
    /// </summary>
    /// <param name="accountId">Идентификатор целевого счета.</param>
    /// <param name="balance">Сумма изменения.</param>
    /// <param name="transactionType">Тип операции (Доход/Расход) для определения направления изменения баланса.</param>
    /// <exception cref="KeyNotFoundException">Выбрасывается, если счет с указанным ID не найден.</exception>
    public async Task UpdateBalanceAsync(Guid accountId, decimal balance, TransactionType transactionType)
    {
        var account = await _dbContext.Accounts.FindAsync(accountId);
        if (account == null) throw new KeyNotFoundException("Account not found");
        if(transactionType == TransactionType.Expense)
        {
            account.Balance += balance;
        }
        else if (transactionType == TransactionType.Income)
        {
            account.Balance -= balance;
        }
        await _dbContext.SaveChangesAsync();
    }

}