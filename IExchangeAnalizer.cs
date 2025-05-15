using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabatex.ObjectExchange.Core;

public interface IExchangeAnalizer
{
    Task<AnalizeResult> MessageAnalizeAsync(ExchangeNode exchangeNode, string messageHeader,string? message);
}
public sealed record AnalizeResult(bool Success, string? ErrorMessage = null);