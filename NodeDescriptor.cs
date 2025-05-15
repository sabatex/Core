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

        public Type GetAnalizerType(string objectType)
        {
            if (_analizers.TryGetValue(objectType.ToLower(), out var analizer))
            {
                return analizer;
            }
            throw new Exception($"Не знайдено аналізатор для типу {objectType}");
        }

        public IObjectAnalizer GetObjectAnalizer(string objectType)
        {
            if (_analizers.TryGetValue(objectType.ToLower(), out var analizer))
            {
                return (Activator.CreateInstance(analizer) ?? throw new Exception("Uknown error if create analizer")) as IObjectAnalizer ?? throw new Exception("The analizator not contains interface IObjectAnalizer");
            }
            throw new Exception($"Не знайдено аналізатор для типу {objectType}");
        }
    }
}
