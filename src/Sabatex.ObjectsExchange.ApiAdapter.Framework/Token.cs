using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sabatex.ObjectsExchange.ApiAdapter
{
    public class Token
    {
        public string access_token { get; set; } = string.Empty;
        public string token_type { get; set; } = "Bearer";
        public string refresh_token { get; set; } = string.Empty;
        public int expires_in { get; set; }

    }
}
