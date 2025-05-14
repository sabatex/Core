using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Localization;
using Moq;
using ObjectsExchange;
using Sabatex.ObjectsExchange.ApiAdapter;
using Sabatex.ObjectsExchange.BASAdapter;
using Sabatex.ObjectsExchange.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Sabatex.ObjectsExchange.Tests
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program> 
    {
        public ExchangeService NodeA { get;private set; }
        public ExchangeService NodeB { get; private set; }
        public ExchangeService NodeC { get; private set; }


        public bool IsInitialized { get; set; } = false;

        public CustomWebApplicationFactory()
        {
            var mockLocalizer = new Mock<IStringLocalizer<MessageAnalizer>>();

            var apiAdapter = new ExchangeApiAdapterV2(TestData.NodeAOptions, CreateClient());
            
            var dataAdapter = new Sabatex.ObjectExchange.ClientDataAdapter.Memory.DataStorage();
            dataAdapter.Initial(TestData.GetNodeAExchangeNodes());

            var analizer = new MessageAnalizer(mockLocalizer.Object);

            NodeA = new ExchangeService(apiAdapter, dataAdapter,analizer);
            



            apiAdapter = new ExchangeApiAdapterV2(TestData.NodeBOptions, CreateClient());
            
            dataAdapter = new Sabatex.ObjectExchange.ClientDataAdapter.Memory.DataStorage();
            dataAdapter.Initial(TestData.GetNodeAExchangeNodes());
            NodeB = new ExchangeService(apiAdapter, dataAdapter, analizer);
            
            apiAdapter = new ExchangeApiAdapterV2(TestData.NodeCOptions, CreateClient());
            
            dataAdapter = new Sabatex.ObjectExchange.ClientDataAdapter.Memory.DataStorage();
            dataAdapter.Initial(TestData.GetNodeAExchangeNodes());
            NodeC = new ExchangeService(apiAdapter, dataAdapter,analizer);



        }


        public string Token { get; private set; } = string.Empty;
        public HttpClient AuthenticatedClient { get; private set; }

        private readonly string _nodeName = "test-node";
        private readonly string _password = "test-password";

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Customize the services here if needed
            });
        }

        protected override void Dispose(bool disposing)
        {
            AuthenticatedClient?.Dispose();
            base.Dispose(disposing);
        }
        public async Task InitializeAsync()
        {
            AuthenticatedClient = CreateClient();

            await LoginAsync();
        }
        public async Task LoginAsync()
        {
            var loginData = new
            {
                nodeName = _nodeName,
                password = _password
            };

            var response = await AuthenticatedClient.PostAsJsonAsync("/api/v2/login", loginData);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadFromJsonAsync<TokenResponse>();
            Token = json?.Token;

            AuthenticatedClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
        }
        public async Task RefreshTokenAsync()
        {
            var refreshResponse = await AuthenticatedClient.PostAsync("/api/v2/refresh-token", null);
            refreshResponse.EnsureSuccessStatusCode();

            var json = await refreshResponse.Content.ReadFromJsonAsync<TokenResponse>();
            Token = json?.Token;

            AuthenticatedClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
        }
        private class TokenResponse
        {
            public string Token { get; set; }
        }

        public async Task<HttpClient> CreateAuthorizedClientAsync(string nodeName, string password)
        {
            var client = CreateClient();

            var loginData = new { nodeName, password };
            var response = await client.PostAsJsonAsync("/api/v2/login", loginData);
            response.EnsureSuccessStatusCode();

            var loginResult = await response.Content.ReadFromJsonAsync<TokenResponse>();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginResult?.Token);

            return client;
        }



    }

}
