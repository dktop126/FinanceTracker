namespace FinanceTracker.Application.DTOs;

public class CreateTransactionDto
{
    public decimal Amount { get; set; }
    public string Description { get; set; } = string.Empty;
    public Guid AccountId { get; set; }
    public Guid CategoryId { get; set; }
    public int TransactionType { get; set; }
}