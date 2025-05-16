using Sabatex.ObjectExchange.Core;
using Sabatex.ObjectsExchange.Core;
using Sabatex.ObjectsExchange.Models;
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
        //public const string Client1Id = "161bab4f-02de-4616-b3dd-826ec8288026";
        public const string Client1Password = "DemoPassword";
        //public const string Client2Id = "161bab4f-02de-4616-b3dd-826ec8288026";
        public const string Client2Password = "DemoPassword";
        public static ClientInfo Client1 => new ClientInfo(Guid.Parse("161bab4f-02de-4616-b3dd-826ec8288026"), "DemoPassword");
        public const string NodeAId = "161bab4f-02de-4616-b3dd-826ec8288026";
        public const string NodeBId = "6e5ebe6b-120b-4d6c-8ee1-6ea0260ffc91";
        public const string NodeCId = "0196ae62-69db-7f5f-8bf3-63b4f84d3807";

        public static SabatexExchangeOptions NodeAOptions =>
            new SabatexExchangeOptions
            {
                ApiUrl = "https://localhost:5001/api/v2/",
                ClientId = Guid.Parse(NodeAId),
                ClientSecret = Client1Password
            };
        public static SabatexExchangeOptions NodeBOptions => new SabatexExchangeOptions
        {
            ApiUrl = "https://localhost:5002/api/v2/",
            ClientId = Guid.Parse(NodeBId),
            ClientSecret = Client2Password
        };
        public static SabatexExchangeOptions NodeCOptions => new SabatexExchangeOptions
        {
            ApiUrl = "https://localhost:5003/api/v2/",
            ClientId = Guid.Parse(NodeCId),
            ClientSecret = Client2Password
        };

        public static IEnumerable<ExchangeNode> GetNodeAExchangeNodes()
        {
            yield return new ExchangeNode
            {
                Id = Guid.NewGuid(),
                Description = "NodeB",
                DestinationId = Guid.Parse(NodeBId),
                IsActive = true,
                ExchangeMode = ExchangeMode.Auto,
                IsQueryEnable = true,
                IsSend = true,
                IsParse = true,
                TakeDownload = 10
            };
            yield return new ExchangeNode
            {
                Id = Guid.NewGuid(),
                Description = "NodeC",
                DestinationId = Guid.Parse(NodeCId),
                IsActive = true,
                ExchangeMode = ExchangeMode.Auto,
                IsQueryEnable = true,
                IsSend = true,
                IsParse = true,
                TakeDownload = 10
            };
        }

        public static IEnumerable<ExchangeNode> GetNodeBExchangeNodes()
        {
            yield return new ExchangeNode
            {
                Id = Guid.NewGuid(),
                Description = "NodeA",
                DestinationId = Guid.Parse(NodeAId),
                IsActive = true,
                ExchangeMode = ExchangeMode.Auto,
                IsQueryEnable = true,
                IsSend = true,
                IsParse = true,
                TakeDownload = 10
            };
        }

        public static IEnumerable<ExchangeNode> GetNodeCExchangeNodes()
        {
            yield return new ExchangeNode
            {
                Id = Guid.NewGuid(),
                Description = "NodeA",
                DestinationId = Guid.Parse(NodeAId),
                IsActive = true,
                ExchangeMode = ExchangeMode.Auto,
                IsQueryEnable = true,
                IsSend = true,
                IsParse = true,
                TakeDownload = 10
            };
 
        }



    }
}
