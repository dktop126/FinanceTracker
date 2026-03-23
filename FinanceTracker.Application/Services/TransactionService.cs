using FinanceTracker.Application.DTOs;
using FinanceTracker.Application.Interfaces;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Enums;
using FinanceTracker.Domain.Interfaces;
using FluentValidation;

namespace FinanceTracker.Application.Services;

public class TransactionService : ITransactionService
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IValidator<CreateTransactionDto> _validator;

    public TransactionService(ITransactionRepository transactionRepository, IAccountRepository accountRepository,
        ICategoryRepository categoryRepository,
        IValidator<CreateTransactionDto> validator)
    {
        _transactionRepository = transactionRepository;
        _accountRepository = accountRepository;
        _categoryRepository = categoryRepository;
        _validator = validator;
    }

    public async Task<Guid> CreateTransactionAsync(CreateTransactionDto dto)
    {
        var validationResult = await _validator.ValidateAsync(dto);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors.First().ErrorMessage);

        var account = await _accountRepository.GetByIdAsync(dto.AccountId);
        if (account == null) throw new KeyNotFoundException("Account not found");

        var transaction = new Transaction
        {
            Amount = dto.Amount,
            Description = dto.Description,
            AccountId = dto.AccountId,
            CategoryId = dto.CategoryId,
            Type = (TransactionType)dto.TransactionType,
            Account = account,
            Category = await _categoryRepository.GetByIdAsync(dto.CategoryId)
        };

        if (transaction.Type == TransactionType.Income)
            account.Balance += transaction.Amount;
        else if (transaction.Type == TransactionType.Expense)
            account.Balance -= transaction.Amount;

        await _transactionRepository.AddAsync(transaction);
        await _accountRepository.UpdateAsync(account);

        return transaction.Id;
    }

    public async Task<IEnumerable<TransactionDto>> GetAllTransactionsAsync()
    {
        var transactions = await _transactionRepository.GetAllAsync();
        var transactionDtos = transactions.Select(x => new TransactionDto
        {
            AccountId = x.AccountId,
            AccountName = x.Account?.Name,
            Amount = x.Amount,
            CategoryId = x.CategoryId,
            CategoryName = x.Category?.Name,
            Currency = x.Currency,
            Date = x.CreatedOn,
            TransactionId = x.Id,
            TransactionType = x.Type
        }).OrderByDescending(x => x.Date);
        return transactionDtos;
    }

    public async Task DeleteTransactionAsync(Guid transactionId)
    {
        var transaction = await _transactionRepository.GetByIdAsync(transactionId);
        if (transaction == null) throw new KeyNotFoundException("Transaction not found");
        var account = await _accountRepository.GetByIdAsync(transaction.AccountId);
        if (account == null) throw new KeyNotFoundException("Account not found");
        if (transaction.Type == TransactionType.Income)
            account.Balance -= transaction.Amount;
        else if (transaction.Type == TransactionType.Expense)
            account.Balance += transaction.Amount;

        await _accountRepository.UpdateAsync(account);
        await _transactionRepository.DeleteAsync(transactionId);
    }
}