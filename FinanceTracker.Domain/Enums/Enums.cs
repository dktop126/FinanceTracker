namespace FinanceTracker.Domain.Enums;

/// <summary>
/// Тип транзакции или категории (доход/расход).
/// </summary>
public enum TransactionType
{
    Income = 1,
    Expense = 2,
}

/// <summary>
/// Валюта.
/// </summary>
public enum Currency
{
    Rub,
    Usd,
    Eur,
}