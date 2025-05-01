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

using Sabatex.ObjectsExchange.Models;
using Sabatex.ObjectsExchange.ApiAdapter;
using Sabatex.ObjectsExchange.Tests;
using Radzen.Blazor.Rendering;
using System.Runtime.CompilerServices;
using System.Diagnostics;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Engine.ClientProtocol;
using Org.BouncyCastle.Tls;
using System.IO;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace WebApiDocumentEx.Test;

public class v1Test :  IClassFixture<WebApplicationFactory<Program>>
{
    private readonly ITestOutputHelper _output;
    private readonly WebApplicationFactory<Program> _factory;
     
    const string  apiRoute = "api/v1";


    public v1Test(WebApplicationFactory<Program> factory, ITestOutputHelper output)
    {
        _output = output;
        _factory = factory;
    }

    [Fact]
    public async Task Login()
    {
        // login client 1
        var client1 = _factory.CreateClient();
        var responce = await client1.PostAsJsonAsync($"{apiRoute}/login", new { clientId = TestData.Client1.ClientId, password = TestData.Client1.Password });
        Assert.NotNull(responce);
        Assert.True(responce.IsSuccessStatusCode);
        var token = await responce.Content.ReadFromJsonAsync<Token>();
        Assert.NotNull(token);
    }




 


    //[Fact]
    //public async Task ExchangeApiAdapter2()
    //{
    //    var stopwatch = Stopwatch.StartNew();
    //    await RegisterUploadDataAsync();
    //    _output.WriteLine($"RegisterUploadData 100 Execution Time: {stopwatch.ElapsedMilliseconds} ms");
        
    //    stopwatch.Restart();
    //    Assert.True(await _exchangeApiAdapter.RefreshTokenAsync()); 
    //    _output.WriteLine($"First RefreshToken Execution Time: {stopwatch.ElapsedMilliseconds} ms");
        
    //    stopwatch.Restart();
    //    Assert.True(await _exchangeApiAdapter.RefreshTokenAsync());
    //    _output.WriteLine($"Last RefreshToken Execution Time: {stopwatch.ElapsedMilliseconds} ms");
        
    //    stopwatch.Restart();
    //    var uploads = await _exchangeApiAdapter.GetUploadObjectsAsync(_destination1ExchangeApiAdapter.ClientId, 100);
    //    Assert.Equal(100, uploads.Count());
    //    _output.WriteLine($"GetUploadObjectsAsync(100) Execution Time: {stopwatch.ElapsedMilliseconds} ms");
        
    //    stopwatch.Restart();
    //    foreach (var upload in uploads)
    //    {
    //        await _exchangeApiAdapter.PostObjectAsync(_destination1ExchangeApiAdapter.ClientId, upload.MessageHeader, upload.Message,upload.DateStamp);
    //        await _exchangeApiAdapter.RemoveUploadMessageAsync(_destination1ExchangeApiAdapter.ClientId,upload.Id);
    //    }
    //    uploads = await _exchangeApiAdapter.GetUploadObjectsAsync(_destination1ExchangeApiAdapter.ClientId, 100);
    //    Assert.Equal(0, uploads.Count());
    //    _output.WriteLine($"Upload and delete 100 objects Execution Time: {stopwatch.ElapsedMilliseconds} ms");

    //    stopwatch.Restart();
    //    Assert.True(await _destination1ExchangeApiAdapter.RefreshTokenAsync());
    //    _output.WriteLine($"refresh destination token Execution Time: {stopwatch.ElapsedMilliseconds} ms");
        
    //    stopwatch.Restart();
    //    var objects = await _destination1ExchangeApiAdapter.GetObjectsAsync(_exchangeApiAdapter.ClientId, 50);
    //    Assert.Equal(50, objects.Count());
    //    await _destination1ExchangeApiAdapter.DeleteRange(objects.Select(s => s.Id).ToArray());
    //    _output.WriteLine($"Download from Api and delete_range 50 objects Execution Time: {stopwatch.ElapsedMilliseconds} ms");

    //    stopwatch.Restart();
    //    objects = await _destination1ExchangeApiAdapter.GetObjectsAsync(_exchangeApiAdapter.ClientId, 50);
    //    Assert.Equal(50, objects.Count());

    //    foreach (var obj in objects)
    //    {
    //        await _destination1ExchangeApiAdapter.DeleteObjectAsync(obj.Id);
    //    }
    //    _output.WriteLine($"Download from Api and delete 50 objects Execution Time: {stopwatch.ElapsedMilliseconds} ms");

    //    stopwatch.Restart();

    //    stopwatch.Stop();
    //}


 

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
