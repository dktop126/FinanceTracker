using FinanceTracker.Domain.Interfaces;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Infrastructure.Repositories;

public class TransactionRepository : ITransactionRepository
{
    private readonly ApplicationDbContext _dbContext;
    
    public TransactionRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(Transaction transaction)
    {
        await _dbContext.Transactions.AddAsync(transaction);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<Transaction?> GetByIdAsync(Guid id)
    {
        return await _dbContext.Transactions
            .Include(t => t.Category)
            .FirstOrDefaultAsync(t => t.Id == id);
    }
    public async Task<IEnumerable<Transaction>> GetAllAsync() => await _dbContext.Transactions.ToListAsync();
    
    public async Task UpdateAsync(Transaction transaction) 
    { _dbContext.Transactions.Update(transaction); await _dbContext.SaveChangesAsync(); }

    public async Task DeleteAsync(Guid id)
    {
        var t = await _dbContext.Transactions.FindAsync(id);
        if (t != null)
        { _dbContext.Transactions.Remove(t); await _dbContext.SaveChangesAsync(); }
    }
}