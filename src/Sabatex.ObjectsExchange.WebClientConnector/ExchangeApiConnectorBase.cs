using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sabatex.ObjectsExchange.ApiConnector.Common
{
    public abstract class ExchangeApiConnectorBase
    {
        protected const string loginUrl = "api/v0/login";
        protected const string refreshTokenUrl = "api/v0/refresh_token";
        protected const string objectsUrl= "api/v0/objects";
        protected const string queryUrl = "api/v0/queries";
        protected readonly Guid clientId;
        protected readonly Guid destinationId;
        protected readonly string BaseUri;
        
        protected string? accessToken;
        protected DateTime expired_token;


        protected ExchangeApiConnectorBase(string baseUri, Guid clientId, Guid destinationId, string accessToken, DateTime expired_token)
        {
            if (baseUri == null || baseUri == string.Empty)
                this.BaseUri = "https://sabatex.francecentral.cloudapp.azure.com/";
            else
                this.BaseUri = baseUri;

            this.clientId = clientId;
            this.destinationId = destinationId;

            if (expired_token < DateTime.UtcNow)
            {
                this.accessToken = accessToken;
                this.expired_token = expired_token;
            }
        }

    }
}
