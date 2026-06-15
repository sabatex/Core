using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabatex.Core.Identity;
/// <summary>
/// Provides custom claim type URIs used for application-specific identity claims.
/// </summary>
/// <remarks>This class defines constants for use with claims-based identity systems, such as those based on
/// System.Security.Claims. Use these constants to avoid hard-coding claim type URIs throughout your
/// application.</remarks>
public static class CustomClaimTypes
{
    /// <summary>
    /// Represents the URI for the 'full name' claim used in identity and authentication scenarios.
    /// </summary>
    /// <remarks>This constant can be used when working with claims-based identity to refer to a user's full
    /// name in a standardized way. It is typically used as the claim type identifier when creating or querying
    /// claims.</remarks>
    public const string FullName = "http://schemas.yourapp.org/claims/fullname";
}
