using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabatex.ObjectExchange.Core
{
    /// <summary>
    /// Defines an analyzer for objects.
    /// </summary>
    public interface IObjectAnalizer
    {
        /// <summary>
        /// Analyzes an object asynchronously.
        /// </summary>
        /// <param name="result" >Optional initial analysis result to be used as a starting point for the analysis. If null, a new analysis will be performed.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the analysis result.</returns>
        public Task<AnalizeResult> AnalyzeAsync(AnalizeResult? result);
    }
}
