
using Sabatex.Core.ClassExtensions;
using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;

namespace Sabatex.Core.DateTimeExtensions
{
    /// <summary>
    /// Represents a time period defined by optional start and end dates.
    /// </summary>
    /// <remarks>A period can have either or both endpoints undefined (null), representing an open-ended
    /// interval. The struct provides factory methods for common periods such as day, week, month, quarter, and year,
    /// based on a specified date. Periods are considered equal if both their start and end dates are equal. The maximum
    /// supported range is defined by the MaxRange constant.</remarks>
    [TypeConverter(typeof(PeriodConverter))]
    public struct Period : IEquatable<Period>
    {
        /// <summary>
        /// Gets or sets the start date and time of the period, or null if no start is specified.
        /// </summary>
        public DateTime? Begin { get; set; }
        /// <summary>
        /// Gets or sets the end date and time for the associated event or time interval.
        /// </summary>
        public DateTime? End { get; set; }
        //public virtual DateTime? Begin { get => begin; set => SetProperty(ref begin, value); }
        //public virtual DateTime? End { get=>end; set=>SetProperty(ref end,value);}

        /// <summary>
        /// Max years range
        /// </summary>
        public const int MaxRange = 255;
        /// <summary>
        /// Initializes a new instance of the Period class that represents the entire day containing the specified date.
        /// </summary>
        /// <param name="date">The date for which to create a period covering the full day. The time component is ignored.</param>
        public Period(DateTime date) : this(date.BeginOfDay(), date.EndOfDay()) { }
        /// <summary>
        /// Initializes a new instance of the Period class with the specified start and end dates.
        /// </summary>
        /// <param name="beginDate">The start date of the period, or null if the period has no defined beginning.</param>
        /// <param name="endDate">The end date of the period, or null if the period has no defined end.</param>
        public Period(DateTime? beginDate, DateTime? endDate)
        {
            Begin = beginDate;
            End = endDate;
        }
        /// <summary>
        /// Creates a period representing the entire day that contains the specified date.
        /// </summary>
        /// <param name="date">The date for which to create the day period. The time component is used to determine the day.</param>
        /// <returns>A Period that starts at the beginning of the specified day and ends at the end of the same day.</returns>
        public static Period GetDay(DateTime date) => new Period(date.BeginOfDay(), date.EndOfDay());
        /// <summary>
        /// Creates a period representing the entire month that contains the specified date.
        /// </summary>
        /// <param name="date">A date within the month to retrieve. The time component is included in the calculation.</param>
        /// <returns>A Period object that starts at the first day of the month at 00:00:00 and ends at the last day of the same
        /// month at 23:59:59.9999999.</returns>
        public static Period GetMonth(DateTime date) => new Period(date.BeginOfMonth(), date.EndOfMonth());
        /// <summary>
        /// Returns a period representing the quarter of the specified date.
        /// </summary>
        /// <param name="date">The date for which to determine the corresponding quarter.</param>
        /// <returns>A <see cref="Period"/> that starts at the beginning of the quarter and ends at the end of the quarter
        /// containing <paramref name="date"/>.</returns>
        public static Period GetQuarter(DateTime date) => new Period(date.BeginOfQuarter(), date.EndOfQuarter());
        /// <summary>
        /// Creates a period representing the entire year that contains the specified date.
        /// </summary>
        /// <param name="date">The date for which to obtain the corresponding year period.</param>
        /// <returns>A Period that starts at the beginning of the year and ends at the end of the year containing the specified
        /// date.</returns>
        public static Period GetYear(DateTime date) => new Period(date.BeginOfYear(), date.EndOfYear());
        /// <summary>
        /// Returns a period representing the week that contains the specified date.
        /// </summary>
        /// <param name="date">The date for which to determine the corresponding week.</param>
        /// <returns>A <see cref="Period"/> that starts at the beginning of the week and ends at the end of the week containing
        /// <paramref name="date"/>.</returns>
        public static Period GetWeek(DateTime date) => new Period(date.BeginOfWeek(), date.EndOfWeek());
        /// <summary>
        /// Parses a string representation of a period into a new Period instance.
        /// </summary>
        /// <remarks>The input string should be in the format "start,end", where each part is either a
        /// date string or "null". The start date is interpreted as the beginning of the specified day, and the end date
        /// as the end of the specified day.</remarks>
        /// <param name="value">A string containing two comma-separated date values, where each value is either a date in a recognized
        /// format or the literal "null" to indicate an unspecified boundary.</param>
        /// <returns>A Period instance representing the parsed start and end dates. If both values are "null" or the input does
        /// not contain a comma, returns a Period with unspecified boundaries.</returns>
        /// <exception cref="ArgumentNullException">Thrown if value is an empty string.</exception>
        public static Period Parse(string value)
        {
            if (value.Length == 0)
                throw new ArgumentNullException(nameof(value));
            int pos = value.IndexOf(',');
            if (pos != -1)
            {
                   string s1 = value.Substring(0, pos);
                    DateTime? d1 = s1 == "null" ? new DateTime?() : DateTime.Parse(s1).BeginOfDay();
                    string s2 = value.Substring(pos + 1);
                    DateTime? d2 = s2 == "null" ? new DateTime?() : DateTime.Parse(s2).EndOfDay();
                    return new Period(d1, d2);
            }
            return new Period();
        }
        /// <summary>
        /// Determines whether the specified object is equal to the current Period instance.
        /// </summary>
        /// <remarks>Equality is determined by comparing the Begin and End properties of both Period
        /// instances. This method overrides Object.Equals(Object).</remarks>
        /// <param name="obj">The object to compare with the current Period instance.</param>
        /// <returns>true if the specified object is a Period and has the same Begin and End values as the current instance;
        /// otherwise, false.</returns>
        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if (obj.GetType() != typeof(Period))
                return false;
            var p = (Period)obj;
            return ((this.Begin == p.Begin) && (this.End == p.End));
        }

        /// <summary>
        /// 14 bit Begin.Year 8 bit (Range years ) 9 bit (range days)
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            if (Begin == null)
            {
                if (End == null) return 0x4e7e0000;
                return 0x4e5e0000 | (End.Value.Year * 366 + End.Value.DayOfYear);
            }
            else
            {

                if (End == null) return 0x4e3e0000 | (Begin.Value.Year * 366 + Begin.Value.DayOfYear);
                return (Begin.Value.Year << 17) | (((End.Value.Year * 366 + End.Value.DayOfYear) - (Begin.Value.Year * 366 + Begin.Value.DayOfYear)) & 0x1FFFF);
            }
        }
        /// <summary>
        /// Determines whether two specified Period instances are equal.
        /// </summary>
        /// <remarks>Equality is determined by comparing the values of the Period instances. This operator
        /// returns the same result as the Equals method.</remarks>
        /// <param name="arg1">The first Period to compare.</param>
        /// <param name="arg2">The second Period to compare.</param>
        /// <returns>true if the two Period instances are equal; otherwise, false.</returns>
        public static bool operator ==(Period arg1, Period arg2)
        {
            return arg1.Equals(arg2);
        }
        /// <summary>
        /// Determines whether two specified Period instances are not equal.
        /// </summary>
        /// <param name="arg1">The first Period to compare.</param>
        /// <param name="arg2">The second Period to compare.</param>
        /// <returns>true if the specified Period instances are not equal; otherwise, false.</returns>
        public static bool operator !=(Period arg1, Period arg2)
        {
            return !arg1.Equals(arg2);
        }
        /// <summary>
        /// Returns a string that represents the period defined by the Begin and End dates.
        /// </summary>
        /// <returns>A string in the format "Period from {Begin} to {End}", where {Begin} and {End} are the short date
        /// representations of the Begin and End properties, or "null" if either is not set.</returns>
        public override string ToString()
        {
            var s1 = Begin == null ? "null" : Begin.Value.ToShortDateString();
            var s2 = End == null ? "null" : End.Value.ToShortDateString();
            return string.Format("Period from {0} to {1}", s1, s2);
        }
        /// <summary>
        /// Determines whether the current Period instance is equal to the specified Period.
        /// </summary>
        /// <param name="other">The Period to compare with the current instance.</param>
        /// <returns>true if the Begin and End values of both Period instances are equal; otherwise, false.</returns>
        public bool Equals(Period other)
        {
            return Begin == other.Begin && End == other.End;
        }
    }

}

