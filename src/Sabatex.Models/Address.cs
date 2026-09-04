using System;
using System.Collections.Generic;
using System.Text;
using Sabatex.Core;

namespace Sabatex.Models;

public class Address : EntityBaseVersioned<Guid>
{
    /// <summary>
    /// Номер будинку або квартири, що використовується для точного визначення місця розташування.
    /// </summary>
    public string HouseNumber { get; set; } = default!;
    /// <summary>
    /// Вулиця місця розташування.
    /// </summary>
    public string Street { get; set; } = default!;
    /// <summary>
    /// Назва населеного пункту (місто, село тощо), де знаходиться адреса.
    /// </summary>
    public string City { get; set; } = default!;
    /// <summary>
    /// Штат або провінція місця розташування.
    /// </summary>
    public string State { get; set; } = default!;
    /// <summary>
    /// Поштовий індекс місця розташування.
    /// </summary>
    public string PostalCode { get; set; } = default!;
    /// <summary>
    /// Країна місця розташування.
    /// </summary>
    public string Country { get; set; } = default!;
    /// <summary>
    /// Отримує або задає представлення адреси, яке може включати форматоване рядкове представлення для відображення.
    /// </summary>
    public string Presentation
    {
        get => $"{Street}, {City}, {State}, {PostalCode}, {Country}";
        set {
            var d = value.Split(',');
            if (d.Length == 5)
                Country = d[4];
            if (d.Length >= 4)
                PostalCode = d[3];
            if (d.Length >= 3)
                State = d[2];
            if (d.Length >= 2)
                City = d[1];
            if (d.Length >= 1)
                Street = d[0];

        } 
     }
}
