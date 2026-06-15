using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabatex.ObjectsExchange.HttpClientApiConnector;

public class ClentNodeConfig
{
    public string BaseUri { get; set; } = "https://sabatex.francecentral.cloudapp.azure.com/";
    public string ClientId { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public bool AcceptFailCertificates { get; set; } = false;
}
