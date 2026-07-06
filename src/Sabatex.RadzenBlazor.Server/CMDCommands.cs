using System;
using System.Collections.Generic;
using System.Text;

namespace Sabatex.RadzenBlazor.Server;
/// <summary>
/// Represents the available command-line commands for the application, providing a clear enumeration of operations that can be performed via command-line arguments.
/// </summary>
public enum CMDCommands
{
    /// <summary>
    /// Command to perform database migration.
    /// </summary>
    Migrate,
    /// <summary>
    /// Command to assign administrative privileges to a user.
    /// </summary>
    Admin,
    /// <summary>
    /// Command to initialize the database with default data.
    /// </summary>
    Initialise,
}
