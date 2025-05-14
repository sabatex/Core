using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
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
    public class IntegrationFactory : WebApplicationFactory<Program> 
    {
        public ExchangeService NodeA { get;private set; }
        public ExchangeService NodeB { get; private set; }
        public ExchangeService NodeC { get; private set; }
        public string Token { get; private set; } = string.Empty;
        //public HttpClient AuthenticatedClient { get; private set; }

        public IntegrationFactory()
        {
            var mockLocalizer = new Mock<IStringLocalizer<MessageAnalizer>>();
            mockLocalizer
                .Setup(m => m[It.IsAny<string>()])
                .Returns((string key) => new LocalizedString(key, key));
            
            var analizer = new MessageAnalizer(mockLocalizer.Object);
            
            NodeA = new ExchangeService(new ExchangeApiAdapterV2(TestData.NodeAOptions, CreateClient()),
                                        new Sabatex.ObjectExchange.ClientDataAdapter.Memory.DataStorage("MemoryDatabaseA").Initial(TestData.GetNodeAExchangeNodes()),
                                        analizer);
            NodeB = new ExchangeService(new ExchangeApiAdapterV2(TestData.NodeBOptions, CreateClient()),
                    new Sabatex.ObjectExchange.ClientDataAdapter.Memory.DataStorage("MemoryDatabaseB").Initial(TestData.GetNodeBExchangeNodes()),
                    analizer);
            NodeC = new ExchangeService(new ExchangeApiAdapterV2(TestData.NodeCOptions, CreateClient()),
                                        new Sabatex.ObjectExchange.ClientDataAdapter.Memory.DataStorage("MemoryDatabaseC").Initial(TestData.GetNodeCExchangeNodes()),
                                        analizer);
        }


        //protected override void ConfigureWebHost(IWebHostBuilder builder)
        //{
        //    builder.ConfigureServices(services =>
        //    {
        //        // Customize the services here if needed
        //    });
        //}

        ////protected override void Dispose(bool disposing)
        //{
        //    AuthenticatedClient?.Dispose();
        //    base.Dispose(disposing);
        //}
        //public async Task InitializeAsync()
        //{
        //    AuthenticatedClient = CreateClient();

        //    await LoginAsync();
        //}
        //public async Task LoginAsync()
        //{
        //    var loginData = new
        //    {
        //        nodeName = _nodeName,
        //        password = _password
        //    };

        //    var response = await AuthenticatedClient.PostAsJsonAsync("/api/v2/login", loginData);
        //    response.EnsureSuccessStatusCode();

        //    var json = await response.Content.ReadFromJsonAsync<TokenResponse>();
        //    Token = json?.Token;

        //    AuthenticatedClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
        //}
        //public async Task RefreshTokenAsync()
        //{
        //    var refreshResponse = await AuthenticatedClient.PostAsync("/api/v2/refresh-token", null);
        //    refreshResponse.EnsureSuccessStatusCode();

        //    var json = await refreshResponse.Content.ReadFromJsonAsync<TokenResponse>();
        //    Token = json?.Token;

        //    AuthenticatedClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
        //}
        //private class TokenResponse
        //{
        //    public string Token { get; set; }
        //}

        //public async Task<HttpClient> CreateAuthorizedClientAsync(string nodeName, string password)
        //{
        //    var client = CreateClient();

        //    var loginData = new { nodeName, password };
        //    var response = await client.PostAsJsonAsync("/api/v2/login", loginData);
        //    response.EnsureSuccessStatusCode();

        //    var loginResult = await response.Content.ReadFromJsonAsync<TokenResponse>();
        //    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginResult?.Token);

        //    return client;
        //}



    }

}
