using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabatex.Core.RadzenBlazor;
/// <summary>
/// Represents a foreign key reference with a name and identifier.
/// </summary>
/// <param name="Name">The name of the foreign key. This typically corresponds to the constraint or relationship name in the database
/// schema.</param>
/// <param name="Id">The identifier value associated with the foreign key. This is usually the referenced primary key value.</param>
public record ForeginKey(string Name, string Id);
