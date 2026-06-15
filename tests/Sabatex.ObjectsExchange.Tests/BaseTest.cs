using Microsoft.AspNetCore.Mvc.Testing;
using ObjectsExchange;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Sabatex.ObjectsExchange.Tests;

public abstract class BaseTest: IClassFixture<WebApplicationFactory<Program>>
{
    protected readonly WebApplicationFactory<Program> _factory;
    protected readonly ITestOutputHelper _output;
    protected BaseTest(WebApplicationFactory<Program> factory, ITestOutputHelper output)
    {
        _factory = factory;
        _output = output;
    }
}
