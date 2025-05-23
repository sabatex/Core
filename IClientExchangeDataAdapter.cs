using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabatex.ObjectExchange.Core;

public interface IClientExchangeDataAdapter
{
    /// <summary>
    /// Реэстрація повідомлення для відправки.
    /// </summary>
    /// <param name="nodeId">id нода з таблиці ExchangeNode (не destinationId)</param>
    /// <param name="messageHeader">Заголовк повідомлення</param>
    /// <param name="message">Саме повідомлення, для запитів може бути відсутнє</param>
    /// <returns></returns>

    Task RegisterUploadMessageAsync(Guid nodeId, string messageHeader, string message);
    Task RemoveUploadMessageAsync(Guid id);
    Task<IEnumerable<UploadObject>> GetUploadMessagesAsync(Guid destinationNode, int take = 10);
    /// <summary>
    /// Реєстрація повідомлення, яке ще не обробилось.
    /// </summary>
    /// <param name="nodeId">id нода з таблиці ExchangeNode (не destinationId)</param>
    /// <param name="unresolvedObject">вхідний обєкт/запит який буде аналізуватись</param>
    /// <returns></returns>
    Task RegisterUnresolvedMessageAsync(Guid nodeId, UnresolvedObject unresolvedObject);
    Task RemoveUnresolvedMessageAsync(Guid id);
    Task<IEnumerable<UnresolvedObject>> GetUnresolvedMessagesAsync(Guid nodeId, int take = 10);
    /// <summary>
    /// Реєстрація статусу обробки повідомлення.
    /// </summary>
    /// <param name="nodeId"></param>
    /// <param name="id"></param>
    /// <param name="state">Статус обробки</param>
    /// <returns></returns>
    Task RegisterUnresolvedMessageStatusAsync(Guid nodeId, Guid id, string state);
    /// <summary>
    /// Перевірка наявності повідомлення з таким же заголовком.
    /// </summary>
    /// <param name="nodeId">Нод призначення</param>
    /// <param name="messageHeader">заголовок повідомлення</param>
    /// <returns>true - message exist</returns>
    Task<bool> CheckExistUnresolvedMessage(Guid nodeId, string messageHeader);

    /// <summary>
    /// Отримати список вузлів обміну даними.
    /// </summary>
    /// <returns>Список активних вузлів обміну</returns>
    Task<IEnumerable<ExchangeNode>> GetExchangeNodesAsync();

}
