using FinanceTracker.Domain.Entities;

namespace FinanceTracker.Domain.Interfaces;

/// <summary>
/// Интерфейс репозитория для управления аккаунтами в базе данных.
/// Отвечает за сохранение, чтение и удаление аккаунтов.
/// </summary>
public interface IAccountRepository
{
    /// <summary>
    /// Добавляет новый аккаунт в базу данных.
    /// </summary>
    /// <param name="account">Объект аккаунта для добавления.</param>
    /// <returns></returns>
    Task AddAsync(Account account);

    /// <summary>
    /// Находит аккаунт по его уникальному идентификатору.
    /// </summary>
    /// <param name="id">Уникальный идентификатор аккаунта (Guid).</param>
    /// <returns>Объект аккаунта или null, если запись не найдена.</returns>
    Task<Account?> GetByIdAsync(Guid id);

    /// <summary>
    /// Получает список всех аккаунтов.
    /// </summary>
    /// <returns>Список всех аккаунтов.</returns>
    Task<IEnumerable<Account>> GetAllAsync();

    /// <summary>
    /// Обновляет аккаунт.
    /// </summary>
    /// <param name="account"></param>
    /// <returns></returns>
    Task UpdateAsync(Account account);
}