using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabatex.ObjectExchange.Core;

public interface IExchangeService
{
    Task Exchange(CancellationToken cancellationToken, bool asTasks = false);
}
