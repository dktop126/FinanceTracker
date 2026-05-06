using FinanceTracker.Application.DTOs;
using FluentValidation;

namespace FinanceTracker.Application.Validators;

public class CreateAccountValidator : AbstractValidator<CreateAccountDto>
{
    public CreateAccountValidator()
    {
        RuleFor(x => x.Name)
            .NotNull().WithMessage("Укажите название счета.")
            .NotEmpty().WithMessage("Название счета не может быть пустым.")
            .MaximumLength(100).WithMessage("Название счета не может превышать 100 символов.");
    }
}