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
/// Converts between JSON string representations and <see cref="TimeOnly"/> values using a fixed format for
/// serialization and deserialization.
/// </summary>
/// <remarks>This converter serializes <see cref="TimeOnly"/> values as strings in the format
/// "0001-01-01THH:mm:ss" to ensure compatibility with systems expecting a date-time string. When deserializing, only
/// strings matching this format are supported. This converter is typically used when working with <see
/// cref="System.Text.Json"/> and <see cref="TimeOnly"/> values that require a consistent string
/// representation.</remarks>
public class TimeOnlyConverter : JsonConverter<TimeOnly>
{
    private const string DateTimeFormat = "0001-01-01THH:mm:ss";
    private const string TimeFormat = "HH:mm:ss.FFFFFFF";
    /// <summary>
    /// Reads and converts the JSON string representation of a time to a <see cref="TimeOnly"/> value using the
    /// specified format and culture.
    /// </summary>
    /// <remarks>The JSON string must match the expected time format exactly. If the string does not conform
    /// to the required format, a <see cref="FormatException"/> is thrown.</remarks>
    /// <param name="reader">The reader to read the JSON value from. The reader must be positioned at a JSON string token representing a
    /// time.</param>
    /// <param name="typeToConvert">The type of the object to convert. This parameter is not used.</param>
    /// <param name="options">The serialization options to use. This parameter is not used.</param>
    /// <returns>A <see cref="TimeOnly"/> value that represents the time parsed from the JSON string.</returns>
    public override TimeOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return TimeOnly.ParseExact(reader.GetString() ?? String.Empty, DateTimeFormat, CultureInfo.InvariantCulture);
    }
    /// <summary>
    /// Writes the specified TimeOnly value as a JSON string using the configured date and time format.
    /// </summary>
    /// <param name="writer">The Utf8JsonWriter to which the JSON value will be written. Cannot be null.</param>
    /// <param name="value">The TimeOnly value to convert and write as a JSON string.</param>
    /// <param name="options">The serialization options to use when writing the value. This parameter is provided for compatibility and may
    /// influence custom converters.</param>
    public override void Write(Utf8JsonWriter writer, TimeOnly value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString(DateTimeFormat, CultureInfo.InvariantCulture));
    }
}