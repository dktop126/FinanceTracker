using FinanceTracker.Application.DTOs;

namespace FinanceTracker.Application.Interfaces;

/// <summary>
/// Интерфейс сервиса управления транзакциями.
/// Содержит бизнес-логику для работы с транзакциями.
/// </summary>
public interface ITransactionService
{
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