using Sabatex.Core;
using System.ComponentModel.DataAnnotations;
using System;


namespace Sabatex.ObjectsExchange.Models
{
    /// <summary>
    /// Helper class for post object to service
    /// </summary>
    public sealed class PostObject
    {
        /// <summary>
        /// Client object type
        /// </summary>
        public string objectType { get; set; }=string.Empty;
        /// <summary>
        /// Client object Id
        /// </summary>
        public string objectId = string.Empty;
        /// <summary>
        /// Serialized object
        /// </summary>
        public string text = string.Empty;
        /// <summary>
        /// Moment then object serialized on Client mashine
        /// </summary>
        public DateTime dateStamp = DateTime.UtcNow;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="objectType"></param>
        /// <param name="objectId"></param>
        /// <param name="text"></param>
        public PostObject(string objectType, string objectId,string text)
        {
            this.objectType = objectType;
            this.objectId = objectId;
            this.text = text;
        }
    }
}