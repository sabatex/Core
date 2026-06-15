using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabatex.ObjectExchange.Core;
/// <summary>
/// Режим обміну
/// </summary>
public enum ExchangeMode
{
    /// <summary>
    /// Ручний
    /// </summary>
    [Display(Description = "Manual")]
    Manual,
    /// <summary>
    /// Автоматичний
    /// </summary>
    [Display(Description ="Auto")]
    Auto
}
