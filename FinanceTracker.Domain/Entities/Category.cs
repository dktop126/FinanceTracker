using FinanceTracker.Domain.Common;

namespace FinanceTracker.Domain.Entities;

public class Category : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;

    public List<Transaction> Transactions { get; set; } = new();
}