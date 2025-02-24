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

public interface IApiAdapter
{
    Task DownloadObjectsAsync(ExchangeNode exchangeNode);

}
public class ApiAdapter
{
    readonly HttpClient _httpClient;
    readonly ExchangeDbContext  _dbContext;
    public ApiAdapter(HttpClient httpClient,ExchangeDbContext dbContext)
    {
        _httpClient = httpClient;
        _dbContext = dbContext;
    }

    async Task<bool> UpdateAccess()
    {
        return false;
    }


    public async Task DownloadObjectsAsync(ExchangeNode exchangeNode,bool first=true)
    {
        try
        {
            var responce = await _httpClient.GetAsync($"objects?DestinationId={exchangeNode.DestinationId}&take={exchangeNode.Take}");
            if (responce == null)
            {
                throw new NullReferenceException($"The responce is null result!");
            }
            if (responce.StatusCode == HttpStatusCode.Unauthorized)
            {
                if (!first)
                {
                    throw new AccessDeniedException(exchangeNode);
                }


                if (!await UpdateAccess())
                {
                    throw new Exception("Access denied!");
                }
                await DownloadObjectsAsync(exchangeNode, false);
                return;
            }

            var objects = await responce.Content.ReadFromJsonAsync<ObjectExchange[]>();
            if (objects == null)
                throw new Exception();
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    foreach (var obj in objects)
                    {
                        var data = (new UnresolvedObject()).FillAttributes(obj);
                        await _dbContext.AddAsync(data);
                        await _dbContext.SaveChangesAsync();
                    }
                    responce = await _httpClient.PostAsJsonAsync($"objects_delete", new { ids = objects.Select(s => s.Id).ToArray() });
                    if (responce == null)
                        throw new Exception();
                    if (!responce.IsSuccessStatusCode)
                        throw new Exception();
                    transaction.Commit();
                }
                catch (Exception ex) 
                {
                    transaction.Rollback();
                }
            }
        }
        catch (Exception ex)
        {

        }

        using (var transaction = _dbContext.Database.BeginTransaction())
        {

        }
    }

}
