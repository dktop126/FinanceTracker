namespace FinanceTracker.Domain.Interfaces;

/// <summary>
/// Интерфейс Unit of Work для управления транзакциями базы данных.
/// Обеспечивает атомарность операций и согласованность данных.
/// </summary>
public interface IUnitOfWork : IDisposable
{
    /// <summary>
    /// Репозиторий для работы со счетами.
    /// </summary>
    IAccountRepository Accounts { get; }
    
    /// <summary>
    /// Репозиторий для работы с транзакциями.
    /// </summary>
    ITransactionRepository Transactions { get; }
    
    /// <summary>
    /// Репозиторий для работы с категориями.
    /// </summary>
    ICategoryRepository Categories { get; }
    
    /// <summary>
    /// Сохраняет все изменения в базе данных в рамках одной транзакции.
    /// </summary>
    /// <returns>Количество затронутых записей.</returns>
    Task<int> SaveChangesAsync();
    
    /// <summary>
    /// Начинает транзакцию базы данных.
    /// </summary>
    Task BeginTransactionAsync();
    
    /// <summary>
    /// Фиксирует транзакцию базы данных.
    /// </summary>
    Task CommitTransactionAsync();
    
    /// <summary>
    /// Откатывает транзакцию базы данных.
    /// </summary>
    Task RollbackTransactionAsync();
}
