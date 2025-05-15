using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabatex.ObjectExchange.Core
{
    public abstract class ObjectAnalizer
    {
        public readonly string ObjectType;
        protected ObjectAnalizer(string ObjectType)
        {
            this.ObjectType = ObjectType;
        }


        public async Task<AnalizeResult> AnalyzeAsync(AnalizerObjectContext context)
        {
            context.Error($"Object type {ObjectType} not implemented");
            return new AnalizeResult(false,string.Join("\r\n",context.ErrorMessages));
        }

    }
}
