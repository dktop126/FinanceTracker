using FinanceTracker.Domain.Entities;

namespace FinanceTracker.Domain.Interfaces;

/// <summary>
/// Интерфейс репозитория для управления финансовыми транзакциями в базе данных.
/// Отвечает за сохранение, чтение и удаление записей о доходах и расходах.
/// </summary>
public interface ITransactionRepository
{
    /// <summary>
    /// Добавляет новую транзакцию в базу данных.
    /// </summary>
    /// <param name="transaction">Объект транзакции для добавления.</param>
    Task AddAsync(Transaction transaction);

    /// <summary>
    /// Находит транзакцию по её уникальному идентификатору.
    /// </summary>
    /// <param name="id">Уникальный идентификатор транзакции (Guid).</param>
    /// <returns>Объект транзакции или null, если запись не найдена.</returns>
    Task<Transaction?> GetByIdAsync(Guid id);

    /// <summary>
    /// Получает список всех транзакций.
    /// Может включать связанные сущности (категории и счета).
    /// </summary>
    /// <returns>Список всех транзакций.</returns>
    Task<IEnumerable<Transaction>> GetAllAsync();

    /// <summary>
    /// Обновляет транзакцию.
    /// </summary>
    /// <param name="transaction"></param>
    Task UpdateAsync(Transaction transaction);

    /// <summary>
    /// Удаляет транзакцию из базы данных по её идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор транзакции для удаления.</param>
    Task DeleteAsync(Guid id);
}