using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Enums;

namespace FinanceTracker.Domain.Interfaces;

/// <summary>
/// Интерфейс репозитория для управления счетами в базе данных.
/// Отвечает за сохранение, чтение и удаление счетов.
/// </summary>
public interface IAccountRepository
{
    /// <summary>
    /// Добавляет новый счет в базу данных.
    /// </summary>
    /// <param name="account">Объект аккаунта для добавления.</param>
    /// <returns></returns>
    Task AddAsync(Account account);

    /// <summary>
    /// Находит счет по его уникальному идентификатору.
    /// </summary>
    /// <param name="id">Уникальный идентификатор аккаунта (Guid).</param>
    /// <returns>Объект аккаунта или null, если запись не найдена.</returns>
    Task<Account?> GetByIdAsync(Guid id);

    /// <summary>
    /// Получает список всех счетов.
    /// </summary>
    /// <returns>Список всех аккаунтов.</returns>
    Task<IEnumerable<Account>> GetAllAsync();

    /// <summary>
    /// Обновляет счет.
    /// </summary>
    /// <param name="account"></param>
    /// <returns></returns>
    Task UpdateAsync(Account account);
    
    /// <summary>
    /// Обновляет баланс счета по его уникальному идентификатору.
    /// </summary>
    /// <param name="accountId"></param>
    /// <param name="balance"></param>
    /// <param name="transactionType"></param>
    /// <returns></returns>
    Task UpdateBalanceAsync(Guid accountId, decimal balance, TransactionType transactionType);
    
}