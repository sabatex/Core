using Sabatex.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabatex.ObjectExchange.Core;

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
    public int TakeDownload { get; set; } = 10;

    public short TakeUpload { get; set; } = 10;
    public short TakeUnresolved { get; set; } = 10;

    static Dictionary<Guid,NodeDescriptor> _nodeDescriptors = new Dictionary<Guid, NodeDescriptor>();
    public NodeDescriptor GetNodeDescriptor()
    {
        if (_nodeDescriptors.TryGetValue(this.DestinationId, out var nodeDescriptor))
            return nodeDescriptor;
        nodeDescriptor = new NodeDescriptor(this.DestinationId);
        _nodeDescriptors.Add(this.DestinationId, nodeDescriptor);
        return nodeDescriptor;
    }

    public static void SetNodeDescriptor(Guid destinationId, NodeDescriptor nodeDescriptor)
    {
        if (_nodeDescriptors.ContainsKey(destinationId))
            throw new Exception("The descriptor is exist");
        else
            _nodeDescriptors.Add(destinationId, nodeDescriptor);
    }

}
