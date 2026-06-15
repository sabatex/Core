// (c)Serhiy Lakas(https://sabatex.github.io)
namespace Sabatex.ObjectsExchange.HttpClientApiConnector;

using System;
using Sabatex.ObjectsExchange.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Net.Http;
using System.Net.Http.Json;
using System.Net.Http.Headers;
using System.Threading.Tasks;

public delegate Task<string> PasswordGetterDelegateAsync();
public delegate Task<string> RefreshTokenGetterDelegateAsync();
public delegate Task UpdateTokenDelegateAsync(Token token,DateTime expiredToken);



public  class ExchangeApiConnector : IDisposable
{
    protected const string loginUrl = "api/v1/login";
    protected const string refreshTokenUrl = "api/v1/refresh_token";
    protected const string objectsUrl = "api/v1/objects";
    //protected const string queryUrl = "api/v1/queries";


    private Token? _accessToken;
    
    private readonly HttpClient _httpClient;
    private readonly ClentNodeConfig _clientNodeConfig;
    private DateTime _expired_token;
    private readonly PasswordGetterDelegateAsync passwordGetterAsync;
    private readonly UpdateTokenDelegateAsync tokenUpdateAsync;
    private readonly RefreshTokenGetterDelegateAsync refreshTokenGetter;
    protected AuthenticationHeaderValue _authenticationHeaderValue => new(_accessToken.TokenType,_accessToken.AccessToken);

    #region constructors
    /// <summary>
    /// 
    /// </summary>
    /// <param name="settings"></param>
    /// <param name="acceptFailCertificates"></param>
    /// <param name="passwordGetter"></param>
    /// <param name="tokenUpdate"></param>
    public ExchangeApiConnector(ClentNodeConfig clientNodeConfig, Token accessToken,string destinationId,  PasswordGetterDelegateAsync passwordGetter,RefreshTokenGetterDelegateAsync refreshTokenGetter, UpdateTokenDelegateAsync tokenUpdate)
    {
        _clientNodeConfig = clientNodeConfig;

        this.passwordGetterAsync = passwordGetter;
        this.tokenUpdateAsync = tokenUpdate;
        this.refreshTokenGetter = refreshTokenGetter;
        _accessToken = accessToken;
        if (clientNodeConfig.AcceptFailCertificates )
        {

            var httpClientHandler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; }
            };
            _httpClient = new HttpClient(httpClientHandler);
        }
        else
        {
            _httpClient = new HttpClient();
        }
        
        _httpClient.BaseAddress = new Uri(clientNodeConfig.BaseUri);
        _httpClient.DefaultRequestHeaders.Add("apiToken", _accessToken.AccessToken);
        _httpClient.DefaultRequestHeaders.Add("clientId", clientNodeConfig.ClientId);
        _httpClient.DefaultRequestHeaders.Add("destinationId", destinationId);
   }

#endregion

    #region Autorization

    private async Task UpdateAccessTokenAsync(Token? token)
    {
        if (token == null)
            throw new ArgumentNullException(nameof(token));
        
        _accessToken = token;
        _expired_token = DateTime.UtcNow + TimeSpan.FromSeconds(token.ExpiresIn);

        if (tokenUpdateAsync != null)
            await tokenUpdateAsync.Invoke(token,_expired_token);
        _httpClient.DefaultRequestHeaders.Remove("apiToken");
        _httpClient.DefaultRequestHeaders.Add("apiToken",_accessToken.AccessToken);
    }

    private async Task AutorizeAsync()
    {

        var login = new Login
        {
            ClientId =  new Guid(_clientNodeConfig.ClientId),
            Password = await passwordGetterAsync.Invoke()
        };
        var response = await _httpClient.PostAsJsonAsync("api/v0/login", login) ?? throw new Exception();
        if (response.IsSuccessStatusCode)
        {
            var token = await response.Content.ReadFromJsonAsync<Token>() ?? throw new Exception();
            await UpdateAccessTokenAsync(token);
            return;
        }
       throw new Exception();
    }
    private async Task<bool> RefreshTokenAsync()
    {
        if (refreshTokenGetter == null)
            return false;
        var refreshToken = await refreshTokenGetter.Invoke();
        if (refreshToken == null) return false;
        var response = await _httpClient.PostAsJsonAsync(refreshTokenUrl, new
        Login
        {
            ClientId = new Guid(_clientNodeConfig.ClientId),
            Password = refreshToken
        });
        if (response.IsSuccessStatusCode)
        {
            var rt = await response.Content.ReadFromJsonAsync<Token>();
            if (rt == null) return false;
            await UpdateAccessTokenAsync(rt);
            return true;
        }
        return false;
    }

    /// <summary>
    /// speed check valid token and update invalid
    /// </summary>
    /// <returns></returns>
    private async Task CheckAutorizedAsync()
    {
        if (_accessToken != null)
        {
            if (_expired_token > DateTime.UtcNow)
            {
                return; // token is valid
            }
            else
            {
                if (await RefreshTokenAsync())
                    return; // update access token
            }
        }
        await AutorizeAsync(); // login with password
    }

    #endregion

    #region IDisposable
    public void Dispose()
    {
        _httpClient?.Dispose();
    }
#endregion

    /// <summary>
    /// renev access_token by refresh token or login by password
    /// </summary>
    /// <returns></returns>
    private async Task<HttpResponseMessage> PostAsync<T>(string? uriString, T value)
    {
        await CheckAutorizedAsync(); // exception if unautorized
        var response = await _httpClient.PostAsJsonAsync(uriString, value);
        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            await AutorizeAsync();
            response = await _httpClient.PostAsJsonAsync(uriString, value);
        }
        if (response.IsSuccessStatusCode) return response;
        throw new Exception($"The error post object:{value}");
    }

    private async Task<HttpResponseMessage> GetAsync(string? uriString, int take = 10)
    {
        await CheckAutorizedAsync(); // exception if unautorized
        var response = await _httpClient.GetAsync($"{uriString}?take={take}");
        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            await AutorizeAsync();
            response = await _httpClient.GetAsync(uriString);
        }
        if (response.IsSuccessStatusCode) return response;
        throw new Exception($"The error get {uriString}");
    }

    private async Task DeleteAsync(string uriString, long id)
    {
        await CheckAutorizedAsync(); // exception if unautorized
        var response = await _httpClient.DeleteAsync($"{uriString}/{id}");
        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            await AutorizeAsync();
            response = await _httpClient.DeleteAsync($"{uriString}/{id}"); 
        }
        if (response.IsSuccessStatusCode)
            return;
        throw new Exception();

    }

    #region Objects API
    /// <summary>
    /// 
    /// </summary>
    /// <param name="take"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public async Task<IEnumerable<ObjectExchange>> GetObjectsAsync(int take=10)
    {
        var responce = await GetAsync(objectsUrl,take);
        if (responce != null)
        {
            if (responce.IsSuccessStatusCode)
            {
                var result = await responce.Content.ReadFromJsonAsync<IEnumerable<ObjectExchange>>();
                if (result != null)
                    return result;
            }
        }
        throw new Exception();
        
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="objectType"></param>
    /// <param name="objectId"></param>
    /// <param name="text"></param>
    /// <returns></returns>
    public async Task PostObjectAsync(string objectType, string objectId, string text) =>
    await PostAsync(objectsUrl,
                                         new {
                                             objectType,
                                             objectId,
                                             text,
                                             dateStamp = DateTime.UtcNow
                                         });
    public async Task DeleteObjectAsync(long id)=>await DeleteAsync(objectsUrl, id);
    #endregion

//    #region Query
//    public async Task PostQueryObjectAsync(string objectType, string objectId) =>
//        await PostAsync(queryUrl, new { objectType, objectId });

//    public async Task<IEnumerable<QueryObject>?> GetQueryObjectsAsync(int take)
//    {
//        var responce = await GetAsync(queryUrl, take);
//        if (responce != null)
//        {
//            if (responce.IsSuccessStatusCode)
//            {
//                var result = await responce.Content.ReadFromJsonAsync<IEnumerable<QueryObject>>();
//                if (result != null)
//                    return result;
//            }
//        }
//        throw new Exception();

//    }

//    public async Task DeleteQueryObjectAsync(long id) => await DeleteAsync(queryUrl, id);

//#endregion












}
