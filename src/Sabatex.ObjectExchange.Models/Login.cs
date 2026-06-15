namespace Sabatex.ObjectsExchange.Models;
using System;
/// <summary>
/// Login data class
/// </summary>
public class Login
{
    /// <summary>
    /// client id
    /// </summary>
    public Guid ClientId { get; set; }
    /// <summary>
    /// Client password
    /// </summary>
    public string Password { get; set; }  = default!;
}