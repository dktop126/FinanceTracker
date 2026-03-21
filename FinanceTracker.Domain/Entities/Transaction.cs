using FinanceTracker.Domain.Common;
using FinanceTracker.Domain.Enums;

namespace FinanceTracker.Domain.Entities;

public class Transaction : BaseEntity
{
    public decimal Amount { get; set; }
    public string Description { get; set; } = string.Empty;
    public TransactionType Type { get; set; }
    public Currency Currency { get; set; }
    
    public Guid CategoryId { get; set; }
    public Category? Category { get; set; }
    
    public Guid AccountId { get; set; }
    public Account? Account { get; set; }
}