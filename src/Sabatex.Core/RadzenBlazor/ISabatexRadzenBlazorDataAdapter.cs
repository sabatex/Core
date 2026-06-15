using Sabatex.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sabatex.Core.RadzenBlazor;
using System.Reflection;

namespace Sabatex.Core.RadzenBlazor;
/// <summary>
/// Defines an abstraction for performing asynchronous CRUD and query operations on entities using OData-compatible
/// parameters in a Radzen Blazor application.
/// </summary>
/// <remarks>This interface provides methods for retrieving, creating, updating, and deleting entities, as well as
/// querying collections with support for filtering, sorting, paging, and expansion of related data. Implementations are
/// expected to handle OData query expressions and return results in a format suitable for data-bound UI components. All
/// operations are asynchronous and return tasks that complete when the underlying data operation finishes.</remarks>
public interface ISabatexRadzenBlazorDataAdapter
{
    /// <summary>
    /// Asynchronously retrieves a collection of items that match the specified query parameters.
    /// </summary>
    /// <typeparam name="TItem">The type of the items to retrieve. Must implement the IEntityBase&lt;TKey&gt; interface.</typeparam>
    /// <typeparam name="TKey">The type of the key for the items.</typeparam>
    /// <param name="queryParams">The parameters that define filtering, sorting, and paging options for the query. Cannot be null.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a QueryResult&lt;TItem&gt; with the items
    /// that match the query and related metadata.</returns>
    Task<QueryResult<TItem>> GetAsync<TItem, TKey>(QueryParams queryParams) where TItem : class, IEntityBase<TKey>;
    /// <summary>
    /// Asynchronously retrieves an entity by its unique identifier.
    /// </summary>
    /// <typeparam name="TItem">The type of the entity to retrieve. Must implement IEntityBase&lt;TKey&gt;.</typeparam>
    /// <typeparam name="TKey">The type of the unique identifier for the entity.</typeparam>
    /// <param name="id">The unique identifier of the entity to retrieve.</param>
    /// <param name="expand">An optional comma-separated list of related entities to include in the result. If null, only the main entity is
    /// retrieved.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the entity if found; otherwise,
    /// null.</returns>
    Task<TItem?> GetByIdAsync<TItem, TKey>(TKey id, string? expand=null) where TItem: class, IEntityBase<TKey>;
    /// <summary>
    /// Asynchronously retrieves an entity by its unique identifier.
    /// </summary>
    /// <typeparam name="TItem">The type of the entity to retrieve. Must implement IEntityBase&lt;TKey&gt;.</typeparam>
    /// <typeparam name="TKey">The type of the unique identifier for the entity.</typeparam>
    /// <param name="id">The unique identifier of the entity to retrieve. Cannot be null or empty.</param>
    /// <param name="expand">An optional comma-separated list of related entities to include in the result. If null, only the main entity is
    /// returned.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the entity if found; otherwise,
    /// null.</returns>
    Task<TItem?> GetByIdAsync<TItem, TKey>(string id, string? expand = null) where TItem : class, IEntityBase<TKey>;
    /// <summary>
    /// Asynchronously posts the specified item and returns the result of the validation operation.
    /// </summary>
    /// <typeparam name="TItem">The type of the item to post. Must implement the IEntityBase&lt;TKey&gt; interface.</typeparam>
    /// <typeparam name="TKey">The type of the key for the item.</typeparam>
    /// <param name="item">The item to be posted. Can be null if the operation supports null values.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a SabatexValidationModel&lt;TItem&gt; with
    /// the validation outcome and the posted item.</returns>
    Task<SabatexValidationModel<TItem>> PostAsync<TItem, TKey>(TItem? item) where TItem : class, IEntityBase<TKey>;
    /// <summary>
    /// Asynchronously updates the specified item in the data store and returns the result of the validation operation.
    /// </summary>
    /// <typeparam name="TItem">The type of the item to update. Must be a reference type that implements IEntityBase&lt;TKey&gt;.</typeparam>
    /// <typeparam name="TKey">The type of the key for the item.</typeparam>
    /// <param name="item">The item to update. Cannot be null. The item's identifier is used to locate the existing record in the data
    /// store.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a SabatexValidationModel&lt;TItem&gt; with
    /// the updated item and validation results.</returns>
    Task<SabatexValidationModel<TItem>> UpdateAsync<TItem, TKey>(TItem item) where TItem : class, IEntityBase<TKey>;
    /// <summary>
    /// Asynchronously deletes the entity of the specified type with the given identifier.
    /// </summary>
    /// <typeparam name="TItem">The type of the entity to delete. Must implement IEntityBase&lt;TKey&gt;.</typeparam>
    /// <typeparam name="TKey">The type of the identifier for the entity.</typeparam>
    /// <param name="id">The identifier of the entity to delete.</param>
    /// <returns>A task that represents the asynchronous delete operation.</returns>
    Task DeleteAsync<TItem, TKey>(TKey id) where TItem : class, IEntityBase<TKey>;

    /// <summary>
    /// Default convenience overloads that allow calling methods without specifying TKey.
    /// These implementations use a small amount of reflection at type-initialization time and then cache the constructed
    /// MethodInfo instances to avoid repeated generic construction lookups on each call.
    /// </summary>
    /// <typeparam name="TItem">The type of the entity to delete. Must implement IEntityBase&lt;TKey&gt;</typeparam>
    /// <param name="item">The type of the identifier for the entity.</param>
    /// <returns>A task that represents the asynchronous delete operation.</returns>
    /// <exception cref="InvalidOperationException"></exception>
    async Task<SabatexValidationModel<TItem>> UpdateAsync<TItem>(TItem item)
        where TItem : class
    {
        var mi = DispatchCache<TItem>.UpdateAsyncMethod;
        var task = (Task<SabatexValidationModel<TItem>>)mi.Invoke(this, new object[] { item })!;
        return await task;
    }
    /// <summary>
    /// Sends the specified item to the server using an HTTP POST request and returns the validation result.
    /// </summary>
    /// <typeparam name="TItem">The type of the item to be posted. Must be a reference type.</typeparam>
    /// <param name="item">The item to post to the server. Can be null if the server endpoint accepts null values.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a SabatexValidationModel{TItem} with
    /// the validation outcome of the posted item.</returns>
    async Task<SabatexValidationModel<TItem>> PostAsync<TItem>(TItem? item)
        where TItem : class
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(item), "Item cannot be null for PostAsync operation.");      
        }

        var mi = DispatchCache<TItem>.PostAsyncMethod;
        var task = (Task<SabatexValidationModel<TItem>>)mi.Invoke(this, new object[] { item })!;
        return await task;
    }
    /// <summary>
    /// Asynchronously executes a query using the specified parameters and returns the result as a strongly typed
    /// collection.
    /// </summary>
    /// <typeparam name="TItem">The type of the items to be returned in the query result. Must be a reference type.</typeparam>
    /// <param name="queryParams">The parameters that define the query to execute. Cannot be null.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="QueryResult{TItem}"/>
    /// with the items matching the query parameters.</returns>
    async Task<QueryResult<TItem>> GetAsync<TItem>(QueryParams queryParams)
        where TItem : class
    {
        var mi = DispatchCache<TItem>.GetAsyncMethod;
        var task = (Task<QueryResult<TItem>>)mi.Invoke(this, new object[] { queryParams })!;
        return await task;
    }
    /// <summary>
    /// Asynchronously retrieves an item of the specified type by its unique identifier.
    /// </summary>
    /// <typeparam name="TItem">The type of the item to retrieve. Must be a reference type.</typeparam>
    /// <param name="id">The unique identifier of the item to retrieve. Cannot be null.</param>
    /// <param name="expand">An optional comma-separated list of related entities to include in the result. If null, no related entities are
    /// expanded.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the item if found; otherwise, null.</returns>
    async Task<TItem?> GetByIdAsync<TItem>(string id, string? expand = null)
        where TItem : class
    {
        var mi = DispatchCache<TItem>.GetByIdMethod;
#pragma warning disable CS8601 // Possible null reference assignment.
        var task = (Task<TItem?>)mi.Invoke(this, new object[] { id, expand })!;
#pragma warning restore CS8601 // Possible null reference assignment.
        return await task;
    }
    /// <summary>
    /// Asynchronously deletes an entity of the specified type with the given identifier.
    /// </summary>
    /// <remarks>If the entity with the specified identifier does not exist, the operation completes without
    /// error. This method is typically used to remove entities from a data store by their primary key.</remarks>
    /// <typeparam name="TItem">The type of the entity to delete. Must be a reference type.</typeparam>
    /// <param name="id">The identifier of the entity to delete. Must be convertible to the entity's key type and cannot be null.</param>
    /// <returns>A task that represents the asynchronous delete operation.</returns>
    async Task DeleteAsync<TItem>(object id)
        where TItem : class
    {
        var mi = DispatchCache<TItem>.DeleteAsyncMethod;
        object? keyValue = DispatchCache<TItem>.ConvertId(id);

        var task = (Task)mi.Invoke(this, new object[] { keyValue! })!;
        await task;
    }

}

/// <summary>
/// Helper that caches the constructed MethodInfo instances for a given TItem type to avoid repeated generic construction.
/// Initialization runs once per TItem and performs the minimal reflection required to discover the entity key type.
/// </summary>
internal static class DispatchCache<TItem> where TItem : class
{
    public static readonly Type KeyType;
    public static readonly MethodInfo UpdateAsyncMethod;
    public static readonly MethodInfo PostAsyncMethod;
    public static readonly MethodInfo GetAsyncMethod;
    public static readonly MethodInfo GetByIdMethod;
    public static readonly MethodInfo DeleteAsyncMethod;

    static DispatchCache()
    {
        var tItem = typeof(TItem);
        var iface = tItem.GetInterfaces().FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEntityBase<>));
        if (iface == null)
            throw new InvalidOperationException($"Type {tItem.FullName} does not implement IEntityBase<...> required for generic dispatch.");

        KeyType = iface.GetGenericArguments()[0];

        var methods = typeof(ISabatexRadzenBlazorDataAdapter).GetMethods(BindingFlags.Public | BindingFlags.Instance);

        var update = methods.First(m => m.Name == nameof(ISabatexRadzenBlazorDataAdapter.UpdateAsync) && m.IsGenericMethodDefinition && m.GetGenericArguments().Length == 2 && m.GetParameters().Length == 1);
        UpdateAsyncMethod = update.MakeGenericMethod(tItem, KeyType);

        var post = methods.First(m => m.Name == nameof(ISabatexRadzenBlazorDataAdapter.PostAsync) && m.IsGenericMethodDefinition && m.GetGenericArguments().Length == 2 && m.GetParameters().Length == 1);
        PostAsyncMethod = post.MakeGenericMethod(tItem, KeyType);

        var get = methods.First(m => m.Name == nameof(ISabatexRadzenBlazorDataAdapter.GetAsync) && m.IsGenericMethodDefinition && m.GetGenericArguments().Length == 2 && m.GetParameters().Length == 1 && m.GetParameters()[0].ParameterType == typeof(QueryParams));
        GetAsyncMethod = get.MakeGenericMethod(tItem, KeyType);

        var getByString = methods.First(m => m.Name == nameof(ISabatexRadzenBlazorDataAdapter.GetByIdAsync) && m.IsGenericMethodDefinition && m.GetGenericArguments().Length == 2 && m.GetParameters().Length == 2 && m.GetParameters()[0].ParameterType == typeof(string));
        GetByIdMethod = getByString.MakeGenericMethod(tItem, KeyType);

        var del = methods.First(m => m.Name == nameof(ISabatexRadzenBlazorDataAdapter.DeleteAsync) && m.IsGenericMethodDefinition && m.GetGenericArguments().Length == 2 && m.GetParameters().Length == 1);
        DeleteAsyncMethod = del.MakeGenericMethod(tItem, KeyType);
    }

    /// <summary>
    /// Convert the provided id to the cached KeyType using fast TryParse/typed conversions for common key types.
    /// Falls back to Convert.ChangeType when no specific parser is available.
    /// </summary>
    public static object? ConvertId(object? id)
    {
        if (id == null) return null;

        var target = KeyType;
        if (target == typeof(object)) return id;

        // If already the right type, return as-is
        if (id.GetType() == target) return id;

        // Fast paths for common key types
        if (target == typeof(string)) return id.ToString();

        if (target == typeof(Guid))
        {
            if (id is Guid g) return g;
            var s = id.ToString();
            if (Guid.TryParse(s, out var parsedG)) return parsedG;
        }

        if (target == typeof(int))
        {
            if (id is int i) return i;
            var s = id.ToString();
            if (int.TryParse(s, out var parsedInt)) return parsedInt;
        }

        if (target == typeof(long))
        {
            if (id is long l) return l;
            var s = id.ToString();
            if (long.TryParse(s, out var parsedLong)) return parsedLong;
        }

        // Try nullable int/long wrappers
        var nullableTarget = Nullable.GetUnderlyingType(target);
        if (nullableTarget != null)
        {
            if (nullableTarget == typeof(Guid))
            {
                var s = id.ToString();
                if (Guid.TryParse(s, out var parsedG)) return (object?)parsedG;
            }
            if (nullableTarget == typeof(int))
            {
                var s = id.ToString();
                if (int.TryParse(s, out var parsedInt)) return (object?)parsedInt;
            }
            if (nullableTarget == typeof(long))
            {
                var s = id.ToString();
                if (long.TryParse(s, out var parsedLong)) return (object?)parsedLong;
            }
        }

        // Fallback to ChangeType which handles IConvertible implementations
        try
        {
            return Convert.ChangeType(id, target);
        }
        catch
        {
            // as a last resort, try parsing from string via TypeConverter pattern or throw helpful exception
            var s = id.ToString();
            if (s != null)
            {
                if (target.IsEnum && Enum.TryParse(target, s, out var enumVal)) return enumVal;
            }

            // rethrow original concept as invalid cast
            throw new InvalidCastException($"Cannot convert id value of type {id.GetType()} to key type {target}.");
        }
    }
}



/// <summary>
/// Represents a descriptor for a field, including its name, type, operation, priority, and optional value. 
/// </summary>
/// <param name="Name">The name of the field being described. Cannot be null.</param>
/// <param name="FieldType">The data type of the field. Cannot be null.</param>
/// <param name="operation">The logical operation associated with the field. Defaults to " or ".</param>
/// <param name="priority">The priority of the field in processing or evaluation. Higher values indicate higher priority. Defaults to 0.</param>
/// <param name="value">An optional value associated with the field. Can be null.</param>
public record struct  FieldDescriptor(string Name,Type FieldType,string operation=" or ",int priority = 0,string? value=null);
/// <summary>
/// Represents the result of a validation operation, including the validated item and any associated validation errors.
/// </summary>
/// <typeparam name="TItem">The type of the item being validated.</typeparam>
/// <param name="Result">The validated item, or null if validation failed or no result is available.</param>
/// <param name="Errors">A dictionary containing validation errors, where each key is a property name and the value is a list of error
/// messages for that property. Can be null if there are no errors.</param>
public record SabatexValidationModel<TItem>(TItem? Result, Dictionary<string, List<string>>? Errors=null);
/// <summary>
/// Represents errors that occur during object deserialization.
/// </summary>
/// <remarks>Use this exception to indicate that an object could not be deserialized due to invalid data, format
/// issues, or other deserialization failures. This exception is typically thrown by custom deserialization routines
/// when the input cannot be converted to the expected object type.</remarks>
public class DeserializeException : Exception
{
    /// <summary>
    /// Initializes a new instance of the DeserializeException class with a default error message indicating a
    /// deserialization failure.
    /// </summary>
    public DeserializeException() : base("Deserialize object error.") { }
}