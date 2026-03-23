using FinanceTracker.Domain.Enums;

namespace FinanceTracker.Application.DTOs;

public class CategoryDto
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Icon { get; set; }
    public TransactionType Type { get; set; }
}