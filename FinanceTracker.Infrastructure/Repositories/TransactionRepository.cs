using FinanceTracker.Domain.Interfaces;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Infrastructure.Repositories;

/// <summary>
/// Репозиторий для работы с транзакциями в базе данных.
/// Реализует доступ к данным с использованием Entity Framework Core.
/// </summary>
public class TransactionRepository : ITransactionRepository
{
    private readonly ApplicationDbContext _dbContext;

    /// <summary>
    /// Инициализирует новый экземпляр репозитория с контекстом базы данных.
    /// </summary>
    /// <param name="dbContext">Контекст базы данных приложения.</param>
    public TransactionRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Сохраняет новую транзакцию в базе данных.
    /// </summary>
    /// <param name="transaction">Сущность новой транзакции.</param>
    public async Task AddAsync(Transaction transaction)
    {
        await _dbContext.Transactions.AddAsync(transaction);
        await _dbContext.SaveChangesAsync();
    }

    /// <summary>
    /// Находит транзакцию по её уникальному идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор транзакции.</param>
    /// <returns>Экземпляр транзакции или null, если запись не найдена.</returns>
    public async Task<Transaction?> GetByIdAsync(Guid id)
    {
        return await _dbContext.Transactions
            .Include(t => t.Category)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    /// <summary>
    /// Получает список всех активных транзакций из базы данных.
    /// </summary>
    /// <returns>Список активных транзакции.</returns>
    public async Task<IEnumerable<Transaction>> GetAllAsync() => await _dbContext.Transactions
        .Include(t => t.Category)
        .Include(t => t.Account)
        .ToListAsync();

    /// <summary>
    /// Обновляет существующую запись транзакции.
    /// </summary>
    /// <param name="transaction">Сущность транзакции с обновленными данными.</param>
    public async Task UpdateAsync(Transaction transaction)
    {
        _dbContext.Transactions.Update(transaction);
        await _dbContext.SaveChangesAsync();
    }

    /// <summary>
    /// Удаляет запись о транзакции из базы данных.
    /// </summary>
    /// <param name="id">Идентификатор транзакции.</param>
    public async Task DeleteAsync(Guid id)
    {
        var t = await _dbContext.Transactions.FindAsync(id);
        if (t != null)
        {
            _dbContext.Transactions.Remove(t);
            await _dbContext.SaveChangesAsync();
        }
    }
}