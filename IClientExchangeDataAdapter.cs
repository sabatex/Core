using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabatex.ObjectExchange.Core;

public interface IClientExchangeDataAdapter
{
    Task RegisterUploadMessageAsync(Guid destination, string messageHeader, string message);
    Task RemoveUploadMessageAsync(Guid destination, Guid id);
    Task<IEnumerable<UploadObject>> GetUploadMessagesAsync(Guid destinationNode, int take = 10);
    
    Task RegisterUnresolvedMessageAsync(Guid destination, UnresolvedObject unresolvedObject);
    Task RemoveUnresolvedMessageAsync(Guid destination, Guid id);
    Task<IEnumerable<UnresolvedObject>> GetUnresolvedMessagesAsync(Guid destinationNode, int take = 10);
    /// <summary>
    /// Реєстрація статусу обробки повідомлення.
    /// </summary>
    /// <param name="destination"></param>
    /// <param name="id"></param>
    /// <param name="state">Статус обробки</param>
    /// <returns></returns>
    Task RegisterUnresolvedMessageStatusAsync(Guid destination, Guid id, string state);



    /// <summary>
    /// Отримати список вузлів обміну даними.
    /// </summary>
    /// <returns>Список активних вузлів обміну</returns>
    Task<IEnumerable<ExchangeNode>> GetExchangeNodesAsync();

}
