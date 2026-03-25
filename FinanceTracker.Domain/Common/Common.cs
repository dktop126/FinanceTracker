namespace FinanceTracker.Domain.Common;

/// <summary>
/// Базовый абстрактный класс для всех доменных сущностей системы.
/// Содержит общие поля, необходимые для идентификации и аудита записей.
/// </summary>
public abstract class BaseEntity
{
    /// <summary>
    /// Уникальный идентификатор сущности (Первичный ключ).
    /// По умолчанию генерируется новый <see cref="Guid"/>.
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Дата и время создания записи в формате UTC.
    /// Используется для сортировки и истории операций.
    /// </summary>
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    
    /// <summary>
    /// Флаг "мягкого удаления", если true, то сущность считается удаленной и не используется в программе.
    /// </summary>
    public bool IsDeleted { get; set; }
    
    /// <summary>
    /// Дата и время удаления сущности.
    /// </summary>
    public DateTime? DeletedOn { get; set; }
}