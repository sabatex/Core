using Sabatex.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabatex.ObjectsExchange.Models;
/// <summary>
/// Presents user in client
/// </summary>
public class ClientUser : IEntityBase<Guid>
{
    /// <summary>
    /// Primary key
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Normalized user name (ADMINISTRATOR@CONTOSO.COM)
    /// </summary>
    public string UserName { get; set; } = default!;
    
    /// <summary>
    /// User role (Client or ClientUser)
    /// </summary>
    public string UserRole { get; set; } = default!;
    
    /// <summary>
    /// Foregin key for client
    /// </summary>
    public Guid ClientId { get; set; }

}
