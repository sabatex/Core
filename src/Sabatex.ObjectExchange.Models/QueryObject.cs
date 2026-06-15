using Sabatex.Core;
using System.ComponentModel.DataAnnotations;
using System;
using System.Text.Json;
using System.Text.Encodings.Web;
using System.Text.Unicode;
namespace Sabatex.ObjectsExchange.Models
{
        /// <summary>
        /// обєкти які потрібно отримати з нода
        /// </summary>
        public class QueryObject : IEntityBase<long>
        {
        public QueryObject()
        {
            
        }

        public QueryObject(ObjectExchange objectExchange)
        {
            var objectDescription = JsonSerializer.Deserialize<ObjectDescription>(objectExchange.MessageHeader);
            this.ObjectId = objectDescription.id;
            this.ObjectType = objectDescription.type;
            this.Id = objectExchange.Id;
            this.Sender = objectExchange.Sender;
            this.Destination = objectExchange.Destination;

        }
        /// <summary>
        /// primary key
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// Sender client id
        /// </summary>
            public Guid Sender { get; set; }
        /// <summary>
        /// Destination client id
        /// </summary>
            public Guid Destination { get; set; }
        /// <summary>
        /// Client object unique id
        /// </summary>
        [MaxLength(50)]
        public string ObjectId { get; set; } = default!; //must lovercase
        /// <summary>
        /// client object type
        /// </summary>
        [MaxLength(50)]
        public string ObjectType { get; set; } = default!; // lovercase
                                                           //string IEntityBase.KeyAsString() => Id.ToString();


        public ObjectExchange GetObjectExchange()
        {
            var result = new ObjectExchange();
            result.MessageHeader = JsonSerializer.Serialize(new ObjectDescription(this.ObjectId, this.ObjectType), new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic)
            });
            result.Message = null;
            result.Sender = this.Sender;
            result.Destination = this.Destination;
            result.Id = this.Id;
            result.SenderDateStamp = DateTime.UtcNow;
            result.DateStamp = DateTime.UtcNow;
            return result;
        }

    }

}