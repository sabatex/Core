using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabatex.Core.Identity;

/// <summary>
/// Represents the result of a sign-in attempt.
/// </summary>
/// <remarks>This enumeration is used to indicate the outcome of a sign-in operation. It provides information
/// about whether the sign-in was successful, requires additional steps, or failed due to specific conditions.</remarks>
public enum SignInStatus
{
    /// <summary>
    /// Indicates that the operation completed successfully.
    /// </summary>
    Success,
    /// <summary>
    /// Gets or sets a value indicating whether two-factor authentication is required for the user or operation.
    /// </summary>
    RequiresTwoFactor,
    /// <summary>
    /// Gets or sets a value indicating whether the user account is locked out.
    /// </summary>
    LockedOut,
    /// <summary>
    /// Indicates that the provided credentials are invalid.
    /// </summary>
    InvalidCredentials
}