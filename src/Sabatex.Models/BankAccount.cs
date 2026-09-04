using Sabatex.Core;

namespace Sabatex.Models;
/// <summary>
/// Represents a bank account with an International Bank Account Number (IBAN).
/// </summary>
public class BankAccount : EntityBaseVersioned<Guid>
{
    /// <summary>
    /// Gets or sets the International Bank Account Number (IBAN) of the bank account.
    /// </summary>
    public string IBAN { get; set; } = string.Empty;

}
