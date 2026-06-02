using System;
using System.Collections.Generic;
using System.Text;

namespace Sabatex.ObjectExchange.Core;
/// <summary>
/// Абстрактий класс для аналізаторів, який надає базову реалізацію для збору та управління повідомленнями про помилки під час процесу аналізу. Цей клас містить список для зберігання повідомлень про помилки, а також властивості для отримання конкатенованого рядка повідомлень про помилки та індикатора успішності аналізу. Метод Error дозволяє додавати повідомлення про помилки до списку та повертає false для позначення виникнення помилки.
/// </summary>
public abstract class Analizer
{
    private List<string> ErrorMessages = new List<string>();
    /// <summary>
    /// Gets a string that represents the error messages collected during the analysis process. If there are multiple error messages, they are concatenated into a single string with each message separated by a new line. If there are no error messages, an empty string is returned. This property provides a convenient way to access and display the errors encountered during the analysis.
    /// </summary>
    public string ErrorMessage => string.Join(Environment.NewLine, ErrorMessages);
    /// <summary>
    /// Gets a value indicating whether the analysis was successful. This property returns true if no error messages were collected during the analysis; otherwise, it returns false.
    /// </summary>
    public bool Success => ErrorMessages.Count == 0;
    /// <summary>
    /// Adds an error message to the list of error messages and returns false. This method is used to record errors encountered during the analysis process. By calling this method with a specific error message, the message is added to the collection of error messages, and the method returns false to indicate that an error has occurred.
    /// </summary>
    /// <param name="message">The error message to add.</param>
    /// <returns>Always returns false.</returns>
    protected bool Error(string message)
    {
        ErrorMessages.Add(message);
        return false;
    }
    public Task<bool> AnalyzeAsync() 
    {
        return Task.FromResult(true); 
    }

}
