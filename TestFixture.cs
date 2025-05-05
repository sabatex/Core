using Microsoft.AspNetCore.Mvc.Testing;
using ObjectsExchange;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabatex.ObjectsExchange.Tests
{
    public class TestFixture 
    {
        private static readonly Lazy<WebApplicationFactory<Program>> _factory = new(() => new WebApplicationFactory<Program>());
        public static WebApplicationFactory<Program> Instance => _factory.Value;
    }
}
