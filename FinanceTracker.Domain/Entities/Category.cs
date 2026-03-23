using FinanceTracker.Domain.Common;
using FinanceTracker.Domain.Enums;

namespace FinanceTracker.Domain.Entities;

/// <summary>
/// Сущность, представляющая категорию финансовых операций (например, "Продукты", "Зарплата", "Транспорт").
/// Используется для группировки и классификации транзакций.
/// </summary>
public class Category : BaseEntity
{
    /// <summary>
    /// Название категории.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Строковый идентификатор или код иконки для визуального отображения в интерфейсе (например, "mdi-cart").
    /// </summary>
    public string Icon { get; set; } = string.Empty;

    /// <summary>
    /// Тип операций, для которых доступна данная категория.
    /// Позволяет фильтровать категории в зависимости от того, является ли транзакция доходом или расходом.
    /// </summary>
    public TransactionType Type { get; set; }

    /// <summary>
    /// Навигационное свойство: список всех транзакций, отнесенных к данной категории.
    /// </summary>
    public List<Transaction> Transactions { get; set; } = new();
}