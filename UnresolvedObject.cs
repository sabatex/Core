using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sabatex.Core;

namespace Sabatex.ObjectsExchange.ApiAdapter;

public class UnresolvedObject:UploadObject
{
    public string Log { get; set; }
    public DateTime SenderDateStamp { get; set; }
    public DateTime ServerDateStamp { get; set; }
    public short LiveLevel { get; set; }
}
