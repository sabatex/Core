using Sabatex.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabatex.ObjectExchange.Core;
/// <summary>
/// Represents an object that is being uploaded in the context of an object exchange system. This class implements the IEntityBase interface with a Guid as the identifier type, allowing it to be used as a base entity in a data storage system. The UploadObject class contains properties for storing information about the upload, including the associated ExchangeNode, message header, message content, and a timestamp for when the upload occurred.
/// </summary>
public class UploadObject:IEntityBase<Guid>
{
    /// <summary>
    /// Gets or sets the unique identifier for the upload object. This property is of type Guid and serves as the primary key for the entity in a data storage system. It is used to uniquely identify each instance of the UploadObject class.
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// Gets or sets the associated ExchangeNode for the upload object. This property represents the node in the object exchange system to which the upload object is related.
    /// </summary>
    public ExchangeNode? Node { get; set; }
    /// <summary>
    /// Gets or sets the unique identifier of the associated ExchangeNode. This property is used to establish a relationship between the upload object and the exchange node.
    /// </summary>
    public Guid NodeId { get; set; }
    /// <summary>
    /// Gets or sets the header of the message associated with the upload object.
    /// </summary>
    public string MessageHeader { get; set; }= string.Empty;
    /// <summary>
    /// Gets or sets the content of the message associated with the upload object.
    /// </summary>
    public string Message { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the timestamp indicating when the upload object was created or last modified.
    /// </summary>
    public DateTimeOffset DateStamp { get; set; }
}
