using FinanceTracker.Application.DTOs;
using FinanceTracker.Application.Validators;
using FluentValidation.TestHelper;
using Xunit.v3;

namespace FinanceTracker.UnitTests;

public class ValidationTests
{
    #region CreateTransactionDto Validation Tests

    [Fact]
    public void CreateTransactionValidator_ValidData_ShouldPass()
    {
        // Arrange
        var validator = new CreateTransactionValidator();
        var dto = new CreateTransactionDto
        {
            Amount = 100,
            Description = "Test transaction",
            AccountId = Guid.NewGuid(),
            CategoryId = Guid.NewGuid(),
            TransactionType = 1
        };

        // Act
        var result = validator.TestValidate(dto);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void CreateTransactionValidator_NegativeAmount_ShouldFail()
    {
        // Arrange
        var validator = new CreateTransactionValidator();
        var dto = new CreateTransactionDto
        {
            Amount = -100,
            Description = "Test",
            AccountId = Guid.NewGuid(),
            CategoryId = Guid.NewGuid(),
            TransactionType = 1
        };

        // Act
        var result = validator.TestValidate(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Amount)
            .WithErrorMessage("Сумма транзакции должна быть больше нуля.");
    }

    [Fact]
    public void CreateTransactionValidator_ZeroAmount_ShouldFail()
    {
        // Arrange
        var validator = new CreateTransactionValidator();
        var dto = new CreateTransactionDto
        {
            Amount = 0,
            Description = "Test",
            AccountId = Guid.NewGuid(),
            CategoryId = Guid.NewGuid(),
            TransactionType = 1
        };

        // Act
        var result = validator.TestValidate(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Amount)
            .WithErrorMessage("Сумма транзакции должна быть больше нуля.");
    }

    [Fact]
    public void CreateTransactionValidator_DescriptionTooLong_ShouldFail()
    {
        // Arrange
        var validator = new CreateTransactionValidator();
        var dto = new CreateTransactionDto
        {
            Amount = 100,
            Description = new string('a', 201), // 201 символ
            AccountId = Guid.NewGuid(),
            CategoryId = Guid.NewGuid(),
            TransactionType = 1
        };

        // Act
        var result = validator.TestValidate(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Description)
            .WithErrorMessage("Описание не может превышать 200 символов.");
    }

    [Fact]
    public void CreateTransactionValidator_EmptyCategoryId_ShouldFail()
    {
        // Arrange
        var validator = new CreateTransactionValidator();
        var dto = new CreateTransactionDto
        {
            Amount = 100,
            Description = "Test",
            AccountId = Guid.NewGuid(),
            CategoryId = Guid.Empty,
            TransactionType = 1
        };

        // Act
        var result = validator.TestValidate(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.CategoryId)
            .WithErrorMessage("Необходимо выбрать категорию.");
    }

    [Fact]
    public void CreateTransactionValidator_EmptyAccountId_ShouldFail()
    {
        // Arrange
        var validator = new CreateTransactionValidator();
        var dto = new CreateTransactionDto
        {
            Amount = 100,
            Description = "Test",
            AccountId = Guid.Empty,
            CategoryId = Guid.NewGuid(),
            TransactionType = 1
        };

        // Act
        var result = validator.TestValidate(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.AccountId)
            .WithErrorMessage("Необходимо выбрать счет.");
    }

    [Fact]
    public void CreateTransactionValidator_MaxLengthDescription_ShouldPass()
    {
        // Arrange
        var validator = new CreateTransactionValidator();
        var dto = new CreateTransactionDto
        {
            Amount = 100,
            Description = new string('a', 200), // Ровно 200 символов
            AccountId = Guid.NewGuid(),
            CategoryId = Guid.NewGuid(),
            TransactionType = 1
        };

        // Act
        var result = validator.TestValidate(dto);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Description);
    }

    [Fact]
    public void CreateTransactionValidator_EmptyDescription_ShouldPass()
    {
        // Arrange
        var validator = new CreateTransactionValidator();
        var dto = new CreateTransactionDto
        {
            Amount = 100,
            Description = string.Empty,
            AccountId = Guid.NewGuid(),
            CategoryId = Guid.NewGuid(),
            TransactionType = 1
        };

        // Act
        var result = validator.TestValidate(dto);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Description);
    }

    #endregion

    #region CreateAccountDto Validation Tests

    [Fact]
    public void CreateAccountValidator_ValidData_ShouldPass()
    {
        // Arrange
        var validator = new CreateAccountValidator();
        var dto = new CreateAccountDto
        {
            Name = "My Account",
            Balance = 1000,
            Currency = Domain.Enums.Currency.Rub
        };

        // Act
        var result = validator.TestValidate(dto);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void CreateAccountValidator_EmptyName_ShouldFail()
    {
        // Arrange
        var validator = new CreateAccountValidator();
        var dto = new CreateAccountDto
        {
            Name = string.Empty,
            Balance = 1000,
            Currency = Domain.Enums.Currency.Rub
        };

        // Act
        var result = validator.TestValidate(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void CreateAccountValidator_NullName_ShouldFail()
    {
        // Arrange
        var validator = new CreateAccountValidator();
        var dto = new CreateAccountDto
        {
            Name = null!,
            Balance = 1000,
            Currency = Domain.Enums.Currency.Rub
        };

        // Act
        var result = validator.TestValidate(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void CreateAccountValidator_NameTooLong_ShouldFail()
    {
        // Arrange
        var validator = new CreateAccountValidator();
        var dto = new CreateAccountDto
        {
            Name = new string('a', 101), // 101 символ
            Balance = 1000,
            Currency = Domain.Enums.Currency.Rub
        };

        // Act
        var result = validator.TestValidate(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void CreateAccountValidator_NegativeBalance_ShouldPass()
    {
        // Arrange
        var validator = new CreateAccountValidator();
        var dto = new CreateAccountDto
        {
            Name = "Debt Account",
            Balance = -500, // Отрицательный баланс допустим (долг)
            Currency = Domain.Enums.Currency.Rub
        };

        // Act
        var result = validator.TestValidate(dto);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void CreateAccountValidator_ZeroBalance_ShouldPass()
    {
        // Arrange
        var validator = new CreateAccountValidator();
        var dto = new CreateAccountDto
        {
            Name = "New Account",
            Balance = 0,
            Currency = Domain.Enums.Currency.Usd
        };

        // Act
        var result = validator.TestValidate(dto);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void CreateAccountValidator_MaxLengthName_ShouldPass()
    {
        // Arrange
        var validator = new CreateAccountValidator();
        var dto = new CreateAccountDto
        {
            Name = new string('a', 100), // Ровно 100 символов
            Balance = 1000,
            Currency = Domain.Enums.Currency.Eur
        };

        // Act
        var result = validator.TestValidate(dto);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Name);
    }

    #endregion
}
