// (c)Serhiy Lakas(https://sabatex.github.io)
namespace Sabatex.ObjectsExchange.ApiConnector
{
    using System;
    using Sabatex.ObjectsExchange.Models;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Text;
    using Newtonsoft.Json.Linq;
    using Newtonsoft.Json;
    using Sabatex.ObjectsExchange.ApiConnector.Common;
    using System.IO;
    using System.Net.NetworkInformation;



    /// <summary>
    /// Delegate for user password getter 
    /// </summary>
    /// <returns></returns>
    public delegate string PasswordGetterDelegate();
    /// Delegate for user password getter 
    /// </summary>
    /// <returns></returns>
    public delegate string RefreshGetterDelegate();

    /// <summary>
    /// Token update call back
    /// </summary>
    /// <returns></returns>
    public delegate void UpdateTokenDelegate(Token token, DateTime expiredToken);

    public partial class ExchangeApiConnector:ExchangeApiConnectorBase
    {
 
        private readonly PasswordGetterDelegate passwordGetter;
        private readonly UpdateTokenDelegate tokenUpdate;
        private readonly RefreshGetterDelegate refreshGetter;

#region constructors
        /// <summary>
        /// 
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="acceptFailCertificates"></param>
        /// <param name="passwordGetter"></param>
        /// <param name="tokenUpdate"></param>
        public ExchangeApiConnector(string baseUri, Guid clientId, Guid destinationId, string accessToken, DateTime expired_token, bool acceptFailCertificates, PasswordGetterDelegate passwordGetter, RefreshGetterDelegate refreshTokenGetter,UpdateTokenDelegate tokenUpdate) : base(baseUri,clientId,destinationId,accessToken,expired_token)
        {
            this.passwordGetter = passwordGetter;
            this.tokenUpdate = tokenUpdate;
            this.refreshGetter = refreshTokenGetter;

        }

        #endregion

        #region Autorization
        [Obsolete]
        private void UpdateAccessToken(Token? token)
        {
            if (token == null)
                throw new ArgumentNullException(nameof(token));
            expired_token = DateTime.UtcNow + TimeSpan.FromSeconds(token.ExpiresIn);
            if (tokenUpdate != null)
                tokenUpdate.Invoke(token,expired_token);
            accessToken = token.AccessToken;
         }

        [Obsolete]
        private void Autorize()
        {
            var login = new Login
            {
                ClientId = clientId,
                Password = passwordGetter?.Invoke() ?? string.Empty,
            };
            var url = new Uri(new Uri(BaseUri), loginUrl);
            var webClient = new WebClient();
            webClient.Headers.Add("Content-Type", "application/json; charset=utf-8");
            webClient.Headers.Add("accept", "*/*");
            var s = JsonConvert.SerializeObject(login);
            try
            {
                var result = webClient.UploadString(url, "POST", s);
                var token = JsonConvert.DeserializeObject<Token>(result);
                UpdateAccessToken(token);

            }
            catch (Exception ex)
            {
                throw new Exception($"Login failed: {ex.Message}");
            }
        }
        [Obsolete]
        private bool RefreshToken()
        {
            if (refreshGetter == null) return false;
            var refreshToken = refreshGetter.Invoke();
            if (refreshToken == null) return false;
            var login = new Login
            {
                ClientId = clientId,
                Password = refreshToken,
            };
            var url = new Uri(new Uri(BaseUri), refreshTokenUrl);
            var webClient = new WebClient();
            webClient.Headers.Add("Content-Type", "application/json; charset=utf-8");
            webClient.Headers.Add("accept", "*/*");
            var s = JsonConvert.SerializeObject(login);
            try
            {
                var result = webClient.UploadString(url, "POST", s);
                var token = JsonConvert.DeserializeObject<Token>(result);
                UpdateAccessToken(token);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// speed check valid token and update invalid
        /// </summary>
        /// <returns></returns>
        [Obsolete]
        private void CheckAutorized()
        {
            if (accessToken != null)
            {
                if (expired_token > DateTime.UtcNow)
                {
                    return; // token is valid
                }
                else
                {
                    if (RefreshToken()) return; // update access token
                }
            }
            Autorize(); // login with password
        }

        #endregion
        private WebClient GetWebClientForJSONContent()
        {
            var webClient = new WebClient();
            webClient.Encoding = Encoding.UTF8;
            webClient.Headers.Add("Content-Type", "application/json; charset=utf-8");
            webClient.Headers.Add("accept", "*/*");
            webClient.Headers.Add("clientId", clientId.ToString());
            webClient.Headers.Add("apiToken", accessToken);
            return webClient;
        }

        /// <summary>
        /// renev access_token by refresh token or login by password
        /// </summary>
        /// <returns></returns>
        [Obsolete]
        private void Post<T>(string? uriString, T value)
        {
            string postData = JsonConvert.SerializeObject(value);
            CheckAutorized(); // exception if unautorized
            var url = new Uri(new Uri(BaseUri), uriString);
            var webClient = new WebClient();
            webClient.Encoding = Encoding.UTF8;
            webClient.Headers.Add("Content-Type", "application/json; charset=utf-8");
            webClient.Headers.Add("accept", "*/*");
            webClient.Headers.Add("clientId", clientId.ToString());
            webClient.Headers.Add("destinationId", destinationId.ToString());
            webClient.Headers.Add("apiToken", accessToken);
            var response = webClient.UploadString(url, "POST", postData);
            

        }



        #region Objects API
        [Obsolete]
        public void PostObject(string objectType, string objectId, string text)
        {
            Post(objectsUrl,
                                     new
                                     {
                                         objectType = objectType,
                                         objectId = objectId,
                                         text = text,
                                         dateStamp = DateTime.UtcNow
                                     });

        }


        #endregion












    }
}
