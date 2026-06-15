using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Sabatex.ObjectsExchange.ApiConnector
{
    public class ExchangeApiSettings
    {
        /// <summary>
        /// Base api uri https://host{:port}
        /// </summary>
        public string BaseUri { get; set; } = "https://sabatex.francecentral.cloudapp.azure.com/";
        public string AccessToken { get; set; } = string.Empty;
        public string ClientId { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public string Password { get; set; }= string.Empty;
        public bool AcceptFailCertificates { get; set; }
    }
}
