using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ObjectsExchange.ApiAdapter
{
    public class SessionContext
    {
        public Uri Host { get; set; }
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public Guid ClientId { get; set; }
        public string Password { get; set; } = string.Empty;
        public int Take { get; set; } = 100;
    }
}
