using FinanceTracker.Application.DTOs;
using FinanceTracker.Application.Interfaces;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Interfaces;

namespace FinanceTracker.Application.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IAccountRepository _accountRepository;

    public CategoryService(ICategoryRepository categoryRepository, IAccountRepository accountRepository)
    {
        _categoryRepository = categoryRepository;
        _accountRepository = accountRepository;
    }
    
    public async Task<Guid> CreateCategoryAsync(CreateCategoryDto dto)
    {
        var category = new Category
        {
            Name = dto.Name,
            Icon = dto.Icon,
            Type = dto.Type
        };
        await _categoryRepository.AddAsync(category);

        return category.Id;
    }

    public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync()
    {
        var categories = await _categoryRepository.GetAllAsync();
        var categoriesDtos = categories.Select(x => new CategoryDto
            {
                Id = x.Id,
                Name = x.Name,
                Icon = x.Icon,
                Type = x.Type
            }
        );
        return categoriesDtos;
    }

    public async Task DeleteCategoryAsync(Guid id)
    {
        var category = await _categoryRepository.GetByIdAsync(id);
        if (category == null) throw new KeyNotFoundException("Category not found");
        category.IsDeleted = true;
        category.DeletedOn = DateTime.UtcNow;
        
        await _categoryRepository.UpdateAsync(category);
    }

}