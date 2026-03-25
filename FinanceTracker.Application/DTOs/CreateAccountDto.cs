using FinanceTracker.Domain.Enums;

namespace FinanceTracker.Application.DTOs;

public class CreateAccountDto
{
    public string Name { get; set; } = string.Empty;
    public decimal Balance { get; set; }
    public Currency Currency { get; set; }
}