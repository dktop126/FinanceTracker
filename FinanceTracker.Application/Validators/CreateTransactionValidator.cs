using FinanceTracker.Application.DTOs;
using FluentValidation;

namespace FinanceTracker.Application.Validators;

public class CreateTransactionValidator : AbstractValidator<CreateTransactionDto>
{
    public CreateTransactionValidator()
    {
        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("Сумма транзакции должна быть больше нуля.");
        
        RuleFor(x => x.Description)
            .MaximumLength(200).WithMessage("Описание не может превышать 200 символов.");
        
        RuleFor(x => x.CategoryId)
            .NotEmpty().WithMessage("Необходимо выбрать категорию.");
        
        RuleFor(x => x.AccountId)
            .NotEmpty().WithMessage("Необходимо выбрать счет.");
    }
}