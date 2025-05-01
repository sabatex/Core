using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using ObjectsExchange;
using Xunit;
using Sabatex.ObjectsExchange.Tests;
using Sabatex.ObjectsExchange.Models;


namespace WebApiDocumentEx.Test;
public class v0Test : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    const string apiRoute = "api/v1";

    public v0Test(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
     }

    [Fact]
    public async Task Login()
    {
        // login client 1
        var client1 = _factory.CreateClient();
        Assert.NotNull(client1);
        // bad fake all
        var responce = await client1.PostAsJsonAsync($"{apiRoute}/login", new { clientId = "fake", password = "fake" });
        Assert.NotNull(responce);
        Assert.False(responce.IsSuccessStatusCode);
        // bad fake password
        responce = await client1.PostAsJsonAsync($"{apiRoute}/login", new { clientId = TestData.Client1.ClientId, password = "fake" });
        Assert.NotNull(responce);
        Assert.False(responce.IsSuccessStatusCode);
    
        responce = await client1.PostAsJsonAsync($"{apiRoute}/login", new { clientId = TestData.Client1.ClientId, password = TestData.Client1.Password });
        Assert.NotNull(responce);
        Assert.True(responce.IsSuccessStatusCode);
        var token = await responce.Content.ReadFromJsonAsync<Token>();
        Assert.NotNull(token);
    }


    //private async Task SendObjectsAsync(ExchangeApiConnector client, int count)
    //{
    //    for (int i = 0; i < 100; i++)
    //    {
    //        var postObject = new
    //        {
    //            objectType = "Довідник.Номенклатура",
    //            objectId = Guid.NewGuid().ToString(),
    //            text = TestConst.TestConstString,
    //            dateStamp = DateTime.Now
    //        };
    //        await client.PostObjectAsync("Довідник.Номенклатура", Guid.NewGuid().ToString(), TestConst.TestConstString);
    //    }
    //}

    //private async Task RandomAnalize(HttpClient client, string destination,ObjectExchange objectExchange)
    //{
    //    if (Random.Shared.Next(0, 3) != 0)
    //    {
    //        await client.ApiDeleteObjectExchangeAsync(objectExchange.Id);
    //    }
    //    else
    //    {
    //        await client.ApiPostQueryAsync(destination, "Довідник.ОдиниціВиміру", Random.Shared.Next(1, 10).ToString());
    //        await client.ApiDeleteObjectExchangeAsync(objectExchange.Id);
    //    }

    //}

    //private async Task<int> ReadObjectsAsync(HttpClient client, string destination,int count,bool random=false)
    //{
    //    int result = 0;
    //    while (result < count)
    //    {
    //        int c = count - result;
    //        int take = c < 10 ? c : 10;
    //        if (take == 0)
    //            return result;
    //        var objects = await client.ApiGetObjectExchangeAsync(destination,take);
    //        if (objects.Count() == 0)
    //            return result;

    //        result = result + objects.Count();
    //        foreach (var obj in objects)
    //        {
    //            if (random)
    //                await RandomAnalize(client,destination,obj);
    //            else
    //                await client.ApiDeleteObjectExchangeAsync(obj.Id);
    //        }
    //    }
    //    return result;
    //}


    //[Fact, Priority(1)]
    //public async void TestLogin()
    //{
    //    // логінимось на сервері
    //    var senderHTTPClient = _factory.CreateClient();
    //    Assert.True(await senderHTTPClient.Login(senderNodeName, "1"));
    //    var destinationHTTPClient = _factory.CreateClient();
    //    Assert.True(await destinationHTTPClient.Login(destinationNodeName, "1"));
    //}


    //[Fact,Priority(2)]
    //public async void TestObjectExchange()
    //{
    //    // логінимось на сервері
    //    var senderHTTPClient = _factory.CreateClient();
    //    Assert.True(await senderHTTPClient.Login(senderNodeName, "1"));
    //    var destinationHTTPClient = _factory.CreateClient();
    //    Assert.True(await destinationHTTPClient.Login(destinationNodeName, "1"));

    //     // send 100 objects
    //    var postObjects = await SendObjectsAsync(senderHTTPClient,destinationNodeName,100);
    //    Assert.Equal(100, postObjects.Count());

    //    // get 100 objects
    //    var getObjects = await destinationHTTPClient.ApiGetObjectExchangeAsync(senderNodeName, 100);
    //    Assert.Equal(100, getObjects.Count());

    //    // delete 100 objects
    //    foreach (var obj in getObjects)
    //    {
    //        await destinationHTTPClient.ApiDeleteObjectExchangeAsync(obj.Id);
    //    }

    //    getObjects = await destinationHTTPClient.ApiGetObjectExchangeAsync(senderNodeName, 1000);
    //    foreach (var obj in getObjects)
    //    {
    //        await destinationHTTPClient.ApiDeleteObjectExchangeAsync(obj.Id);
    //    }

    //    Assert.True(getObjects.Count()==0);



    //}

    //[Fact, Priority(3)]
    //public async void TestQueryExchange()
    //{
    //    // логінимось на сервері
    //    var senderHTTPClient = _factory.CreateClient();
    //    Assert.True(await senderHTTPClient.Login(senderNodeName, "1"));
    //    var destinationHTTPClient = _factory.CreateClient();
    //    Assert.True(await destinationHTTPClient.Login(destinationNodeName, "1"));

    //    for (int i = 0; i < 100; i++)
    //    {
    //        await senderHTTPClient.ApiPostQueryAsync(destinationNodeName, "TestObjectType", Random.Shared.Next(0, 10).ToString());
    //    }

    //    var queries = await destinationHTTPClient.ApiGetQueryObjectAsync(senderNodeName, 100);
    //    Assert.Equal(10,queries.Count());

    //    foreach (var query in queries)
    //    {
    //        await destinationHTTPClient.ApiDeleteQueryObjectAsync(query.Id);
    //    }

    //    queries = await destinationHTTPClient.ApiGetQueryObjectAsync(senderNodeName, 100);


    //    foreach (var query in queries)
    //    {
    //        await destinationHTTPClient.ApiDeleteQueryObjectAsync(query.Id);
    //    }
    //    Assert.True(queries.Count()==0);
    //}



}
