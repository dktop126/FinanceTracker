using FinanceTracker.Application.DTOs;
using FinanceTracker.Domain.Entities;

namespace FinanceTracker.Application.Interfaces;

/// <summary>
/// Интерфейс сервиса управления финансовыми операциями.
/// Содержит бизнес-логику для работы с транзакциями, счетами и категориями.
/// </summary>
public interface IFinanceService
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

    /// <summary>Создает новую категорию транзакций.</summary>
    /// <param name="dto">Данные категории (имя, тип).</param>
    Task<Guid> CreateCategoryAsync(CreateCategoryDto dto);

    /// <summary>Возвращает список всех категорий.</summary>
    /// <remarks>Тут лучше тоже использовать CategoryDto, если есть.</remarks>
    Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync();

    /// <summary>Удаляет категорию. Убедитесь, что к ней не привязаны транзакции.</summary>
    /// <param name="categoryId">ID удаляемой категории.</param>
    Task DeleteCategoryAsync(Guid categoryId);

    /// <summary>Создает транзакцию и автоматически обновляет баланс связанного счета.</summary>
    /// <param name="dto">Данные транзакции.</param>
    /// <returns>Идентификатор созданной транзакции.</returns>
    Task<Guid> CreateTransactionAsync(CreateTransactionDto dto);

    /// <summary>Возвращает полную историю транзакций.</summary>
    Task<IEnumerable<TransactionDto>> GetAllTransactionsAsync();

    /// <summary>Удаляет транзакцию и выполняет корректировку баланса счета (возврат суммы).</summary>
    /// <param name="transactionId">ID транзакции для удаления.</param>
    Task DeleteTransactionAsync(Guid transactionId);
}