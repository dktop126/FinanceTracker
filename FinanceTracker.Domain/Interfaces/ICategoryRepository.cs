using FinanceTracker.Domain.Entities;

namespace FinanceTracker.Domain.Interfaces;

public interface ICategoryRepository
{
    Task<Category?> GetByIdAsync (Guid id);
    Task<IEnumerable<Category>> GetAllAsync ();
    Task AddAsync (Category category);
    Task DeleteAsync (Guid id);
}