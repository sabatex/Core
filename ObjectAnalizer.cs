using Sabatex.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabatex.ObjectExchange.Core;

public abstract class ObjectAnalizer<T> :IObjectAnalizer where T : EntityBase<Guid>, new ()
{
    public bool Success { get; }
    public string? ErrorMessage { get; }

    public virtual async Task<bool> AnalyzeAsync()
    {
        return await Task.FromResult(true);
    }

}
