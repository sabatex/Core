namespace Sabatex.Core;

/// <summary>
/// Non-generic base for entities. Provides a common ToKeyString method used across marker interfaces.
/// </summary>
//public interface IEntityBase
//{
//    /// <summary>
//    /// Returns a string representation of the object's key identifier.
//    /// </summary>
//    string ToKeyString();
//}

/// <summary>
/// Base entity methods for (api, ef)
/// </summary>
public interface IEntityBase<TKey>// : IEntityBase
{
    /// <summary>
    /// Get primary key as string (int,long,Guid,string)
    /// </summary>
    TKey Id { get; set; }

    /// <summary>
    /// Returns a string representation of the object's key identifier.
    /// </summary>
    /// <returns>A string that represents the key identifier. Returns an empty string if the identifier is null.</returns>
    string ToKeyString()
    {
        return Id?.ToString() ?? "";
    }
}

// Marker interfaces for common key types. New code can constrain to these for compile-time safety.
public interface IEntityWithStringKey : IEntityBase<string> { }
public interface IEntityWithIntKey : IEntityBase<int> { }
public interface IEntityWithLongKey : IEntityBase<long> { }
public interface IEntityWithGuidKey : IEntityBase<Guid> { }