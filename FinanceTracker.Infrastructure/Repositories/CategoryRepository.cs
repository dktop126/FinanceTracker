using FinanceTracker.Domain.Interfaces;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Infrastructure.Repositories;

/// <summary>
/// Репозиторий для работы с категориями в базе данных.
/// Реализует доступ к данным с использованием Entity Framework Core.
/// </summary>
public class CategoryRepository : ICategoryRepository
{
    private readonly ApplicationDbContext _dbContext;

    /// <summary>
    /// Инициализирует новый экземпляр репозитория с контекстом базы данных.
    /// </summary>
    /// <param name="dbContext">Контекст базы данных приложения.</param>
    public CategoryRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Сохраняет новую категорию в базе данных.
    /// </summary>
    /// <param name="category">Сущность новой категории.</param>
    public async Task AddAsync(Category category)
    {
        await _dbContext.Categories.AddAsync(category);
        await _dbContext.SaveChangesAsync();
    }

    /// <summary>
    /// Находит счет по его уникальному идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор счета.</param>
    /// <returns>Экземпляр счета или null, если запись не найдена.</returns>
    public async Task<Category?> GetByIdAsync(Guid id)
        => await _dbContext.Categories.FindAsync(id);

    /// <summary>
    /// Получает список всех активных счетов из базы данных.
    /// </summary>
    public async Task<IEnumerable<Category>> GetAllAsync()
        => await _dbContext.Categories.ToListAsync();

    /// <summary>
    /// Обновляет существующую запись категории.
    /// </summary>
    /// <param name="category">Сущность категории с обновленными данными.</param>
    public async Task UpdateAsync(Category category)
    {
        _dbContext.Categories.Update(category);
        await _dbContext.SaveChangesAsync();
    }

    /// <summary>
    /// Удаляет запись о категории из базы данных.
    /// </summary>
    /// <param name="id">Идентификатор счета.</param>
    public async Task DeleteAsync(Guid id)
    {
        var category = await _dbContext.Categories.FindAsync(id);
        if (category != null)
        {
            _dbContext.Categories.Remove(category);
            await _dbContext.SaveChangesAsync();
        }
    }
}