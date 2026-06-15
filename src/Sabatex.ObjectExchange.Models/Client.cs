using Sabatex.Core;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Sabatex.ObjectsExchange.Models;

/// <summary>
/// Client defined
/// </summary>
public class Client : IEntityBase<Guid>
{
    /// <summary>
    /// Primary key
    /// </summary>
    public Guid Id{ get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    public string Description { get; set; } = string.Empty;
    
    /// <summary>
    /// Owner user name (administrator@contoso.com)
    /// </summary>
    public string OwnerUser { get; set; } = default!;
    
    /// <summary>
    /// 
    /// </summary>
    [JsonIgnore]
    public IEnumerable<ClientNode>? ClientNodes { get; set; }
}
