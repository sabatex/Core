using Sabatex.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabatex.ObjectExchange.Core;

public class UploadObject:IEntityBase<Guid>
{
    public Guid Id { get; set; }
    public ExchangeNode? Node { get; set; }
    public Guid NodeId { get; set; }
    public string MessageHeader { get; set; }= string.Empty;
    public string Message { get; set; } = string.Empty;
    public DateTimeOffset DateStamp { get; set; }
}
