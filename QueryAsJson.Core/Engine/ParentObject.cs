using QueryAsJson.Core.Compiled;
using System.Collections.Generic;

namespace QueryAsJson.Core.Engine
{
    /// <summary>
    /// Parent object for the current sub query
    /// </summary>
    public class ParentObject
    {
        /// <summary>
        /// Creates a parent object which is important for sub queries
        /// </summary>
        /// <param name="parent">Parent of this object</param>
        /// <param name="values">Current row with values as dictionary</param>
        /// <param name="mappedProperty">Mapped property info</param>
        public ParentObject(ParentObject parent, IDictionary<string, object> values, MappedProperty mappedProperty)
        {
            Parent = parent;
            Values = values;
            MappedProperty = mappedProperty;
        }
        /// <summary>
        /// Parent of this object
        /// </summary>
        public ParentObject Parent { get; private set; }
        /// <summary>
        /// Current row with values as dictionary
        /// </summary>
        public IDictionary<string, object> Values { get; private set; }
        /// <summary>
        /// Mapped property info
        /// </summary>
        public MappedProperty MappedProperty { get; private set; }

    }
}
