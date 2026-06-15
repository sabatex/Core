using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Sabatex.Core.Identity;
/// <summary>
/// Represents external authentication information provided by a third-party login provider, including provider details
/// and associated user claims.
/// </summary>
/// <remarks>This data transfer object is typically used to convey information obtained from external
/// authentication sources, such as OAuth or OpenID Connect providers. It includes the provider's identifier, a unique
/// key for the user within that provider, and a collection of claims that may contain user profile information such as
/// email or name.</remarks>
public class ExternalLoginInfoDTO
{
    /// <summary>
    /// Gets the name of the provider associated with this instance.
    /// </summary>
    public string Provider { get; init; } = "";
    /// <summary>
    /// Gets the unique key or identifier for the authentication provider.
    /// </summary>
    public string ProviderKey { get; init; } = "";
    /// <summary>
    /// Gets the collection of claims associated with the current instance.
    /// </summary>
    public IEnumerable<(string Type, string Value)> Claims { get; init; } = Array.Empty<(string, string)>();

    /// <summary>
    /// Gets the email address associated with the current claims principal, if available.
    /// </summary>
    public string? Email =>
      Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value;
    /// <summary>
    /// Gets the full name of the user, if available.
    /// </summary>
    public string? Name => Claims.GetFullNameOrDefault();
}
