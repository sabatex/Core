using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;

namespace Sabatex.Core.Converters.Json;
/// <summary>
/// Converts <see cref="DateOnly"/> values to and from JSON using the ISO 8601 date format ("yyyy-MM-dd").
/// </summary>
/// <remarks>This converter enables serialization and deserialization of <see cref="DateOnly"/> values with <see
/// cref="System.Text.Json"/>. It ensures that dates are represented in a consistent, culture-invariant format
/// compatible with most JSON consumers. Use this converter when working with <see cref="DateOnly"/> properties in types
/// that are serialized or deserialized with <see cref="System.Text.Json"/>.</remarks>
public class DateOnlyConverter : JsonConverter<DateOnly>
{
    private const string DateFormat = "yyyy-MM-dd";
    /// <summary>
    /// Reads and converts the JSON string representation of a date to a DateOnly value using the specified format and
    /// culture.
    /// </summary>
    /// <remarks>The JSON string must match the expected date format exactly. If the string is not in the
    /// correct format, a FormatException is thrown.</remarks>
    /// <param name="reader">The reader to read the JSON value from. The reader must be positioned at a JSON string token representing a
    /// date.</param>
    /// <param name="typeToConvert">The type of the object to convert. This parameter is ignored for this converter.</param>
    /// <param name="options">The serialization options to use. This parameter is ignored for this converter.</param>
    /// <returns>A DateOnly value parsed from the JSON string.</returns>
    public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return DateOnly.ParseExact(reader.GetString() ?? String.Empty, DateFormat, CultureInfo.InvariantCulture);
    }
    /// <summary>
    /// Writes the specified DateOnly value as a JSON string using the configured date format.
    /// </summary>
    /// <param name="writer">The Utf8JsonWriter to which the JSON value will be written. Cannot be null.</param>
    /// <param name="value">The DateOnly value to write as a JSON string.</param>
    /// <param name="options">The serialization options to use when writing the value. This parameter is provided for compatibility and may
    /// influence serialization behavior.</param>
    public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString(DateFormat, CultureInfo.InvariantCulture));
    }
}