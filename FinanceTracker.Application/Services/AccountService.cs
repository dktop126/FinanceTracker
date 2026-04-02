using FinanceTracker.Application.DTOs;
using FinanceTracker.Application.Interfaces;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Interfaces;
using FluentValidation;

namespace FinanceTracker.Application.Services;

public class AccountService : IAccountService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<CreateAccountDto> _validator;

    public AccountService(IUnitOfWork unitOfWork, IValidator<CreateAccountDto> validator)
    {
        _unitOfWork = unitOfWork;
        _validator = validator;
    }

    public async Task<Guid> CreateAccountAsync(CreateAccountDto dto)
    {
        var validationResult = await _validator.ValidateAsync(dto);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var account = new Account
        {
            Name = dto.Name,
            Balance = dto.Balance,
            Currency = dto.Currency
        };

        await _unitOfWork.Accounts.AddAsync(account);
        await _unitOfWork.SaveChangesAsync();
        return account.Id;
    }

    public async Task<AccountDto?> GetAccountAsync(Guid accountId)
    {
        var account = await _unitOfWork.Accounts.GetByIdAsync(accountId);
        if (account == null) throw new KeyNotFoundException("Счет не найден");
        return new AccountDto
        {
            Id = account.Id,
            Name = account.Name,
            Balance = account.Balance,
            Currency = account.Currency
        };
    }

    public async Task<IEnumerable<AccountDto>> GetAllAccountsAsync()
    {
        var accounts = await _unitOfWork.Accounts.GetAllAsync();
        return accounts.Select(x => new AccountDto
            {
                Id = x.Id,
                Name = x.Name,
                Balance = x.Balance,
                Currency = x.Currency
            }
        );
    }

    public async Task<decimal> GetAccountBalanceAsync(Guid accountId)
    {
        var account = await _unitOfWork.Accounts.GetByIdAsync(accountId);
        if (account == null) throw new KeyNotFoundException("Счет не найден");
        return account.Balance;
    }
}