using Microsoft.Extensions.Options;
using Sabatex.ObjectsExchange.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Metadata;
using Sabatex.Extensions.ClassExtensions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Sabatex.ObjectsExchange.ApiAdapter;

public class ExchangeApiAdapterV2 : IExchangeApiAdapter
{
    private volatile bool _disposed;
    private string? _refreshToken;
    //private readonly Guid _clientId;
    private readonly HttpClient _httpClient;
    private readonly SabatexExchangeOptions _options;
    private readonly IClientExchangeDataAdapter _dataAdapter;

    public Guid ClientId => _options.ClientId;


    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="apiConfig"></param>
    /// <exception cref="ArgumentException"></exception>
    public ExchangeApiAdapterV2(IOptions<SabatexExchangeOptions> apiConfig, IClientExchangeDataAdapter dataAdapter) : this(apiConfig, dataAdapter, CreateDefaultHttpClient(apiConfig))
    {
        //_dataAdapter = dataAdapter;
        //_clientId = apiConfig.Value.ClientId;
        //if (_clientId == Guid.Empty)
        //    throw new ArgumentException("Client Id is empty");
        //if (string.IsNullOrEmpty(apiConfig.Value.ClientSecret))
        //    throw new ArgumentException("Client Secret is empty");
        //_clientSecret = apiConfig.Value.ClientSecret;
        //if (string.IsNullOrEmpty(apiConfig.Value.ApiUrl))
        //    throw new ArgumentException("Api Url is empty");
        //_httpClient = new HttpClient()
        //{
        //    BaseAddress = new Uri(apiConfig.Value.ApiUrl)
        //};
        //_httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        //_httpClient.DefaultRequestHeaders.Add("Accept-Charset", "utf-8");
        //_httpClient.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate");

    }

    internal ExchangeApiAdapterV2(
            IOptions<SabatexExchangeOptions> apiConfig,
            IClientExchangeDataAdapter dataAdapter,
            HttpClient httpClient)
    {
        _options = apiConfig?.Value ?? throw new ArgumentNullException(nameof(apiConfig));
        _dataAdapter = dataAdapter ?? throw new ArgumentNullException(nameof(dataAdapter));
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    private static HttpClient CreateDefaultHttpClient(IOptions<SabatexExchangeOptions> apiConfig)
    {
        // Створюємо клієнт за допомогою налаштувань, що надані в опціях.
        return new HttpClient
        {   
            BaseAddress = new Uri(apiConfig.Value.ApiUrl),
            DefaultRequestHeaders =
            {
                { "Accept", "application/json" },
                { "Accept-Charset", "utf-8" },
                { "Accept-Encoding", "gzip, deflate" }
            }
        };
    }


    public async Task<bool> RefreshTokenAsync()
    {
        HttpResponseMessage? responce = null;
        if ( !string.IsNullOrWhiteSpace(_refreshToken))
        {
            var token = new { refresh_token = _refreshToken };
            responce = await _httpClient.PutAsJsonAsync("/api/v2/refresh_token", token);
            if (responce.IsSuccessStatusCode)
            {
                var result = await responce.Content.ReadFromJsonAsync<Token>();
                if (result == null)
                    throw new Exception("Помилка десереалізації");
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", result.AccessToken);
                _refreshToken = result.RefreshToken;
                return true;
            }
        }

        // try login
        var login = new { ClientId = _options.ClientId, Password = _options.ClientSecret };
        responce = await _httpClient.PostAsJsonAsync("/api/v2/login", login);
        if (responce.IsSuccessStatusCode)
        {
            var result = await responce.Content.ReadFromJsonAsync<Token>();
            if (result == null)
                throw new Exception("Помилка десереалізації");
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", result.AccessToken);
            _refreshToken = result.RefreshToken;
            return true;
        }
        return false;
    }

    public async Task<IEnumerable<ObjectExchange>> GetObjectsAsync(Guid destinationNode, int take = 10)
    {
        var responce = await _httpClient.GetAsync($"api/v2/objects?DestinationId={destinationNode}&take={take}");
        if (responce == null)
        {
            throw new NullReferenceException($"The responce is null result!");
        }
        if (responce.IsSuccessStatusCode)
        {
            var objects = await responce.Content.ReadFromJsonAsync<ObjectExchange[]>();
            if (objects == null)
                throw new Exception("Помилка десереалізації");
            return objects;
        }
        
        if (responce.StatusCode == HttpStatusCode.Unauthorized)
        {
           throw new Exception("Access denied");
        }
        throw new Exception("Error ");
    }


    public async Task PostObjectAsync(Guid destinationId,string messageHeader,string message, DateTime? dateStamp)
    {
        if (dateStamp == null)
            dateStamp = DateTime.UtcNow;

        var responce = await _httpClient.PostAsJsonAsync("api/v2/objects", new { destinationId = destinationId, messageHeader =messageHeader,message = message, dateStamp =dateStamp});
        if (responce == null)
        {
            throw new NullReferenceException($"The responce is null result!");
        }
        if (responce.IsSuccessStatusCode)
        {
            return;
        }
        if (responce.StatusCode == HttpStatusCode.Unauthorized)
        {
            throw new Exception("Access denied");
        }
        throw new Exception("Error ");
    }

    public async Task DeleteObjectAsync(long objectId)
    {
        var responce = await _httpClient.DeleteAsync($"api/v2/objects/{objectId}");
        if (responce == null)
        {
            throw new NullReferenceException($"The responce is null result!");
        }
        if (responce.IsSuccessStatusCode)
        {
            return;
        }
        if (responce.StatusCode == HttpStatusCode.Unauthorized)
        {
            throw new Exception("Access denied");
        }
        throw new Exception("Error ");
    }


    public async Task DeleteRange(long[] ids)
    {
        var responce = await _httpClient.PostAsJsonAsync("api/v2/objects_delete_range",new {ids=ids});
        if (responce == null)
        {
            throw new NullReferenceException($"The responce is null result!");
        }
        if (responce.IsSuccessStatusCode)
        {
            return;
        }
        if (responce.StatusCode == HttpStatusCode.Unauthorized)
        {
            throw new Exception("Access denied");
        }
        throw new Exception("Error ");

    }


    async Task DownloadAsync(ExchangeNode exchangeNode)
    {
        var objects = await GetObjectsAsync(exchangeNode.DestinationId, exchangeNode.Take);
        foreach (var obj in objects)
        {
            await _dataAdapter.RegisterUnresolvedMessageAsync(exchangeNode.DestinationId, (new UnresolvedObject()).FillAttributes(obj));
            await DeleteObjectAsync(obj.Id);
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






    public void Dispose()
    {
        Dispose(true);

    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing && !_disposed)
        {
            _disposed = true;
            _httpClient.Dispose();
        }
    }

    public async Task<Guid> RegisterMessageAsync(Guid destination, string messageHeader, string message)
    {
        return await _dataAdapter.RegisterMessageAsync(destination,messageHeader,message);
    }

    public async Task<IEnumerable<UploadObject>> GetUploadObjectsAsync(Guid destinationNode, int take = 10)
    {
        return await _dataAdapter.GetUploadObjectsAsync(destinationNode,take);
    }

    public async Task RemoveUploadMessageAsync(Guid destination, Guid id)
    {
        await _dataAdapter.RemoveUploadMessageAsync(destination,id);
    }

  
}
