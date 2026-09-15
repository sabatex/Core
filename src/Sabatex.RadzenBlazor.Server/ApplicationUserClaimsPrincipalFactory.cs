using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Sabatex.Core.Identity;
using System.Security.Claims;

namespace Sabatex.RadzenBlazor.Server;

/// <summary>
/// Represents a factory for creating claims principals for application users, extending the default UserClaimsPrincipalFactory to include custom claims such as FullName.
/// </summary>
public class ApplicationUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ApplicationUserClaimsPrincipalFactory"/> class with the specified user manager and identity options.
    /// </summary>
    /// <param name="userManager">The user manager.</param>
    /// <param name="optionsAccessor">The options accessor.</param>
    public ApplicationUserClaimsPrincipalFactory(UserManager<ApplicationUser> userManager, IOptions<IdentityOptions> optionsAccessor)
        : base(userManager, optionsAccessor) { }
    /// <summary>
    /// Generates a claims identity for the specified application user, adding custom claims such as FullName if available. 
    /// </summary>
    /// <param name="user">The application user.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the generated claims identity.</returns>
    protected override async Task<ClaimsIdentity> GenerateClaimsAsync(ApplicationUser user)
    {
        var identity = await base.GenerateClaimsAsync(user);

        if (!string.IsNullOrEmpty(user.FullName))
        {
            identity.AddClaim(new Claim(CustomClaimTypes.FullName, user.FullName));
        }

        return identity;
    }
}