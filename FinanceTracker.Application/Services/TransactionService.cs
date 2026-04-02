using FinanceTracker.Application.DTOs;
using FinanceTracker.Application.Interfaces;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Enums;
using FinanceTracker.Domain.Interfaces;
using FluentValidation;

namespace FinanceTracker.Application.Services;

public class TransactionService : ITransactionService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<CreateTransactionDto> _validator;

    public TransactionService(IUnitOfWork unitOfWork, IValidator<CreateTransactionDto> validator)
    {
        _unitOfWork = unitOfWork;
        _validator = validator;
    }

    public async Task<Guid> CreateTransactionAsync(CreateTransactionDto dto)
    {
        var validationResult = await _validator.ValidateAsync(dto);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        await _unitOfWork.BeginTransactionAsync();
        
        try
        {
            var account = await _unitOfWork.Accounts.GetByIdAsync(dto.AccountId);
            if (account == null) throw new KeyNotFoundException("Счет не найден");

            var category = await _unitOfWork.Categories.GetByIdAsync(dto.CategoryId);
            if (category == null) throw new KeyNotFoundException("Категория не найдена");

            var transaction = new Transaction
            {
                Amount = dto.Amount,
                Description = dto.Description,
                AccountId = dto.AccountId,
                CategoryId = dto.CategoryId,
                Type = (TransactionType)dto.TransactionType,
                Account = account,
                Category = category
            };

            if (transaction.Type == TransactionType.Income)
                account.Balance += transaction.Amount;
            else if (transaction.Type == TransactionType.Expense)
                account.Balance -= transaction.Amount;

            await _unitOfWork.Transactions.AddAsync(transaction);
            await _unitOfWork.Accounts.UpdateAsync(account);
            
            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitTransactionAsync();

            return transaction.Id;
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }

    public async Task<IEnumerable<TransactionDto>> GetAllTransactionsAsync()
    {
        var transactions = await _unitOfWork.Transactions.GetAllAsync();
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
        await _unitOfWork.BeginTransactionAsync();
        
        try
        {
            var transaction = await _unitOfWork.Transactions.GetByIdAsync(transactionId);
            if (transaction == null) throw new KeyNotFoundException("Транзакция не найдена");
            
            var account = await _unitOfWork.Accounts.GetByIdAsync(transaction.AccountId);
            if (account == null) throw new KeyNotFoundException("Счет не найден");
            
            if (transaction.Type == TransactionType.Income)
                account.Balance -= transaction.Amount;
            else if (transaction.Type == TransactionType.Expense)
                account.Balance += transaction.Amount;
            
            transaction.IsDeleted = true;
            transaction.DeletedOn = DateTime.UtcNow;
            
            await _unitOfWork.Accounts.UpdateAsync(account);
            await _unitOfWork.Transactions.UpdateAsync(transaction);
            
            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitTransactionAsync();
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }
}