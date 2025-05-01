using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabatex.ObjectsExchange.Tests
{
    public record ClientInfo(Guid ClientId, string Password);
 
    public static class TestData
    {
        public static ClientInfo Client1 => new ClientInfo(Guid.Parse("161bab4f-02de-4616-b3dd-826ec8288026"), "DemoPassword");
        public static ClientInfo Client2 => new ClientInfo(Guid.Parse("161bab4f-02de-4616-b3dd-826ec8288026"), "DemoPassword");

    }
}
