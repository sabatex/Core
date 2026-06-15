using Sabatex.Core;

namespace sabatex.BakeryWebApp.Models;

/// <summary>
/// Це базовий абстрактний клас, який надає первинний ключ та підтримку м'якого видалення.
/// Спільний предок для всіх бізнес-об'єктів, сумісних з 1С.
/// </summary>
public abstract class Base1CObject : EntityBase<Guid>, IDeletionMark
{
 
    /// <summary>
    /// Псевдонім для <see cref="Id"/>, використовується для сумісності з системами 1С,
    /// де первинне посилання називається "Ref".
    /// </summary>
    public Guid Ref => Id;

    /// <summary>
    /// Маркер м'якого видалення.
    /// Якщо <c>true</c>, сутність вважається логічно видаленою, але все ще зберігається в базі даних.
    /// </summary>
    public bool DeletionMark { get; set; }
}
