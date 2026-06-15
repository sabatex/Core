using Sabatex.ObjectExchange.Core;
using Sabatex.ObjectsExchange.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabatex.ObjectsExchange.ApiAdapter;

public class AccessDeniedException:Exception
{
    public AccessDeniedException(ExchangeNode clientNode):base($"The access denied for node {clientNode.Description}")
    {
    
    }
}
