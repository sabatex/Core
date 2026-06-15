using ObjectsExchange;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Sabatex.ObjectsExchange.Tests
{
    [CollectionDefinition("MyTestCollection")]

    public class MyTestCollection : ICollectionFixture<CustomWebApplicationFactory>
    {


    }
    [CollectionDefinition("IntegrationTestCollection")]

    public class IntegrationTestCollection : ICollectionFixture<IntegrationFactory>
    {


    }

}
