namespace Sabatex.Core;

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
/// <summary>
/// Represents an entity that uses a string as its unique key identifier.
/// </summary>
/// <remarks>This interface is intended for use as a marker to enable compile-time constraints on entities with
/// string-based keys. It can be used to enforce type safety in generic code that operates on entities with specific key
/// types.</remarks>
public interface IEntityWithStringKey : IEntityBase<string> { }
/// <summary>
/// Represents an entity that uses an integer as its unique identifier.
/// </summary>
/// <remarks>This interface is typically implemented by entity types that require a strongly-typed integer primary
/// key. It extends the generic IEntityBase interface with an integer key type for convenience and consistency across
/// the data model.</remarks>
public interface IEntityWithIntKey : IEntityBase<int> { }
/// <summary>
/// Represents an entity that uses a 64-bit integer as its unique identifier.
/// </summary>
/// <remarks>This interface is typically implemented by domain entities that require a long (Int64) primary key.
/// It extends the generic IEntityBase interface with a long key type, providing a consistent contract for entities with
/// long-based identifiers.</remarks>
public interface IEntityWithLongKey : IEntityBase<long> { }
/// <summary>
/// Represents an entity that is identified by a globally unique identifier (GUID) key.
/// </summary>
/// <remarks>Implement this interface to indicate that an entity uses a GUID as its primary key. This is commonly
/// used for entities that require unique identification across distributed systems or databases.</remarks>
public interface IEntityWithGuidKey : IEntityBase<Guid> { }