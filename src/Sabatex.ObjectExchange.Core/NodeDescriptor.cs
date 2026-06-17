using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabatex.ObjectExchange.Core
{
    /// <summary>
    /// Represents a descriptor for a node in the object exchange system. This class is responsible for managing analyzers associated with different object types and providing access to these analyzers based on the object type.
    /// </summary>
    public class NodeDescriptor
    {
        private readonly Dictionary<string,Type> _analizers = new Dictionary<string, Type>();
        public readonly Guid DestinationId;
        /// <summary>
        /// Initializes a new instance of the NodeDescriptor class with the specified destination ID. The destination ID is used to identify the node in the object exchange system.
        /// </summary>
        /// <param name="destinationId">The unique identifier for the destination node.</param>
        public NodeDescriptor(Guid destinationId)
        {
            DestinationId = destinationId;
        }

        /// <summary>
        /// Registers an analyzer for a specific object type. The analyzer must implement the IObjectAnalizer interface. If an analyzer for the specified object type is already registered, an ArgumentException is thrown.
        /// </summary>
        /// <typeparam name="T">The type of the analyzer to register.</typeparam>
        /// <param name="objectType">The object type for which the analyzer is being registered.</param>
        /// <exception cref="ArgumentException">Thrown if an analyzer for the specified object type is already registered.</exception>
        public void RegisterAnalizer<T>(string objectType) where T : Analizer
        {
            if (_analizers.ContainsKey(objectType.ToLower()))
            {
                throw new ArgumentException($"Object type {objectType} already registered");
            }
            _analizers.Add(objectType.ToLower(), typeof(T));
        }
        /// <summary>
        /// Retrieves the analyzer type associated with the specified object type. If no analyzer is found for the given object type, an exception is thrown.
        /// </summary>
        /// <param name="objectType">The object type for which to retrieve the analyzer type.</param>
        /// <returns>The type of the analyzer associated with the specified object type.</returns>
        /// <exception cref="Exception">Thrown if no analyzer is found for the specified object type.</exception>
        public Type GetAnalizerType(string objectType)
        {
            if (_analizers.TryGetValue(objectType.ToLower(), out var analizer))
            {
                return analizer;
            }
            throw new Exception($"Не знайдено аналізатор для типу {objectType}");
        }
        
        //public IObjectAnalizer GetObjectAnalizer(string objectType)
        //{
        //    if (_analizers.TryGetValue(objectType.ToLower(), out var analizer))
        //    {
        //        return (Activator.CreateInstance(analizer) ?? throw new Exception("Uknown error if create analizer")) as IObjectAnalizer ?? throw new Exception("The analizator not contains interface IObjectAnalizer");
        //    }
        //    throw new Exception($"Не знайдено аналізатор для типу {objectType}");
        //}
    }
}
