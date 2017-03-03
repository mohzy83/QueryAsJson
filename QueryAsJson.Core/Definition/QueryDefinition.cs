using System;

namespace QueryAsJson.Core.Definition
{
    /// <summary>
    /// Defintion of a Query
    /// </summary>
    public class QueryDefinition : TemplateDefinition
    {
        /// <summary>
        /// Creates a definition of a query.
        /// Every row from the query result is used to generate JSON objects of the given template
        /// </summary>
        /// <param name="query">sql query to be executed.</param>
        /// <param name="template">template object of anonymous type is used to specify the layout of the generated JSON object</param>
        public QueryDefinition(string query, object template): base(template)
        {
            if (query == null) throw new ArgumentNullException("query cant be null");
            Query = query;
        }                

        /// <summary>
        /// sql query which will be executed to retrieve the datarows that will be mapped to json objects
        /// </summary>
        public string Query { get; }
    }
}
