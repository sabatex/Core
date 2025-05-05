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
    public class MyTestCollection : ICollectionFixture<CustomWebApplicationFactory<Program>>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }

}
