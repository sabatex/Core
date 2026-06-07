using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;


namespace Sabatex.Core;

/// <summary>
/// Базовий абстрактний клас для сутностей з ключем. 
/// </summary>
/// <typeparam name="TKey"></typeparam>
public abstract class EntityBase<TKey>:IEntityBase<TKey>
{
    /// <summary>
    /// первинний ключ (int,long,Guid,string)
    /// </summary>
    public TKey Id { get; set; } = default!;
}
