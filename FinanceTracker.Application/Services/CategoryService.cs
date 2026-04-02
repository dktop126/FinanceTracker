using FinanceTracker.Application.DTOs;
using FinanceTracker.Application.Interfaces;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Interfaces;

namespace FinanceTracker.Application.Services;

public class CategoryService : ICategoryService
{
    private readonly IUnitOfWork _unitOfWork;

    public CategoryService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Guid> CreateCategoryAsync(CreateCategoryDto dto)
    {
        var category = new Category
        {
            Name = dto.Name,
            Icon = dto.Icon,
            Type = dto.Type
        };
        await _unitOfWork.Categories.AddAsync(category);
        await _unitOfWork.SaveChangesAsync();

        return category.Id;
    }

    public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync()
    {
        var categories = await _unitOfWork.Categories.GetAllAsync();
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
        var category = await _unitOfWork.Categories.GetByIdAsync(id);
        if (category == null) throw new KeyNotFoundException("Категория не найдена");
        category.IsDeleted = true;
        category.DeletedOn = DateTime.UtcNow;
        
        await _unitOfWork.Categories.UpdateAsync(category);
        await _unitOfWork.SaveChangesAsync();
    }

}