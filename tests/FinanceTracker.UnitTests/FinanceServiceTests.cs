using FinanceTracker.Application.Services;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Enums;
using FinanceTracker.Domain.Interfaces;
using Xunit.v3;
using Moq;

namespace FinanceTracker.UnitTests;

public class FinanceServiceTests
{
    [Fact]
    public async Task DeleteTransaction_ShouldIncreaseBalance_WhenTransactionIsExpense()
    {
        var accountRepoMock = new Mock<IAccountRepository>();
        var transRepoMock = new Mock<ITransactionRepository>();
        var categoryRepoMock = new Mock<ICategoryRepository>();

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
        
        var service = new FinanceService(transRepoMock.Object, categoryRepoMock.Object, accountRepoMock.Object);
        
        await service.DeleteTransactionAsync(transaction.Id);

        Assert.Equal(1200, account.Balance);
    }
}
