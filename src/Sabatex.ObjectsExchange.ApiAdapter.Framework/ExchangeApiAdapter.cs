using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ObjectsExchange.ApiAdapter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Sabatex.ObjectsExchange.ApiAdapter
{
    public class ExchangeApiAdapter
    {
        public  Token Login(Uri host, Guid clientId, string password)
        {
            var login = new { clientId = clientId, password = password };
            var url = new Uri(host, "api/v2/login");
            var webClient = new WebClient();
            webClient.Headers.Add("Content-Type", "application/json; charset=utf-8");
            webClient.Headers.Add("accept", "*/*");
            var s = JsonConvert.SerializeObject(login);
            try
            {
                var token = webClient.UploadString(url, "POST", s);
                return JsonConvert.DeserializeObject<Token>(token);
            }
            catch (Exception ex)
            {
                throw new Exception($"Login failed: {ex.Message}");
            }
        }

        public Token RefreshToken(Uri host, string refreshToken)
        {
            var url = new Uri(host, "api/v2/logout");
            var webClient = new WebClient();
            webClient.Headers.Add("Content-Type", "application/json; charset=utf-8");
            webClient.Headers.Add("accept", "*/*");
            var s = JsonConvert.SerializeObject(new { refresh_token=refreshToken });
            try
            {
                var token = webClient.UploadString(url, "POST", s);
                return JsonConvert.DeserializeObject<Token>(token);
            }
            catch (Exception ex)
            {
                throw new Exception($"Refresh token failed: {ex.Message}");
            }
        }


        public string GetToken(SessionContext sessionContext)
        {
            Token token = null;
            if (sessionContext.AccessToken != string.Empty)
            {
                try
                {
                    token = RefreshToken(sessionContext.Host, sessionContext.RefreshToken);
                    sessionContext.AccessToken = token.access_token;
                    sessionContext.RefreshToken = token.refresh_token;
                    return token.access_token;
                }
                catch (Exception ex)
                {

                } 
            }


            token = Login(sessionContext.Host, sessionContext.ClientId, sessionContext.Password);
            sessionContext.AccessToken = token.access_token;
            sessionContext.RefreshToken = token.refresh_token;
            return token.access_token;
        }


        public ObjectExchange[] GetObjects(SessionContext sessionContext,Guid destinationId)
        {
            var url = new Uri(sessionContext.Host, $"api/v2/objects?take={sessionContext.Take}");
            var webClient = new WebClient();
            webClient.Encoding = Encoding.UTF8;
            webClient.Headers.Add("Content-Type", "application/json; charset=utf-8");
            webClient.Headers.Add("accept", "*/*");
            webClient.Headers.Add("authorization", sessionContext.AccessToken);
            webClient.Headers.Add("destinationId", destinationId.ToString());
            try
            {
                string s = webClient.DownloadString(url);
                 return JsonConvert.DeserializeObject<ObjectExchange[]>(s);
            }
            catch (Exception ex)
            {
                throw new Exception($"GET objects to service : {ex.ToString()}");
            }
        }

        public void PostObject(SessionContext sessionContext, Guid destinationId, string messageHeader, string message)
        {
            var url = new Uri(sessionContext.Host, "api/v2/objects");
            var webClient = new WebClient();
            webClient.Encoding = Encoding.UTF8;
            webClient.Headers.Add("Content-Type", "application/json; charset=utf-8");
            webClient.Headers.Add("accept", "*/*");
            webClient.Headers.Add("authorization", sessionContext.AccessToken);
            webClient.Headers.Add("destinationId", destinationId.ToString());
            try
            {
                var obj = new { messageHeader = messageHeader, message = message, dateStamp = DateTime.UtcNow };
                string t = JsonConvert.SerializeObject(obj);
                webClient.UploadString(url, "POST", t);
            }
            catch (Exception ex)
            {
                throw new Exception($"POST object to service : {ex.ToString()}");
            }
        }

        public void DeleteObject(SessionContext sessionContext, long id)
        {
            var url = new Uri(sessionContext.Host, $"api/v2/objects/{id}");
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "DELETE";
            request.Accept = "*/*";
            request.Headers.Add("authorization", sessionContext.AccessToken);

            try
            {
                var responce = request.GetResponse();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error delete object from service : {ex.ToString()}");
            }

        }
    }

}
