using Sabatex.ObjectExchange.Core;
using Sabatex.ObjectsExchange.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Sabatex.ObjectsExchange.ApiAdapter;

public interface IExchangeApiAdapter:IDisposable
{
    Guid ClientId { get; }
    Task<bool> RefreshTokenAsync();
    Task<IEnumerable<Sabatex.ObjectsExchange.Models.ObjectExchange>> GetObjectsAsync(Guid destinationNode,int take=10);
    Task PostObjectAsync(Guid destinationId, string messageHeader, string message, DateTime? dateStamp);
    Task DeleteObjectAsync(long objectId);
    Task DeleteRange(long[] ids);
 }
