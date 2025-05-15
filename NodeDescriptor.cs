using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabatex.ObjectExchange.Core
{
    public class NodeDescriptor
    {
        private readonly Dictionary<string,Type> _analizers = new Dictionary<string, Type>();
        private readonly ExchangeNode _exchangeNode;
        public NodeDescriptor(ExchangeNode exchangeNode)
        {
            _exchangeNode = exchangeNode;
        }


        public void RegisterAnalizer<T>(string objectType) where T : IObjectAnalizer
        {
            if (_analizers.ContainsKey(objectType.ToLower()))
            {
                throw new ArgumentException($"Object type {objectType} already registered");
            }
            _analizers.Add(objectType.ToLower(), typeof(T));
        }


        public IObjectAnalizer? GetObjectAnalizer(string objectType)
        {
            if (_analizers.TryGetValue(objectType.ToLower(), out var analizer))
            {
                return  Activator.CreateInstance(analizer) as IObjectAnalizer;
            }
            return null;
        }
    }
}
