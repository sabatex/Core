using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabatex.ObjectsExchange.ApiAdapter;

public interface IClientExchangeDataAdapter
{
    Task<Guid> RegisterMessageAsync(Guid destination, string messageHeader, string message);
    Task RemoveUploadMessageAsync(Guid destination, Guid id);
    Task<IEnumerable<UploadObject>> GetUploadObjectsAsync(Guid destinationNode, int take = 10);
    Task RegisterUnresolvedMessageAsync(Guid destination, UnresolvedObject unresolvedObject);
    Task RemoveUnresolvedMessageAsync(Guid destination, Guid id);

}
