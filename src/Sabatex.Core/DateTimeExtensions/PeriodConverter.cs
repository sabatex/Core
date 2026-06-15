using Sabatex.Core.ClassExtensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Text;

namespace Sabatex.Core.DateTimeExtensions;
/// <summary>
/// Provides a type converter to convert Period objects to and from their string representations.
/// </summary>
/// <remarks>This converter supports conversion between Period instances and strings formatted as
/// "BeginDate,EndDate", where each date is represented in a short date string format or as "null" if the value is not
/// set. This is useful for serialization, property grid editing, or other scenarios where a Period needs to be
/// represented as a string.</remarks>
public class PeriodConverter : TypeConverter
{
    /// <summary>
    /// Determines whether this converter can convert the object to the specified destination type, using the provided
    /// context.
    /// </summary>
    /// <remarks>This implementation supports conversion only to the String type. Override this method to
    /// support additional destination types.</remarks>
    /// <param name="context">An ITypeDescriptorContext that provides contextual information about the component and its environment. This
    /// parameter can be null.</param>
    /// <param name="destinationType">The Type to which the object is to be converted. Must not be null.</param>
    /// <returns>true if the converter can perform the conversion to the specified destination type; otherwise, false.</returns>
    public override bool CanConvertTo(ITypeDescriptorContext? context, Type? destinationType)
    {
        return destinationType == typeof(string);
    }
    /// <summary>
    /// Converts a Period object to a specified destination type, typically a string representation.
    /// </summary>
    /// <remarks>If either the Begin or End property of the Period is null, the corresponding value in the
    /// output string will be "null". This method is typically used for serialization or display purposes.</remarks>
    /// <param name="context">An ITypeDescriptorContext that provides contextual information about the conversion, or null.</param>
    /// <param name="culture">A CultureInfo object that supplies culture-specific formatting information.</param>
    /// <param name="value">The Period object to convert. Must not be null.</param>
    /// <param name="destinationType">The type to convert the value to. Must be typeof(string) to obtain a string representation.</param>
    /// <returns>A string representing the Period in the format "Begin,End", where each date is formatted using
    /// ToShortDateString(), or "null" if a date is not set. Returns null if the destination type is not string.</returns>
    public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
    {
        if (destinationType == typeof(string))
        {
            if (value == null)
                return new Period();
            var dt = (Period)value;
            var d1 = dt.Begin;
            var s1 = d1 == null ? "null" : d1.Value.ToShortDateString();
            var d2 = dt.End;
            var s2 = d2 == null ? "null" : d2.Value.ToShortDateString();
            return s1 + "," + s2;
        }
        return null;
    }
    /// <summary>
    /// Determines whether this converter can convert an object of the specified source type to the type of this
    /// converter.
    /// </summary>
    /// <param name="context">An optional format context that provides information about the environment from which this converter is invoked.
    /// This parameter may be null.</param>
    /// <param name="sourceType">The type of the source object to evaluate for conversion.</param>
    /// <returns>true if the source type is a string; otherwise, false.</returns>
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
    {
        return sourceType == typeof(string);
    }
    /// <summary>
    /// Converts the specified value to a Period object, using the provided context and culture information.
    /// </summary>
    /// <remarks>The input string should be in the format "start,end", where "start" and "end" are date
    /// strings or the literal "null". If either part is "null", the corresponding date in the Period will be null. If
    /// parsing fails or the input is not a string, a default Period is returned.</remarks>
    /// <param name="context">An ITypeDescriptorContext that provides contextual information about the component and converter. This parameter
    /// can be null.</param>
    /// <param name="culture">A CultureInfo object that specifies the culture to use for parsing. This parameter can be null, in which case
    /// the current culture is used.</param>
    /// <param name="value">The value to convert. Must be a string in the format "start,end", where each part is either a date string or
    /// "null".</param>
    /// <returns>A Period object representing the parsed start and end dates. If the input is not a valid string or cannot be
    /// parsed, returns a Period with default values.</returns>
    public override object ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
    {
        if (value.GetType() == typeof(string))
        {
            string s = (string)value;
            if (s != "")
            {
                int pos = s.IndexOf(',');
                if (pos != -1)
                {
                    try
                    {
                        string s1 = s.Substring(0, pos);
                        DateTime? d1 = s1 == "null" ? new DateTime?() : DateTime.Parse(s1).BeginOfDay();
                        string s2 = s.Substring(pos + 1);
                        DateTime? d2 = s2 == "null" ? new DateTime?() : DateTime.Parse(s2).EndOfDay();
                        return new Period(d1, d2);
                    }
                    catch
                    { }
                }
            }
        }
        return new Period();
    }
}
