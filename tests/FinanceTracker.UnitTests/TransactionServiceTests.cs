using FinanceTracker.Application.DTOs;
using FinanceTracker.Application.Services;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Enums;
using FinanceTracker.Domain.Interfaces;
using Xunit.v3;
using Moq;
using FluentValidation;
using FluentValidation.Results;

namespace FinanceTracker.UnitTests;

public class TransactionServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<ITransactionRepository> _transactionRepoMock;
    private readonly Mock<IAccountRepository> _accountRepoMock;
    private readonly Mock<ICategoryRepository> _categoryRepoMock;
    private readonly Mock<IValidator<CreateTransactionDto>> _validatorMock;
    private readonly TransactionService _service;

    public TransactionServiceTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _transactionRepoMock = new Mock<ITransactionRepository>();
        _accountRepoMock = new Mock<IAccountRepository>();
        _categoryRepoMock = new Mock<ICategoryRepository>();
        _validatorMock = new Mock<IValidator<CreateTransactionDto>>();

        _unitOfWorkMock.Setup(u => u.Transactions).Returns(_transactionRepoMock.Object);
        _unitOfWorkMock.Setup(u => u.Accounts).Returns(_accountRepoMock.Object);
        _unitOfWorkMock.Setup(u => u.Categories).Returns(_categoryRepoMock.Object);

        _service = new TransactionService(_unitOfWorkMock.Object, _validatorMock.Object);
    }

    #region CreateTransaction Tests

    [Fact]
    public async Task CreateTransaction_Income_ShouldIncreaseBalance()
    {
        // Arrange
        var accountId = Guid.NewGuid();
        var categoryId = Guid.NewGuid();
        var account = new Account { Id = accountId, Balance = 1000, Name = "Test Account" };
        var category = new Category { Id = categoryId, Name = "Salary", Type = TransactionType.Income };

        var dto = new CreateTransactionDto
        {
            Amount = 500,
            Description = "Monthly salary",
            AccountId = accountId,
            CategoryId = categoryId,
            TransactionType = (int)TransactionType.Income
        };

        _validatorMock.Setup(v => v.ValidateAsync(dto, default))
            .ReturnsAsync(new ValidationResult());
        _accountRepoMock.Setup(r => r.GetByIdAsync(accountId)).ReturnsAsync(account);
        _categoryRepoMock.Setup(r => r.GetByIdAsync(categoryId)).ReturnsAsync(category);

        // Act
        var result = await _service.CreateTransactionAsync(dto);

        // Assert
        Assert.Equal(1500, account.Balance);
        _unitOfWorkMock.Verify(u => u.BeginTransactionAsync(), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        _unitOfWorkMock.Verify(u => u.CommitTransactionAsync(), Times.Once);
        _transactionRepoMock.Verify(r => r.AddAsync(It.IsAny<Transaction>()), Times.Once);
    }

    [Fact]
    public async Task CreateTransaction_Expense_ShouldDecreaseBalance()
    {
        // Arrange
        var accountId = Guid.NewGuid();
        var categoryId = Guid.NewGuid();
        var account = new Account { Id = accountId, Balance = 1000, Name = "Test Account" };
        var category = new Category { Id = categoryId, Name = "Food", Type = TransactionType.Expense };

        var dto = new CreateTransactionDto
        {
            Amount = 300,
            Description = "Groceries",
            AccountId = accountId,
            CategoryId = categoryId,
            TransactionType = (int)TransactionType.Expense
        };

        _validatorMock.Setup(v => v.ValidateAsync(dto, default))
            .ReturnsAsync(new ValidationResult());
        _accountRepoMock.Setup(r => r.GetByIdAsync(accountId)).ReturnsAsync(account);
        _categoryRepoMock.Setup(r => r.GetByIdAsync(categoryId)).ReturnsAsync(category);

        // Act
        var result = await _service.CreateTransactionAsync(dto);

        // Assert
        Assert.Equal(700, account.Balance);
        _unitOfWorkMock.Verify(u => u.BeginTransactionAsync(), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        _unitOfWorkMock.Verify(u => u.CommitTransactionAsync(), Times.Once);
    }

    [Fact]
    public async Task CreateTransaction_InvalidData_ShouldThrowValidationException()
    {
        // Arrange
        var dto = new CreateTransactionDto
        {
            Amount = -100, // Отрицательная сумма
            AccountId = Guid.NewGuid(),
            CategoryId = Guid.NewGuid(),
            TransactionType = (int)TransactionType.Income
        };

        var validationFailures = new List<ValidationFailure>
        {
            new ValidationFailure("Amount", "Сумма транзакции должна быть больше нуля.")
        };
        _validatorMock.Setup(v => v.ValidateAsync(dto, default))
            .ReturnsAsync(new ValidationResult(validationFailures));

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => _service.CreateTransactionAsync(dto));
        _unitOfWorkMock.Verify(u => u.BeginTransactionAsync(), Times.Never);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task CreateTransaction_AccountNotFound_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        var dto = new CreateTransactionDto
        {
            Amount = 100,
            AccountId = Guid.NewGuid(),
            CategoryId = Guid.NewGuid(),
            TransactionType = (int)TransactionType.Income
        };

        _validatorMock.Setup(v => v.ValidateAsync(dto, default))
            .ReturnsAsync(new ValidationResult());
        _accountRepoMock.Setup(r => r.GetByIdAsync(dto.AccountId)).ReturnsAsync((Account?)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.CreateTransactionAsync(dto));
        Assert.Equal("Счет не найден", exception.Message);
        _unitOfWorkMock.Verify(u => u.RollbackTransactionAsync(), Times.Once);
    }

    [Fact]
    public async Task CreateTransaction_CategoryNotFound_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        var accountId = Guid.NewGuid();
        var account = new Account { Id = accountId, Balance = 1000 };

        var dto = new CreateTransactionDto
        {
            Amount = 100,
            AccountId = accountId,
            CategoryId = Guid.NewGuid(),
            TransactionType = (int)TransactionType.Income
        };

        _validatorMock.Setup(v => v.ValidateAsync(dto, default))
            .ReturnsAsync(new ValidationResult());
        _accountRepoMock.Setup(r => r.GetByIdAsync(accountId)).ReturnsAsync(account);
        _categoryRepoMock.Setup(r => r.GetByIdAsync(dto.CategoryId)).ReturnsAsync((Category?)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.CreateTransactionAsync(dto));
        Assert.Equal("Категория не найдена", exception.Message);
        _unitOfWorkMock.Verify(u => u.RollbackTransactionAsync(), Times.Once);
    }

    [Fact]
    public async Task CreateTransaction_DatabaseError_ShouldRollback()
    {
        // Arrange
        var accountId = Guid.NewGuid();
        var categoryId = Guid.NewGuid();
        var account = new Account { Id = accountId, Balance = 1000 };
        var category = new Category { Id = categoryId, Name = "Test" };

        var dto = new CreateTransactionDto
        {
            Amount = 100,
            AccountId = accountId,
            CategoryId = categoryId,
            TransactionType = (int)TransactionType.Income
        };

        _validatorMock.Setup(v => v.ValidateAsync(dto, default))
            .ReturnsAsync(new ValidationResult());
        _accountRepoMock.Setup(r => r.GetByIdAsync(accountId)).ReturnsAsync(account);
        _categoryRepoMock.Setup(r => r.GetByIdAsync(categoryId)).ReturnsAsync(category);
        _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).ThrowsAsync(new Exception("Database error"));

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _service.CreateTransactionAsync(dto));
        _unitOfWorkMock.Verify(u => u.RollbackTransactionAsync(), Times.Once);
        _unitOfWorkMock.Verify(u => u.CommitTransactionAsync(), Times.Never);
    }

    #endregion

    #region DeleteTransaction Tests

    [Fact]
    public async Task DeleteTransaction_Expense_ShouldIncreaseBalance()
    {
        // Arrange
        var accountId = Guid.NewGuid();
        var transactionId = Guid.NewGuid();
        var account = new Account { Id = accountId, Balance = 1000 };
        var transaction = new Transaction
        {
            Id = transactionId,
            Amount = 200,
            Type = TransactionType.Expense,
            AccountId = accountId
        };

        _transactionRepoMock.Setup(r => r.GetByIdAsync(transactionId)).ReturnsAsync(transaction);
        _accountRepoMock.Setup(r => r.GetByIdAsync(accountId)).ReturnsAsync(account);

        // Act
        await _service.DeleteTransactionAsync(transactionId);

        // Assert
        Assert.Equal(1200, account.Balance);
        Assert.True(transaction.IsDeleted);
        Assert.NotNull(transaction.DeletedOn);
        _unitOfWorkMock.Verify(u => u.BeginTransactionAsync(), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        _unitOfWorkMock.Verify(u => u.CommitTransactionAsync(), Times.Once);
    }

    [Fact]
    public async Task DeleteTransaction_Income_ShouldDecreaseBalance()
    {
        // Arrange
        var accountId = Guid.NewGuid();
        var transactionId = Guid.NewGuid();
        var account = new Account { Id = accountId, Balance = 1000 };
        var transaction = new Transaction
        {
            Id = transactionId,
            Amount = 500,
            Type = TransactionType.Income,
            AccountId = accountId
        };

        _transactionRepoMock.Setup(r => r.GetByIdAsync(transactionId)).ReturnsAsync(transaction);
        _accountRepoMock.Setup(r => r.GetByIdAsync(accountId)).ReturnsAsync(account);

        // Act
        await _service.DeleteTransactionAsync(transactionId);

        // Assert
        Assert.Equal(500, account.Balance);
        Assert.True(transaction.IsDeleted);
    }

    [Fact]
    public async Task DeleteTransaction_NotFound_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        var transactionId = Guid.NewGuid();
        _transactionRepoMock.Setup(r => r.GetByIdAsync(transactionId)).ReturnsAsync((Transaction?)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.DeleteTransactionAsync(transactionId));
        Assert.Equal("Транзакция не найдена", exception.Message);
        _unitOfWorkMock.Verify(u => u.RollbackTransactionAsync(), Times.Once);
    }

    #endregion

    #region GetAllTransactions Tests

    [Fact]
    public async Task GetAllTransactions_ShouldReturnOrderedByDateDescending()
    {
        // Arrange
        var transactions = new List<Transaction>
        {
            new Transaction
            {
                Id = Guid.NewGuid(),
                Amount = 100,
                Type = TransactionType.Income,
                CreatedOn = DateTime.UtcNow.AddDays(-2),
                Account = new Account { Name = "Account1" },
                Category = new Category { Name = "Category1" }
            },
            new Transaction
            {
                Id = Guid.NewGuid(),
                Amount = 200,
                Type = TransactionType.Expense,
                CreatedOn = DateTime.UtcNow.AddDays(-1),
                Account = new Account { Name = "Account2" },
                Category = new Category { Name = "Category2" }
            },
            new Transaction
            {
                Id = Guid.NewGuid(),
                Amount = 300,
                Type = TransactionType.Income,
                CreatedOn = DateTime.UtcNow,
                Account = new Account { Name = "Account3" },
                Category = new Category { Name = "Category3" }
            }
        };

        _transactionRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(transactions);

        // Act
        var result = await _service.GetAllTransactionsAsync();
        var resultList = result.ToList();

        // Assert
        Assert.Equal(3, resultList.Count);
        Assert.Equal(300, resultList[0].Amount); // Самая новая транзакция первая
        Assert.Equal(200, resultList[1].Amount);
        Assert.Equal(100, resultList[2].Amount);
    }

    [Fact]
    public async Task GetAllTransactions_EmptyList_ShouldReturnEmpty()
    {
        // Arrange
        _transactionRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Transaction>());

        // Act
        var result = await _service.GetAllTransactionsAsync();

        // Assert
        Assert.Empty(result);
    }

    #endregion
}
