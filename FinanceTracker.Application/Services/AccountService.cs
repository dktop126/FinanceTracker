using FinanceTracker.Application.DTOs;
using FinanceTracker.Application.Interfaces;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Interfaces;

namespace FinanceTracker.Application.Services;

public class AccountService : IAccountService
{
    private readonly IAccountRepository _accountRepository;

    public AccountService(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }
        
        public async Task<Guid> CreateAccountAsync(CreateAccountDto dto)
    {
        var account = new Account
        {
            Name = dto.Name,
            Balance = dto.Balance,
            Currency = dto.Currency
        };

        await _accountRepository.AddAsync(account);
        return account.Id;
    }

    public async Task<AccountDto?> GetAccountAsync(Guid accountId)
    {
        var account = await _accountRepository.GetByIdAsync(accountId);
        if (account == null) throw new KeyNotFoundException("Account not found");
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
        var accounts = await _accountRepository.GetAllAsync();
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
        var account = await _accountRepository.GetByIdAsync(accountId);
        if (account == null) throw new KeyNotFoundException("Account not found");
        return account.Balance;
    }

}