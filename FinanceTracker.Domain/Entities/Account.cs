using FinanceTracker.Domain.Common;
using FinanceTracker.Domain.Enums;

namespace FinanceTracker.Domain.Entities;

public class Account : BaseEntity
{
    public string? Name { get; set; } = string.Empty;
    public decimal Balance { get; set; }
    public Currency Currency { get; set; }
    public List<Transaction> Transactions { get; set; } = new();
}