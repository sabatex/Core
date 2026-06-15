using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;


namespace Sabatex.Core.ClassExtensions;

/// <summary>
/// Provides extension methods for working with enumeration (enum) types, including retrieving descriptions and display
/// names associated with enum values.
/// </summary>
/// <remarks>These extension methods are intended to simplify common operations with enums, such as obtaining
/// user-friendly descriptions defined by attributes like <see cref="System.ComponentModel.DescriptionAttribute"/>. The
/// methods are available only on .NET 6.0 or later.</remarks>
public static class EnumExtensions
{
    /// <summary>
    /// Retrieves the description associated with the specified enumeration value, as defined by a <see
    /// cref="DescriptionAttribute"/>.
    /// </summary>
    /// <remarks>This method is typically used to obtain user-friendly descriptions for enumeration values,
    /// such as those displayed in user interfaces. If the enumeration member does not have a <see
    /// cref="DescriptionAttribute"/>, the method returns an empty string.</remarks>
    /// <param name="value">The enumeration value for which to retrieve the description. Cannot be null.</param>
    /// <returns>A string containing the description specified by the <see cref="DescriptionAttribute"/> applied to the
    /// enumeration value; or an empty string if no description is defined.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is null.</exception>
    public static string Description(this Enum value)
    {
        if (value == null)
            throw new ArgumentNullException(nameof(value));
        var enumMember = value.GetType().GetMember(value.ToString()).FirstOrDefault();
        var descriptionAttribute =
            enumMember == null
                ? default(DescriptionAttribute)
                : enumMember.GetCustomAttribute(typeof(DescriptionAttribute)) as DescriptionAttribute;
        return
            descriptionAttribute == null
                ? string.Empty
                : descriptionAttribute.Description;
    }
    /// <summary>
    /// Retrieves the description associated with the specified enumeration value.
    /// </summary>
    /// <remarks>This method is typically used to obtain user-friendly descriptions for enumeration values
    /// that have been annotated with the <see cref="DescriptionAttribute"/>. If the attribute is not present, the
    /// method returns the default string representation of the enumeration value.</remarks>
    /// <param name="value">The enumeration value for which to obtain the description.</param>
    /// <returns>A string containing the description specified by the <see cref="DescriptionAttribute"/> applied to the
    /// enumeration value; if no description is found, the name of the enumeration value is returned.</returns>
    public static string GetDescription(this Enum value)
    {
        var enumMember = value.GetType().GetMember(value.ToString()).FirstOrDefault();
        var descriptionAttribute =
            enumMember == null
                ? default(DescriptionAttribute)
                : enumMember.GetCustomAttribute(typeof(DescriptionAttribute)) as DescriptionAttribute;
        return
            descriptionAttribute == null
                ? value.ToString()
                : descriptionAttribute.Description;
    }
    /// <summary>
    /// Returns a collection of names and descriptions for all values defined in the specified enumeration type.
    /// </summary>
    /// <remarks>The method enumerates all values of the type of <paramref name="_enum"/> and returns their
    /// string names along with their descriptions as provided by the <c>GetDescription</c> extension method. If a value
    /// does not have a description, an empty string is returned for the description.</remarks>
    /// <param name="_enum">The enumeration instance whose type is used to retrieve value names and descriptions. Must not be null.</param>
    /// <returns>An enumerable collection of tuples, each containing the name and description of an enumeration value. The
    /// collection includes all values defined in the enumeration type.</returns>
    public static IEnumerable<(string name, string description)> GetEnumDisplayName(this Enum _enum)
    {
        List<Tuple<string, string>> result = new List<Tuple<string, string>>();
        var enumList = Enum.GetValues(_enum.GetType());
        foreach (var e in enumList)
        {
            if (e == null)
                continue;
            yield return (e.ToString() ?? "", ((Enum)e).GetDescription());
        }
    }
    /// <summary>
    /// Returns an array of tuples containing each value of the specified enumeration type and its associated
    /// description.
    /// </summary>
    /// <remarks>The description for each enumeration value is obtained using the value's Description()
    /// method. If an enumeration value does not have a description, it is excluded from the result.</remarks>
    /// <param name="_enum">The type of the enumeration to retrieve values and descriptions from. Must be a valid enum type.</param>
    /// <returns>An array of tuples, where each tuple contains an enumeration value and its description. Only values with
    /// non-empty descriptions are included.</returns>
    public static Tuple<Enum, string>[] GetEnumListWithDescription(Type _enum)
    {
        List<Tuple<Enum, string>> result = new List<Tuple<Enum, string>>();
        var enumList = Enum.GetValues(_enum);
        foreach (var e in enumList)
        {
            var value = (Enum)e;
            string description = value.Description();
            if (description != "")
                result.Add(new Tuple<Enum, string>(value, description));
        }
        return result.ToArray();
    }
}
