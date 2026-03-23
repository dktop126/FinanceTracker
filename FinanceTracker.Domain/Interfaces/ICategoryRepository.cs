using FinanceTracker.Domain.Entities;

namespace FinanceTracker.Domain.Interfaces;

/// <summary>
/// Интерфейс репозитория для управления категориями в базе данных.
/// Отвечает за сохранение, чтение и удаление категорий.
/// </summary>
public interface ICategoryRepository
{
    /// <summary>
    /// Добавляет новую категорию в базу данных.
    /// </summary>
    /// <param name="category">Объект категории для добавления.</param>
    /// <returns></returns>
    Task AddAsync(Category category);

    /// <summary>
    /// Находит категорию по её уникальному идентификатору.
    /// </summary>
    /// <param name="id">Уникальный идентификатор категории (Guid).</param>
    /// <returns>Объект категории или null, если запись не найдена.</returns>
    Task<Category?> GetByIdAsync(Guid id);

    /// <summary>
    /// Получает список всех категорий.
    /// </summary>
    /// <returns>Список всех категорий.</returns>
    Task<IEnumerable<Category>> GetAllAsync();

    /// <summary>
    /// Удаляет категорию из базы данных по её уникальному идентификатору.
    /// </summary>
    /// <param name="id">Уникальный идентификатор категории (Guid).</param>
    Task DeleteAsync(Guid id);
}