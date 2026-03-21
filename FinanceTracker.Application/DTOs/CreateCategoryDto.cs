using FinanceTracker.Domain.Enums;

namespace FinanceTracker.Application.DTOs;

public class CreateCategoryDto
{
    public string Name { get; set; }  = string.Empty;
    public string Icon { get; set; }  = string.Empty;
    public TransactionType Type { get; set; }
}