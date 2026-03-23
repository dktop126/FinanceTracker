using FinanceTracker.Domain.Common;
using FinanceTracker.Domain.Enums;

namespace FinanceTracker.Domain.Entities;

/// <summary>
/// Сущность, представляющая финансовую операцию (доход или расход).
/// Связывает денежное изменение с конкретным счетом и категорией учета.
/// </summary>
public class Transaction : BaseEntity
{
    /// <summary>
    /// Сумма транзакции. Всегда положительное число. 
    /// Направление движения средств определяется свойством <see cref="Type"/>.
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// Комментарий или описание транзакции (например, "Купил хлеб" или "Зарплата за март").
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Тип транзакции: Доход (Income) или Расход (Expense).
    /// </summary>
    public TransactionType Type { get; set; }

    /// <summary>
    /// Валюта, в которой была совершена данная операция.
    /// </summary>
    public Currency Currency { get; set; }
    
    /// <summary>
    /// Идентификатор категории, к которой относится данная транзакция.
    /// </summary>
    public Guid CategoryId { get; set; }

    /// <summary>
    /// Навигационное свойство: Категория транзакции (например, "Продукты" или "Работа").
    /// </summary>
    public Category? Category { get; set; }
    
    /// <summary>
    /// Идентификатор счета, по которому была проведена операция.
    /// </summary>
    public Guid AccountId { get; set; }

    /// <summary>
    /// Навигационное свойство: Счет, к которому привязана транзакция.
    /// </summary>
    public Account? Account { get; set; }
}