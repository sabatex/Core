using Sabatex.Core.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace sabatex.Core.DateTimeExtensions;

/// <summary>
/// Specifies the type of time period used for grouping or filtering data, such as year, quarter, month, week, or day.
/// </summary>
/// <remarks>Use this enumeration to indicate the granularity of a time-based operation or selection. The values
/// correspond to common calendar periods. The None value indicates that no specific period type is selected and may be
/// used as a default or to represent an unfiltered state.</remarks>
public enum PeriodType
{
    /// <summary>
    /// Represents the absence of a period type selection. (Default value) (Any period)
    /// </summary>
    [Display(Name = "PeriodType_None", ResourceType =typeof(Resources))]
    None = 0,
    /// <summary>
    /// Represents a period type that spans a full calendar year.
    /// </summary>
    [Display(Name = "PeriodType_Year", ResourceType = typeof(Resources))]
    Year = 1,
    /// <summary>
    /// Represents a quarterly period, typically spanning three months within a year.
    /// </summary>
    [Display(Name = "PeriodType_Quarter", ResourceType = typeof(Resources))]
    Quarter = 2,
    /// <summary>
    /// Represents a period type that corresponds to a month.
    /// </summary>
    [Display(Name = "PeriodType_Month", ResourceType = typeof(Resources))]
    Month = 3,
    /// <summary>
    /// Represents a period of one week.
    /// </summary>
    [Display(Name = "PeriodType_Week", ResourceType = typeof(Resources))]
    Week = 4,
    /// <summary>
    /// Represents a period of one day.
    /// </summary>
    [Display(Name = "PeriodType_Day", ResourceType = typeof(Resources))]
    Day = 5

}
