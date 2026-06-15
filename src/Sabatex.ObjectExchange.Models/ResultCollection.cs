namespace Sabatex.ObjectsExchange.Models
{
    using Sabatex.Core;
    using System.Collections.Generic;
    /// <summary>
    /// Helper class for part result collection
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public class ResultCollection<T,TKey> where T : IEntityBase<TKey>
    {
        /// <summary>
        /// Part items collection
        /// </summary>
        public IEnumerable<T> Items { get; set; } = new T[] { };
        /// <summary>
        /// total items count in database 
        /// </summary>
            public int Count { get; set; }
        }
}
