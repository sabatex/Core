using Sabatex.Extensions.ClassExtensions;
using Sabatex.ObjectsExchange.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

namespace Sabatex.ObjectsExchange.ApiAdapter;

public class ExchangeApiAdapter
{
    readonly IExchangeApiAdapter _exchangeApiAdapter;
    readonly IClientExchangeDataAdapter  _dataAdapter;
    public ExchangeApiAdapter(IExchangeApiAdapter exchangeApiAdapter, IClientExchangeDataAdapter dataAdapter)
    {
        _exchangeApiAdapter = exchangeApiAdapter;
        _dataAdapter= dataAdapter;
    }



    async Task DownloadAsync(ExchangeNode exchangeNode)
    {
      var objects = await _exchangeApiAdapter.GetObjectsAsync(exchangeNode.DestinationId, exchangeNode.Take);
        foreach (var obj in objects)
        {
            await _dataAdapter.RegisterUnresolvedMessageAsync(exchangeNode.DestinationId, (new UnresolvedObject()).FillAttributes(obj));
            await _exchangeApiAdapter.DeleteObjectAsync(obj.Id);
        }
    }

    async Task AnalizeAsync(ExchangeNode exchangeNode)
    {
        await Task.Yield();
    }

    async Task UploadAsync(ExchangeNode exchangeNode)
    {
        await Task.Yield();
    }


    public async Task Exchange(ExchangeNode exchangeNode)
    {
        try
        {
            await _exchangeApiAdapter.RefreshTokenAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("Error refreshing token", ex);
        }
        
        await DownloadAsync(exchangeNode);
        await AnalizeAsync(exchangeNode);
        await UploadAsync(exchangeNode);
    }


}
