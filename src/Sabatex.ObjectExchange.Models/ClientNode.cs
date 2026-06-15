using ObjectsExchange.Client.Models;
using Sabatex.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;

namespace Sabatex.ObjectsExchange.Models;

/// <summary>
/// Base class for determine client node
/// </summary>
public class ClientNode:IEntityBase<Guid>
{
    /// <summary>
    /// Client ID - UUID string
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Frendly client name (not indexed)
    /// </summary>
    [MaxLength(100)]
    public string Name { get; set; } = default!;

    /// <summary>
    /// The password hash for current node
    /// </summary>
    [Display(Name = "Password")]
    public string Password { get; set; } = default!;
    
    /// <summary>
    /// Client entity
    /// </summary>
    public Client? Client { get; set; }
    
    /// <summary>
    /// Foregin key for Client
    /// </summary>
    public Guid ClientId { get; set; }
    
    /// <summary>
    /// Calculated objects 
    /// </summary>
    [NotMapped]
    public int ObjectsCount { get; set; }
    
    /// <summary>
    /// Reset counter time (drop counter every day
    /// </summary>
    public DateTime CounterReseted { get; set; }
    
    /// <summary>
    /// list messages
    /// </summary>
    [JsonIgnore]
    public IEnumerable<ObjectExchange>? Objects { get; set; }

    /// <summary>
    /// Client description
    /// </summary>
    /// 
    public string? Description { get; set; }
    
    /// <summary>
    /// Client list id's with delimiters char ; for access node  
    /// </summary>
    public string? ClientAccess { get; set; }

    /// <summary>
    /// Get access nodes id`s
    /// </summary>
    /// <returns></returns>
    public IEnumerable<Guid> GetClientAccess() => ClientAccess?.Split(',').Select(s => new Guid(s)) ?? new Guid[] { };

    /// <summary>
    /// Demo mode (limit count transactions and trolling)
    /// </summary>
    public bool IsDemo { get; set; } = true;

    /// <summary>
    /// Transactions counter
    /// </summary>
    public uint Counter { get; set; }

    /// <summary>
    /// limit transactions per day (drop count every day)
    /// </summary>
    public uint MaxOperationPerDay { get; set; } = 1000;



}
