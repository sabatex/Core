using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabatex.ObjectExchange.Core;
/// <summary>
/// Defines an interface for analyzing messages in the context of an exchange node. This interface is responsible for providing a method to analyze messages based on their headers and content, and returning the result of the analysis, which indicates whether the analysis was successful and any associated error messages if applicable.
/// </summary>
public interface IExchangeAnalizer
{
    /// <summary>
    /// Analyzes a message based on the provided exchange node, message header, and message content. The method returns an AnalizeResult indicating the success of the analysis and any error messages if applicable. This method is designed to evaluate the message in the context of the specified exchange node and determine whether it meets certain criteria or conditions defined by the implementation of the IExchangeAnalizer interface.
    /// </summary>
    /// <param name="exchangeNode">The exchange node in the context of which the message is being analyzed.</param>
    /// <param name="messageHeader">The header of the message to be analyzed.</param>
    /// <param name="message">The content of the message to be analyzed.</param>
    /// <returns>An AnalizeResult indicating the success of the analysis and any error messages if applicable.</returns>
    Task<AnalizeResult> MessageAnalizeAsync(ExchangeNode exchangeNode, string messageHeader,string? message);
}
/// <summary>
/// Represents the result of an analysis operation performed by the IExchangeAnalizer. This record contains a boolean property indicating whether the analysis was successful and an optional string property for any error messages that may have occurred during the analysis process. The Success property indicates the outcome of the analysis, while the ErrorMessage property provides additional information in case of failure or issues encountered during the analysis.
/// </summary>
/// <param name="Success">Indicates whether the analysis was successful.</param>
/// <param name="ErrorMessage">Provides additional information in case of failure or issues encountered during the analysis.</param>
public sealed record AnalizeResult(bool Success, string? ErrorMessage = null);