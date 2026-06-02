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
        /// Asynchronously analyzes the object and returns a boolean indicating the success of the operation.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating the success of the analysis.</returns>
        public Task<bool> AnalyzeAsync();
        /// <summary>
        /// Represents the result of an analysis operation.
        /// </summary>
        public bool Success { get; }
        /// <summary>
        /// Gets the error message if the analysis operation failed.
        /// </summary>
        public string? ErrorMessage { get; }
    }
}
