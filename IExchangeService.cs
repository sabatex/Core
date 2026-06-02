using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabatex.ObjectExchange.Core;
/// <summary>
/// Defines an interface for an exchange service that facilitates the exchange of data between nodes and a database. The IExchangeService interface declares a method for performing the exchange operation, which can be executed asynchronously. The method takes a CancellationToken to allow for cancellation of the exchange process and an optional boolean parameter to indicate whether the exchange should be performed as tasks. Implementing classes will provide the logic for how the exchange is conducted, including downloading data from nodes, processing it, and uploading results back to the nodes or database as needed.
/// </summary>
public interface IExchangeService
{
    /// <summary>
    /// Performs the exchange operation between nodes and the database. This method is asynchronous and can be cancelled using the provided CancellationToken. The asTasks parameter indicates whether the exchange should be performed as tasks, allowing for concurrent processing of multiple exchange operations if set to true. Implementing classes will define the specific logic for how the exchange is conducted, including handling data retrieval, processing, and storage as necessary.
    /// </summary>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <param name="asTasks">Indicates whether the exchange should be performed as tasks for concurrent processing.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task Exchange(CancellationToken cancellationToken, bool asTasks = false);
}
