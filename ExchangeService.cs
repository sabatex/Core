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



    async Task DownloadAsync(ExchangeNode exchangeNode)
    {
      var objects = await ExchangeApiAdapter.GetObjectsAsync(exchangeNode.DestinationId, exchangeNode.TakeDownload);
        foreach (var obj in objects)
        {
            var unresolvedObject = new UnresolvedObject
            {
                NodeId = exchangeNode.DestinationId,
                MessageHeader = obj.MessageHeader,
                Message = obj.Message,
                DateStamp = DateTime.UtcNow,
                SenderDateStamp = obj.SenderDateStamp,
                ServerDateStamp = obj.DateStamp,
                LiveLevel = 0,
                State = "Downloaded from ExchangeServer"
            };

            await DataAdapter.RegisterUnresolvedMessageAsync(exchangeNode.DestinationId, unresolvedObject);
            await ExchangeApiAdapter.DeleteObjectAsync(obj.Id);
        }
    }

 

    async Task AnalizeAsync(ExchangeNode exchangeNode)
    {
        var data = await DataAdapter.GetUnresolvedMessagesAsync(exchangeNode.DestinationId, exchangeNode.TakeUpload);
        foreach (var message in data) 
        {

            var analizeResult = await ExchangeAnalizer.MessageAnalizeAsync(exchangeNode, message.MessageHeader, message.Message);
            if (analizeResult.Success)
            {
                await DataAdapter.RemoveUnresolvedMessageAsync(exchangeNode.DestinationId, message.Id);
            }
            else
            {
                await DataAdapter.RegisterUnresolvedMessageStatusAsync(exchangeNode.DestinationId, message.Id, analizeResult.ErrorMessage ?? "Відсутня інформація про помилку!");
            }
   
        }
    }

    async Task UploadAsync(ExchangeNode exchangeNode)
    {
        var data = await DataAdapter.GetUploadMessagesAsync(exchangeNode.DestinationId, exchangeNode.TakeUpload);
        foreach (var obj in data)
        {
            var messageHeader = obj.MessageHeader;
            var message = obj.Message;
            await ExchangeApiAdapter.PostObjectAsync(exchangeNode.DestinationId, messageHeader, message, DateTime.UtcNow);
            await DataAdapter.RemoveUploadMessageAsync(exchangeNode.DestinationId, obj.Id);
        }
    }

    /// <summary>
    /// Запустити сеанс обміну даними між вузлом і базою даних.
    /// </summary>
    /// <param name="exchangeNode"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public async Task ExchangeNode(ExchangeNode exchangeNode)
    {
         
        await DownloadAsync(exchangeNode);
        await AnalizeAsync(exchangeNode);
        await UploadAsync(exchangeNode);
    }


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
        var nodes = await DataAdapter.GetExchangeNodesAsync();
        foreach (var node in nodes)
        {
            if (asTasks)
            {
                Task.Run(() => ExchangeNode(node));
            }
            else
            {
                await ExchangeNode(node);
            }
        }
    }


}
