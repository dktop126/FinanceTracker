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
}