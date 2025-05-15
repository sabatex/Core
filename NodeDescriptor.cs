using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabatex.ObjectExchange.Core
{
    public class NodeDescriptor
    {
        private readonly Dictionary<string,IObjectAnalizer> _analizers = new Dictionary<string, IObjectAnalizer>();
        private readonly ExchangeNode _exchangeNode;
        public NodeDescriptor(ExchangeNode exchangeNode)
        {
            _exchangeNode = exchangeNode;
        }

        public IObjectAnalizer? GetObjectAnalizer(string objectType)
        {
            if (_analizers.TryGetValue(objectType.ToLower(), out var analizer))
                return analizer;
            return null;
        }
    }
}
