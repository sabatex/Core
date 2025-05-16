using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabatex.ObjectsExchange.Core;

/// <summary>
/// Options for Sabatex Exchange
/// </summary>
public class SabatexExchangeOptions
{
    /// <summary>
    /// Client Id
    /// </summary>
    public Guid ClientId { get; set; }
    /// <summary>
    /// Client secret code
    /// </summary>
    public string?  ClientSecret { get; set; }
    /// <summary>
    /// API URL (default is https://sabatex.francecentral.cloudapp.azure.com)
    /// </summary>
    public string ApiUrl { get; set; } = "https://sabatex.francecentral.cloudapp.azure.com";
}
