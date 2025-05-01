using Sabatex.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabatex.ObjectsExchange.ApiAdapter;

public class UploadObject:IEntityBase<Guid>
{
    public Guid Id { get; set; }
    public ExchangeNode Node { get; set; }
    public Guid NodeId { get; set; }
    public string MessageHeader { get; set; }
    public string Message { get; set; }
    public DateTime DateStamp { get; set; }
}
