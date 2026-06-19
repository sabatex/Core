using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sabatex.Core;

namespace Sabatex.ObjectExchange.Core;
/// <summary>
/// Represents an unresolved object in the exchange system, containing information about the object, its state, and relevant timestamps.
/// </summary>
public class UnresolvedObject: IEntityBase<Guid>
{
    /// <summary>
    /// Gets or sets the unique identifier for the unresolved object.
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// Gets or sets the unique identifier of the associated ExchangeNode.
    /// </summary>
    public Guid NodeId { get; set; }
    /// <summary>
    /// Gets or sets the header of the message associated with the unresolved object.
    /// </summary>
    public string MessageHeader { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the content of the message associated with the unresolved object.
    /// </summary>
    public string? Message { get; set; }
    /// <summary>
    /// Gets or sets the timestamp indicating when the unresolved object was created or last modified.
    /// </summary>
    public DateTimeOffset DateStamp { get; set; }
    /// <summary>
    /// Gets or sets the current state of the unresolved object.
    /// </summary>
    public string State { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the timestamp indicating when the unresolved object was sent by the sender.
    /// </summary>
    public DateTimeOffset? SenderDateStamp { get; set; }
    /// <summary>
    /// Gets or sets the timestamp indicating when the unresolved object was processed by the server.
    /// </summary>
    public DateTimeOffset ServerDateStamp { get; set; }
    /// <summary>
    /// Gets or sets the live level of the unresolved object.
    /// </summary>
    public short LiveLevel { get; set; }
}
