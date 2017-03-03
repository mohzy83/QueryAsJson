using System;

namespace QueryAsJson.Core.Definition
{
    /// <summary>
    /// Defintion of a query with nested results
    /// </summary>
    public class QueryWithNestedResultsDefinition : QueryDefinition
    {
        /// <summary>
        /// Creates a definition of a query with nested results.
        /// Every row from the query result is used to generate JSON objects of the given template
        /// </summary>
        /// <param name="idColumn">column which is used to identify identical objects</param>
        /// <param name="query">sql query to be executed.</param>
        /// <param name="template">template object of anonymous type is used to specify the layout of the generated JSON object</param>
        public QueryWithNestedResultsDefinition(string idColumn, string query, object template) : base(query, template)
        {
            if (idColumn == null) throw new ArgumentNullException("idColumn cant be null");
            IdColumn = idColumn;
        }
        /// <summary>
        /// column which is used to identify identical objects
        /// </summary>
        public string IdColumn { get; }
    }
}
