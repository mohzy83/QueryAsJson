using System.Collections.Generic;

namespace QueryAsJson.Core.Compiled
{
    /// <summary>
    /// Basic Mapped Object
    /// </summary>
    public class MappedObject
    {
        /// <summary>
        /// Basic Mapped Object
        /// </summary>
        public MappedObject()
        {
            MappedPropertyList = new List<MappedProperty>();
        }

        /// <summary>
        /// Properties of the MappedObject of type <see cref="MappedProperty"/>
        /// </summary>
        public List<MappedProperty> MappedPropertyList { get; private set; }
    }
}
