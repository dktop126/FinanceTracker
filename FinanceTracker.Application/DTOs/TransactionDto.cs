using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Enums;

namespace FinanceTracker.Application.DTOs;

public class TransactionDto
{
    public Guid TransactionId { get; set; }
    public decimal Amount { get; set; }
    public TransactionType TransactionType { get; set; }
    public Currency Currency { get; set; }
    public Category Category { get; set; }
    public Account Account { get; set; }
    public string Date { get; set; }
}