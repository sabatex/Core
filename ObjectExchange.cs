    using Sabatex.Core;
    using System.ComponentModel.DataAnnotations;
    using System;

namespace Sabatex.ObjectsExchange.Models
{
    /// <summary>
    /// Object for exchange
    /// </summary>
    public class ObjectExchange : IEntityBase<long>
    {
        /// <summary>
        /// primary key
        /// </summary>
        public long Id { get; set; }
        
        /// <summary>
        /// sender id
        /// </summary>
        public Guid Sender { get; set; }
        
        /// <summary>
        /// destination id
        /// </summary>
        public Guid Destination { get; set; }
        
        /// <summary>
        /// internal unique client object id  
        /// </summary>
        [Required]
        [MaxLength(150)]
        public string MessageHeader { get; set; } = default!;
        
        /// <summary>
        /// server datestamp moment 
        /// </summary>
        public DateTime DateStamp { get; set; } = DateTime.UtcNow;
        
        /// <summary>
        /// sender date send object
        /// </summary>
        public DateTime? SenderDateStamp { get; set; }
        
        /// <summary>
        /// serialized object (json,xml,csv...)
        /// </summary>
        public string? Message { get; set; } = default!;

        /// <summary>
        /// message size for calculate transmited data
        /// </summary>
        //public int? MessageSize { get; set; }

    }
}
