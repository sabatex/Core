using System;
using System.ComponentModel;
using System.Globalization;

namespace Sabatex.Core.ClassExtensions;

/// <summary>
/// Provides extension methods for the DateTime structure to simplify common date and time calculations, such as
/// obtaining the start or end of a day, week, month, quarter, or year, as well as determining the quarter or week
/// number for a given date.
/// </summary>
/// <remarks>All methods in this class are implemented as extension methods for the DateTime type, enabling fluent
/// and readable date manipulation. These methods preserve the Kind property of the input DateTime where applicable. The
/// calculations assume the default .NET conventions for week and quarter boundaries (weeks start on Sunday, quarters
/// follow calendar quarters).</remarks>
public static class DateTimeExtension
{
    /// <summary>
    /// Returns a new DateTime instance set to the beginning of the day (midnight) for the specified date.
    /// </summary>
    /// <param name="dt">The date and time value for which to obtain the start of the day.</param>
    /// <returns>A DateTime representing 12:00:00 AM on the same date as <paramref name="dt"/>.</returns>
    public static DateTime BeginOfDay(this DateTime dt)
    {
        return new DateTime(dt.Year, dt.Month, dt.Day, 0, 0, 0);
    }
    /// <summary>
    /// Returns a new <see cref="DateTime"/> instance representing the last moment of the day for the specified date.
    /// </summary>
    /// <remarks>The returned value preserves the <see cref="DateTime.Kind"/> of the input. Milliseconds are
    /// set to zero.</remarks>
    /// <param name="dt">The date and time value for which to calculate the end of the day.</param>
    /// <returns>A <see cref="DateTime"/> set to 23:59:59 on the same date as <paramref name="dt"/>, with the same <see
    /// cref="DateTime.Kind"/> value.</returns>
    public static DateTime EndOfDay(this DateTime dt)
    {
        return new DateTime(dt.Year, dt.Month, dt.Day, 23, 59, 59);
    }
    /// <summary>
    /// Returns a new DateTime representing the start of the week for the specified date, with the time component set to
    /// midnight.
    /// </summary>
    /// <remarks>The start of the week is considered to be Sunday. The returned DateTime has the same Kind as
    /// the input value.</remarks>
    /// <param name="dt">The date for which to determine the beginning of the week.</param>
    /// <returns>A DateTime value set to the first day of the week containing the specified date, with the time set to 00:00:00.</returns>
    public static DateTime BeginOfWeek(this DateTime dt)
    {
        return BeginOfDay(dt.AddDays(1 - (int)(dt.DayOfWeek)));
    }
    /// <summary>
    /// Returns a new <see cref="DateTime"/> representing the last moment of the week that contains the specified date.
    /// </summary>
    /// <remarks>This method assumes the week starts on Sunday and ends on Saturday, following the default
    /// <see cref="DayOfWeek"/> enumeration. The returned value preserves the <see cref="DateTime.Kind"/> of the
    /// input.</remarks>
    /// <param name="dt">The date for which to determine the end of the week.</param>
    /// <returns>A <see cref="DateTime"/> set to the last moment (23:59:59.999) of the week containing <paramref name="dt"/>. The
    /// week is considered to start on Sunday and end on Saturday.</returns>
    public static DateTime EndOfWeek(this DateTime dt)
    {
        return EndOfDay(dt.AddDays(7 - (int)(dt.DayOfWeek)));
    }
    /// <summary>
    /// Returns a new DateTime representing the first moment of the month for the specified date.
    /// </summary>
    /// <param name="dt">The date for which to determine the beginning of the month.</param>
    /// <returns>A DateTime set to the first day of the month at 00:00:00, with the same year and month as the specified date.</returns>
    public static DateTime BeginOfMonth(this DateTime dt)
    {
        return new DateTime(dt.Year, dt.Month, 1, 0, 0, 0);
    }
    /// <summary>
    /// Returns a new DateTime representing the last moment of the month for the specified date.
    /// </summary>
    /// <remarks>The returned DateTime has the same year and month as the input date, and its Kind property
    /// matches that of the input. This method does not account for fractions of a second; the time is set to the last
    /// whole second of the month.</remarks>
    /// <param name="dt">The date for which to determine the end of the month.</param>
    /// <returns>A DateTime set to the last day of the month of the specified date, with the time set to 23:59:59.</returns>
    public static DateTime EndOfMonth(this DateTime dt)
    {
        return new DateTime(dt.Year, dt.Month, DateTime.DaysInMonth(dt.Year, dt.Month), 23, 59, 59);
    }
    /// <summary>
    /// Returns a new DateTime instance representing the first moment of the year for the specified date.
    /// </summary>
    /// <param name="dt">The date for which to retrieve the beginning of the year.</param>
    /// <returns>A DateTime value set to 12:00:00 AM on January 1 of the year of <paramref name="dt"/>.</returns>
    public static DateTime BeginOfYear(this DateTime dt)
    {
        return new DateTime(dt.Year, 1, 1, 0, 0, 0);
    }
    /// <summary>
    /// Returns a new DateTime representing the last moment of the year for the specified date.
    /// </summary>
    /// <param name="dt">The date for which to determine the end of the year.</param>
    /// <returns>A DateTime set to December 31st of the year of <paramref name="dt"/>, at 23:59:59.</returns>
    public static DateTime EndOfYear(this DateTime dt)
    {
        return new DateTime(dt.Year, 12, 31, 23, 59, 59);
    }
    /// <summary>
    /// Returns the quarter of the year that contains the specified date.
    /// </summary>
    /// <param name="dt">The date for which to determine the quarter.</param>
    /// <returns>An integer from 1 to 4 representing the quarter of the year that contains the specified date.</returns>
    public static int Quarter(this DateTime dt)
    {
        switch (dt.Month)
        {
            case 1:
            case 2:
            case 3:
                return 1;
            case 4:
            case 5:
            case 6:
                return 2;
            case 7:
            case 8:
            case 9:
                return 3;
            default:
                return 4;
        }
    }
    /// <summary>
    /// Returns a new DateTime representing the first moment of the quarter in which the specified date occurs.
    /// </summary>
    /// <param name="dt">The date for which to determine the beginning of the quarter.</param>
    /// <returns>A DateTime set to 00:00:00 on the first day of the quarter containing the specified date. The returned value has
    /// the same year as the input date.</returns>
    public static DateTime BeginOfQuarter(this DateTime dt)
    {
        switch (dt.Quarter())
        {
            case 1:
                return new DateTime(dt.Year, 1, 1, 0, 0, 0);
            case 2:
                return new DateTime(dt.Year, 4, 1, 0, 0, 0);
            case 3:
                return new DateTime(dt.Year, 7, 1, 0, 0, 0);
            default:
                return new DateTime(dt.Year, 10, 1, 0, 0, 0);

        }
    }
    /// <summary>
    /// Returns a new DateTime representing the last moment of the quarter in which the specified date occurs.
    /// </summary>
    /// <remarks>The returned DateTime preserves the year of the input date and sets the time to 23:59:49.
    /// This method can be used to determine quarter boundaries for reporting or time period calculations.</remarks>
    /// <param name="dt">The date for which to determine the end of the quarter.</param>
    /// <returns>A DateTime set to 23:59:49 on the last day of the quarter containing the specified date.</returns>
    public static DateTime EndOfQuarter(this DateTime dt)
    {
        switch (dt.Quarter())
        {
            case 1:
                return new DateTime(dt.Year, 3, 31, 23, 59, 49);
            case 2:
                return new DateTime(dt.Year, 6, 30, 23, 59, 49);
            case 3:
                return new DateTime(dt.Year, 9, 30, 23, 59, 49);
            default:
                return new DateTime(dt.Year, 12, 31, 23, 59, 49);

        }
    }
    /// <summary>
    /// Calculates the week number of the year for the specified date, using the current culture's calendar week rule
    /// and first day of the week.
    /// </summary>
    /// <remarks>The calculation uses the calendar week rule and first day of the week from the current
    /// thread's culture. Results may vary depending on the culture settings in effect at the time of the
    /// call.</remarks>
    /// <param name="dt">The date for which to determine the week of the year.</param>
    /// <returns>The week number of the year that contains the specified date, as defined by the current culture's calendar
    /// settings.</returns>
    public static int WeekOfYear(this DateTime dt)
    {
        Calendar cl = CultureInfo.InvariantCulture.Calendar;
        return cl.GetWeekOfYear(dt, DateTimeFormatInfo.CurrentInfo.CalendarWeekRule, DateTimeFormatInfo.CurrentInfo.FirstDayOfWeek);
    }
}
