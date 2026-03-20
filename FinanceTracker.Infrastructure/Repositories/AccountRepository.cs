using FinanceTracker.Domain.Interfaces;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Infrastructure.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly ApplicationDbContext _dbContext;
    public AccountRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<Account?> GetByIdAsync(Guid id) 
        => await _dbContext.Accounts.FindAsync(id);

    public async Task<IEnumerable<Account>> GetAllAsync()
        => await _dbContext.Accounts.ToListAsync();
    
    public async Task UpdateAsync(Account account) 
    { 
        _dbContext.Accounts.Update(account); 
        await _dbContext.SaveChangesAsync(); 
    }
    
    public async Task AddAsync(Account account)
    {
        await _dbContext.Accounts.AddAsync(account);
        await _dbContext.SaveChangesAsync();
    }

}