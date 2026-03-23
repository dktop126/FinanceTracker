using FinanceTracker.Domain.Common;
using FinanceTracker.Domain.Enums;

namespace FinanceTracker.Domain.Entities;

/// <summary>
/// Сущность, представляющая финансовый счет пользователя (например, банковская карта, наличные или вклад).
/// Является владельцем транзакций и хранит текущий денежный остаток.
/// </summary>
public class Account : BaseEntity
{
    /// <summary>
    /// Отображаемое название счета (например, "Основная карта" или "Заначка").
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Текущий баланс счета.
    /// </summary>
    public decimal Balance { get; set; }

    /// <summary>
    /// Валюта, в которой ведется учет по данному счету.
    /// </summary>
    public Currency Currency { get; set; }

    /// <summary>
    /// Навигационное свойство: список всех транзакций, совершенных с использованием этого счета.
    /// </summary>
    public List<Transaction> Transactions { get; set; } = new();
}