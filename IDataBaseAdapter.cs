using Sabatex.ObjectsExchange.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabatex.ObjectsExchange.ApiAdapter;

public interface IDataBaseAdapter
{
    /// <summary>
    /// store unresolved objects to database and return transaction
    /// </summary>
    /// <param name="objects"></param>
    /// <returns></returns>
    Task<string> DownloadObjectsAsync(ExchangeNode exchangeNode);         
}
