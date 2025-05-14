using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabatex.ObjectExchange.Core
{
    public interface IExchangeObjectAnalizer
    {
        Task<bool> MessageAnalize(AnalizerObjectContextBase context, string messageHeader,string? message);
    }
}
