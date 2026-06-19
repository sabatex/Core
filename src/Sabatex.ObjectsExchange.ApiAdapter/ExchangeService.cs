using Sabatex.Extensions.ClassExtensions;
using Sabatex.ObjectExchange.Core;
using Sabatex.ObjectsExchange.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Reflection.PortableExecutable;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

namespace Sabatex.ObjectsExchange.ApiAdapter;
/// <summary>
/// Відповідає за сеанс обміну даними між вузлом і базою даних.
/// </summary>
public class ExchangeService: IExchangeService
{
    public IExchangeApiAdapter ExchangeApiAdapter { get; private set; }
    public IClientExchangeDataAdapter  DataAdapter { get; private set; }
    public IExchangeAnalizer ExchangeAnalizer { get; private set; }

    /// <summary>
    /// Конструктор обміну даними між вузлом і базою даних.
    /// </summary>
    /// <param name="exchangeApiAdapter">Іні</param>
    /// <param name="dataAdapter"></param>
    public ExchangeService(IExchangeApiAdapter exchangeApiAdapter,
                           IClientExchangeDataAdapter dataAdapter,
                           IExchangeAnalizer exchangeAnalizer)
    {
        ExchangeApiAdapter = exchangeApiAdapter;
        DataAdapter = dataAdapter;
        ExchangeAnalizer = exchangeAnalizer;
    }


    /// <summary>
    /// Завантажити дані з вузла обміну даними та зареєструвати їх у базі даних як невирішені повідомлення.
    /// </summary>
    /// <param name="exchangeNode">Вузол обміну даними, з якого будуть завантажені дані.</param>
    /// <returns></returns>
    async Task DownloadAsync(ExchangeNode exchangeNode)
    {
        var objects = await ExchangeApiAdapter.GetObjectsAsync(exchangeNode.Id, exchangeNode.TakeDownload);
        foreach (var obj in objects)
        {
            var unresolvedObject = new UnresolvedObject
            {
                NodeId = exchangeNode.Id,
                MessageHeader = obj.MessageHeader,
                Message = obj.Message,
                DateStamp = DateTime.UtcNow,
                SenderDateStamp = obj.SenderDateStamp,
                ServerDateStamp = obj.DateStamp,
                LiveLevel = 0,
                State = "Downloaded from ExchangeServer"
            };

            await DataAdapter.RegisterUnresolvedMessageAsync(exchangeNode.Id, unresolvedObject);
            await ExchangeApiAdapter.DeleteObjectAsync(obj.Id);
        }
    }
    /// <summary>
    /// Проаналізувати завантажені дані та визначити, чи можна їх обробити, чи потрібно залишити їх як невирішені повідомлення для подальшого аналізу.
    /// </summary>
    /// <param name="exchangeNode">Вузол обміну даними, з якого будуть аналізовані дані.</param>
    /// <returns></returns>
    async Task AnalizeAsync(ExchangeNode exchangeNode)
    {
        var data = await DataAdapter.GetUnresolvedMessagesAsync(exchangeNode.Id, exchangeNode.TakeUpload);
        foreach (var message in data) 
        {

            var analizeResult = await ExchangeAnalizer.MessageAnalizeAsync(exchangeNode, message.MessageHeader, message.Message);
            if (analizeResult.Success)
            {
                await DataAdapter.RemoveUnresolvedMessageAsync(message.Id);
            }
            else
            {
                await DataAdapter.RegisterUnresolvedMessageStatusAsync(exchangeNode.Id, message.Id, analizeResult.ErrorMessage ?? "Відсутня інформація про помилку!");
            }
   
        }
    }
    /// <summary>
    /// Завантажити оброблені дані з бази даних та надіслати їх на вузол обміну даними.
    /// </summary>
    /// <param name="exchangeNode">Вузол обміну даними, на який будуть надіслані дані.</param>
    /// <returns></returns>
    async Task UploadAsync(ExchangeNode exchangeNode)
    {
        var data = await DataAdapter.GetUploadMessagesAsync(exchangeNode.Id, exchangeNode.TakeUpload);
        foreach (var obj in data)
        {
            var messageHeader = obj.MessageHeader;
            var message = obj.Message;
            await ExchangeApiAdapter.PostObjectAsync(exchangeNode.Id, messageHeader, message, DateTime.UtcNow);
            await DataAdapter.RemoveUploadMessageAsync(obj.Id);
        }
    }

    /// <summary>
    /// Запустити сеанс обміну даними між вузлом і базою даних.
    /// </summary>
    /// <param name="exchangeNode">Вузол обміну даними, з яким буде виконано сеанс обміну даними.</param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public async Task ExchangeNode(ExchangeNode exchangeNode)
    {
         
        await DownloadAsync(exchangeNode);
        await AnalizeAsync(exchangeNode);
        await UploadAsync(exchangeNode);
    }
    /// <summary>
    /// Запустити сеанс обміну даними між усіма вузлами та базою даних.
    /// </summary>
    /// <param name="cancellationToken">Токен скасування для контролю завершення операції.</param>
    /// <param name="asTasks">Вказує, чи виконувати обмін даними асинхронно для кожного вузла.</param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>

    public async Task Exchange(CancellationToken cancellationToken, bool asTasks = false)
    {
        try
        {
            await ExchangeApiAdapter.RefreshTokenAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("Error refreshing token", ex);
        }
        var nodes = await DataAdapter.GetExchangeNodesAsync(true);
        if (asTasks)
        {
            var tasks = new List<Task>();
            foreach (var node in nodes)
            {
                tasks.Add(ExchangeNode(node));
            }
            await Task.WhenAll(tasks);
        }
        else 
        {
            foreach (var node in nodes) await ExchangeNode(node);
        }
     }


}
