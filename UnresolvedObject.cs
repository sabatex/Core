using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sabatex.Core;

namespace Sabatex.ObjectExchange.Core;

public class UnresolvedObject: IEntityBase<Guid>
{
    public Guid Id { get; set; }
    public ExchangeNode Node { get; set; }
    public Guid NodeId { get; set; }
    public string MessageHeader { get; set; } = string.Empty;
    public string? Message { get; set; }
    public DateTime DateStamp { get; set; }
    public string State { get; set; }
    public DateTime? SenderDateStamp { get; set; }
    public DateTime ServerDateStamp { get; set; }
    public short LiveLevel { get; set; }
}
