using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabatex.ObjectExchange.Core
{
    public class NodeContext
    {
  

        private readonly ExchangeNode _exchangeNode;
        
        
        
        public NodeContext(ExchangeNode exchangeNode)
        {
            _exchangeNode = exchangeNode;
        }
    }
}
