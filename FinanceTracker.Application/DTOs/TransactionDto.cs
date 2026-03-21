using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Enums;

namespace FinanceTracker.Application.DTOs;

public class TransactionDto
{
    public Guid AccountId { get; set; }
    public string? AccountName { get; set; }
    public decimal Amount { get; set; }
    public Currency Currency { get; set; }
    public Guid CategoryId { get; set; }
    public string? CategoryName { get; set; }
    public DateTime Date { get; set; }
    public Guid TransactionId { get; set; }
    public TransactionType TransactionType { get; set; }
}