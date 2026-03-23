using FinanceTracker.Application.DTOs;

namespace FinanceTracker.Application.Interfaces;

/// <summary>
/// Интерфейс сервиса управления категориями.
/// Содержит бизнес-логику для работы с категориями.
/// </summary>
public interface ICategoryService
{
    /// <summary>Создает новую категорию транзакций.</summary>
    /// <param name="dto">Данные категории (имя, тип).</param>
    Task<Guid> CreateCategoryAsync(CreateCategoryDto dto);

    /// <summary>Возвращает список всех категорий.</summary>
    /// <remarks>Тут лучше тоже использовать CategoryDto, если есть.</remarks>
    Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync();

    /// <summary>Удаляет категорию. Убедитесь, что к ней не привязаны транзакции.</summary>
    /// <param name="categoryId">ID удаляемой категории.</param>
    Task DeleteCategoryAsync(Guid categoryId);

}