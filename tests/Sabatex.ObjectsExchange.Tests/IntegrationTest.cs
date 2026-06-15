using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Sabatex.ObjectsExchange.Tests;

[Collection("IntegrationTestCollection")]
[TestCaseOrderer(typeof(PriorityOrderer))]
public class IntegrationTest
{
    private readonly IntegrationFactory _factory;
    public IntegrationTest(IntegrationFactory factory)
    {
        _factory = factory;
    }

    [Fact, TestPriority(1)]
    public async Task InitialUploadDataNodeA()
    {

        var files = Directory.GetFiles("C:/var/upload/bakery-mobile", "*.json", new EnumerationOptions { RecurseSubdirectories = true });
        foreach (var file in files)
        {
            var fileName = Path.GetFileName(file);
            var objectType = fileName.Split(' ')[0];
            var objectId = fileName.Split(' ')[1].Split('.')[0];
            var fileContent = await File.ReadAllTextAsync(file);
            var messageHeader = $"{{\"type\":\"{objectType}\",\"id\":\"{objectId}\"}}";
            await _factory.NodeA.DataAdapter.RegisterUploadMessageAsync(_factory.NodeB.ExchangeApiAdapter.ClientId, messageHeader, fileContent);
            await _factory.NodeA.DataAdapter.RegisterUploadMessageAsync(_factory.NodeC.ExchangeApiAdapter.ClientId, messageHeader, fileContent);
        }
    }

    [Fact, TestPriority(2)]
    public async Task ExchangeNodeA()
    {
         await _factory.NodeA.Exchange(new System.Threading.CancellationToken(), asTasks: false);
    }

    [Fact, TestPriority(3)]
    public async Task ExchangeNodeB()
    {
        await _factory.NodeB.Exchange(new System.Threading.CancellationToken(), asTasks: false);
    }
    [Fact, TestPriority(3)]
    public async Task ExchangeNodeC()
    {
        await _factory.NodeC.Exchange(new System.Threading.CancellationToken(), asTasks: false);
    }

}
