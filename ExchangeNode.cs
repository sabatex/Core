using Sabatex.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabatex.ObjectsExchange.ApiAdapter;

public class ExchangeNode:IEntityBase<Guid>,IEntityFieldDescription
{
    public Guid Id { get; set; }
    public string Description { get; set; }
    /// <summary>
    /// Вузел призначення обміну
    /// </summary>
    public Guid DestinationId { get; set; }
    /// <summary>
    /// Признак активності даного вузла (так - обмін проводити / ні - обмін призупинити) 
    /// </summary>
    public bool IsActive { get; set; }
    /// <summary>
    /// Режим обміну
    /// </summary>
    public ExchangeMode ExchangeMode { get; set; }
    /// <summary>
    /// Підтримка запитів від даного нода до цієї бази
    /// </summary>
    public bool IsQueryEnable { get; set; }
    /// <summary>
    /// Відправляти обєкти до вузла.
    /// Дозволяє припинити відправку для обслуговування черги відправки.
    /// </summary>
    public bool IsSend { get; set; }
    /// <summary>
    /// Проводити аналіз вхідних повідомлень.
    /// Дозволяє перевірити повідомлення до того як вони будуть оброблені парсером.
    /// </summary>
    public bool IsParse { get; set; }
    /// <summary>
    /// take objects by transaction
    /// </summary>
    public int Take { get; set; }

}
