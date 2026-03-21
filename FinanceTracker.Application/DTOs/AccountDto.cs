using FinanceTracker.Domain.Enums;

namespace FinanceTracker.Application.DTOs;

public class AccountDto
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public decimal Balance { get; set; }
    public Currency Currency { get; set; }
}