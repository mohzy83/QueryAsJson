using System;

namespace QueryAsJson.Core.Compiled
{
    /// <summary>
    /// Mapping of nested results
    /// </summary>
    public class MappedNestedResults : MappedObject
    {
        /// <summary>
        /// Mapping of nested results. Identical objects can be identified by the idColumn.
        /// </summary>
        /// <param name="idColumn">Column name to identify identical objects</param>
        public MappedNestedResults(string idColumn) : base()
        {
            if (idColumn == null) throw new ArgumentNullException("idColumn cant be null");
            IdColumn = idColumn;
        }

        /// <summary>
        /// Current column name to identify identical objects
        /// </summary>
        public string IdColumn { get; }
    }
}
