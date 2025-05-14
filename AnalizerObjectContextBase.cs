using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabatex.ObjectExchange.Core
{
    public abstract class AnalizerObjectContextBase
    {
        public string ObjectType { get; set; } = string.Empty;
        public string ObjectId { get; set; } = string.Empty;
        public bool Success { get; private set; } = true;
        public readonly ExchangeNode ExchangeNode;
        public readonly JsonValueWrapper Json;
        List<string> _errorMessages { get; }

        public IEnumerable<string> ErrorMessages => ErrorMessages;


        public Dictionary<string,ObjectAnalizer> Analizers { get; }

       public AnalizerObjectContextBase(ExchangeNode ExchangeNode,JsonValueWrapper json)
       {
            this.ExchangeNode = ExchangeNode;
            this.Json = json;
            _errorMessages = new List<string>();
            Analizers = new Dictionary<string, ObjectAnalizer>();
       }

        public void AddAnalizer(ObjectAnalizer analizer)
        {
            if (analizer == null)
            {
                throw new ArgumentNullException(nameof(analizer), "Analizer cannot be null");
            }
            Analizers.Add(analizer.ObjectType.ToLower(), analizer);
        }

        public bool Error(string message)
        {
            Success = false;
            _errorMessages.Add(message);
            return false;
        }
 
 
    }
}
