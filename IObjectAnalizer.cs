using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabatex.ObjectExchange.Core
{
    public interface IObjectAnalizer
    {
        public string ObjectType { get; }
        public Task<AnalizeResult> AnalyzeAsync(AnalizerObjectContext context);
    }
}
