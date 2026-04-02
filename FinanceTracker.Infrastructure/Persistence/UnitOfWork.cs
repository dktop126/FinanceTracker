using FinanceTracker.Domain.Interfaces;
using FinanceTracker.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace FinanceTracker.Infrastructure.Persistence;

/// <summary>
/// Реализация паттерна Unit of Work для управления транзакциями базы данных.
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private IDbContextTransaction? _transaction;
    
    private IAccountRepository? _accounts;
    private ITransactionRepository? _transactions;
    private ICategoryRepository? _categories;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }

    public IAccountRepository Accounts => 
        _accounts ??= new AccountRepository(_context);

    public ITransactionRepository Transactions => 
        _transactions ??= new TransactionRepository(_context);

    public ICategoryRepository Categories => 
        _categories ??= new CategoryRepository(_context);

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public async Task BeginTransactionAsync()
    {
        _transaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.CommitAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async Task RollbackTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
    }
}
