using FinanceTracker.Application.DTOs;
using FluentValidation;

namespace FinanceTracker.Application.Validators;

public class CreateAccountValidator : AbstractValidator<CreateAccountDto>
{
    public CreateAccountValidator()
    {
        RuleFor(x => x.Name).NotNull().WithMessage("Укажите название счета.");
    }
}