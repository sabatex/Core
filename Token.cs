namespace Sabatex.ObjectsExchange.Models;

using System;
using System.Text.Json;
using System.Text;
using System.Text.Json.Serialization;

/// <summary>
/// The security token
/// </summary>
public class Token
{
    /// <summary>
    /// Access token string 
    /// </summary>
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; } = string.Empty;

    /// <summary>
    /// Token type, defaut Bearer
    /// </summary>
    [JsonPropertyName("token_type")]
    public string TokenType { get; set; } = "Bearer";

    /// <summary>
    /// Refresh token, used for refresh token after expired
    /// </summary>
    [JsonPropertyName("refresh_token")]
    public string RefreshToken { get; set; }= string.Empty;

    /// <summary>
    /// Expired time by seconds
    /// </summary>
    [JsonPropertyName("expires_in")]
    public int ExpiresIn { get; set; }

    /// <summary>
    /// Connstructor
    /// </summary>
    /// <param name="accessToken">Access token string</param>
    /// <param name="refreshToken">Refresh token, used for refresh token after expired</param>
    /// <param name="expiresIn">Expired time by seconds</param>
    /// <param name="clientId">Client Id</param>
    public Token(string accessToken, string refreshToken, int expiresIn/*,Guid? clientId*/)
    {
        //if (clientId != null)
        //{
        //    AccessToken = Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(new { ClientId = clientId, AccessToken = accessToken })));
        //    RefreshToken = Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(new { ClientId = clientId, AccessToken = refreshToken })));
        //}
        //else
        //{
            AccessToken = accessToken;
            RefreshToken = refreshToken;
        //}
        TokenType = "Bearer";
        ExpiresIn = expiresIn;
    }
    public Token()
    {
        
    }

}
