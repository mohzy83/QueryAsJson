using System;

namespace QueryAsJson.Core.Compiled
{
    /// <summary>
    /// Mapped query with nested (joined) results 
    /// </summary>
    public class MappedQueryWithNestedResults : MappedQuery
    {
        /// <summary>
        /// Creates a query with the given statement.
        /// The id column must be provided to identify identical object
        /// </summary>
        /// <param name="idColumn"></param>
        /// <param name="query"></param>
        public MappedQueryWithNestedResults(string idColumn, string query) : base(query)
        {
            if (idColumn == null) throw new ArgumentNullException("idColumn cant be null");
            IdColumn = idColumn;
        }
        /// <summary>
        /// Id Column to identify identical objects
        /// </summary>
        public string IdColumn { get; }
    }
}
