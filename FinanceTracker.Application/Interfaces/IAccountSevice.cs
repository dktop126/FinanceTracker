using FinanceTracker.Application.DTOs;

namespace FinanceTracker.Application.Interfaces;

/// <summary>
/// Интерфейс сервиса управления счетами.
/// Содержит бизнес-логику для работы со счетами.
/// </summary>
public interface IAccountService
{
    /// <summary>Создает новый счет.</summary>
    /// <param name="dto">Данные для создания счета.</param>
    /// <returns>Идентификатор (Guid) созданного счета.</returns>
    Task<Guid> CreateAccountAsync(CreateAccountDto dto);

    /// <summary>Получает информацию о счете по его идентификатору.</summary>
    /// <param name="accountId">ID искомого счета.</param>
    Task<AccountDto?> GetAccountAsync(Guid accountId);

    /// <summary>Возвращает список всех доступных счетов пользователя.</summary>
    Task<IEnumerable<AccountDto>> GetAllAccountsAsync();

    /// <summary>Получает текущий остаток средств на счете.</summary>
    /// <param name="accountId">ID счета для проверки баланса.</param>
    Task<decimal> GetAccountBalanceAsync(Guid accountId);

}