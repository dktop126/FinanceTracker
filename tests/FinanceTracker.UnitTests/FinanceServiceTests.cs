using FinanceTracker.Application.DTOs;
using FinanceTracker.Application.Services;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Enums;
using FinanceTracker.Domain.Interfaces;
using Xunit.v3;
using Moq;
using FluentValidation;

namespace FinanceTracker.UnitTests;

public class FinanceServiceTests
{
    [Fact]
    public async Task DeleteTransaction_ShouldIncreaseBalance_WhenTransactionIsExpense()
    {
        var accountRepoMock = new Mock<IAccountRepository>();
        var transRepoMock = new Mock<ITransactionRepository>();
        var categoryRepoMock = new Mock<ICategoryRepository>();
        var validatorMock = new Mock<IValidator<CreateTransactionDto>>();

        var account = new Account { Id = Guid.NewGuid(), Balance = 1000 };
        var transaction = new Transaction
        {
            Id = Guid.NewGuid(),
            Amount = 200,
            Type = TransactionType.Expense,
            AccountId = account.Id
        };
        
        transRepoMock.Setup(r => r.GetByIdAsync(transaction.Id)).ReturnsAsync(transaction);
        accountRepoMock.Setup(r => r.GetByIdAsync(account.Id)).ReturnsAsync(account);
        
        var service = new TransactionService(transRepoMock.Object, accountRepoMock.Object, categoryRepoMock.Object,  validatorMock.Object);
        
        await service.DeleteTransactionAsync(transaction.Id);

        Assert.Equal(1200, account.Balance);
    }
}
