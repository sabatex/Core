using System;

namespace Sabatex.ObjectsExchange.ApiAdapter
{

    public class ObjectExchange
    {
        public long Id { get; set; }
        public Guid Sender { get; set; }
        public Guid Destination { get; set; }
        public string MessageHeader { get; set; }
        /// <summary>
        /// внутрішня позначка часу створення обєкта
        /// </summary>
        public DateTime DateStamp { get; set; }
        public DateTime? SenderDateStamp { get; set; }
        public string Message { get; set; }
    }
}
