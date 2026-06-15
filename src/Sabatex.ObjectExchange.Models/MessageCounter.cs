using Sabatex.ObjectsExchange.Models;
using System;
using System.Reflection.PortableExecutable;

namespace ObjectsExchange.Client.Models;

/// <summary>
/// counters for node
/// </summary>
public class MessageCounter
{
    /// <summary>
    /// Primary key
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Client node reference
    /// </summary>
    //public ClientNode? ClientNode { get; set; }
    
    /// <summary>
    ///  foregin key for node
    /// </summary>
    //public Guid ClientNodeId { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int Counter { get; set; }
    /// <summary>
    /// 
    /// </summary>
    //public int TotalTransmited { get; set; }
    /// <summary>
    /// Last counter change
    /// </summary>
    public DateTime CounterChange { get; set; }
}
