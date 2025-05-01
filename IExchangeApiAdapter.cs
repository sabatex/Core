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
    Task<IEnumerable<ObjectExchange>> GetObjectsAsync(Guid destinationNode,int take=10);
    Task PostObjectAsync(Guid destinationId, string messageHeader, string message, DateTime? dateStamp);
    Task DeleteObjectAsync(long objectId);
    Task DeleteRange(long[] ids);
    /// <summary>
    /// Register message for send to destination
    /// </summary>
    /// <param name="destination"></param>
    /// <param name="messageHeader"></param>
    /// <param name="message"></param>
    /// <returns></returns>
     Task<Guid> RegisterMessageAsync(Guid destination, string messageHeader, string message);
    Task<IEnumerable<UploadObject>> GetUploadObjectsAsync(Guid destinationNode, int take = 10);
    Task RemoveUploadMessageAsync(Guid destination, Guid id);

}
