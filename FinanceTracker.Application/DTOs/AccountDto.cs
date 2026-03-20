using FinanceTracker.Domain.Enums;

namespace FinanceTracker.Application.DTOs;

public class AccountDto
{
    public Guid AccountId { get; set; }
    public string? AccountName { get; set; }
    public decimal Balance { get; set; }
    public Currency Currency { get; set; }
}