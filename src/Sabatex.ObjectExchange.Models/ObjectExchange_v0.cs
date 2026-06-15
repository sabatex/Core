    using Sabatex.Core;
    using System.ComponentModel.DataAnnotations;
    using System;
using System.Text.Json;
using System.Text.Encodings.Web;
using System.Text.Unicode;

namespace Sabatex.ObjectsExchange.Models
{

    record ObjectDescription(string id,string type);


    /// <summary>
    /// Object for exchange
    /// </summary>
    public class ObjectExchange_v0 : IEntityBase<long>
    {
        public ObjectExchange_v0()
        {
            
        }
        /// <summary>
        /// 
        /// </summary>
        public ObjectExchange_v0(ObjectExchange objectExchange)
        {
           var objectDescription = JsonSerializer.Deserialize<ObjectDescription>(objectExchange.MessageHeader);
           this.ObjectId = objectDescription.id;
           this.ObjectType = objectDescription.type;
            this.ObjectAsText = objectExchange.Message;
            this.Sender = objectExchange.Sender;
            this.SenderDateStamp = objectExchange.SenderDateStamp;
            this.DateStamp = objectExchange.DateStamp;
            this.Destination = objectExchange.Destination;
            this.Id = objectExchange.Id;
        }

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
        [MaxLength(50)]
        public string ObjectId { get; set; } = default!;
        /// <summary>
        /// client object type
        /// </summary>
        [MaxLength(50)]
        public string ObjectType { get; set; } = default!;
 
        /// <summary>
        /// внутрішня позначка часу створення обєкта
        /// </summary>
        public DateTime DateStamp { get; set; } = DateTime.UtcNow;
        
        /// <summary>
        /// sender date send object
        /// </summary>
        public DateTime? SenderDateStamp { get; set; }

        /// <summary>
        /// serialized object (json,xml,csv...)
        /// </summary>
        public string ObjectAsText { get; set; } = default!;


        public ObjectExchange GetObjectExchange()
        {
            var result = new ObjectExchange();
            result.MessageHeader = JsonSerializer.Serialize(new ObjectDescription(this.ObjectId, this.ObjectType),new JsonSerializerOptions {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic)});
            result.Message = this.ObjectAsText;
            result.Sender = this.Sender;
            result.Destination = this.Destination;
            result.Id = this.Id;
            result.SenderDateStamp= this.SenderDateStamp;
            result.DateStamp = this.DateStamp;
            return result;
        }

    }
}
