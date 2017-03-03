using System;

namespace QueryAsJson.Core.Definition
{
    /// <summary>
    /// This class helps to assign defintion elements to properties of an anonymous type
    /// </summary>
    public static class Define
    {
        /// <summary>
        /// This will map a column value to the target property.
        /// The name of the assigned property will be used as column name
        /// </summary>
        /// <returns>Defintion of a column</returns>
        public static ColumnDefintion Column()
        {
            return new ColumnDefintion(null);
        }
        /// <summary>
        /// Maps the value from a column with the specified columnname to the assigned property.
        /// </summary>
        /// <returns>Defintion of a column</returns>
        public static ColumnDefintion Column(string columnName)
        {
            if (columnName == null) throw new ArgumentNullException("columnName cant be null");
            return new ColumnDefintion(columnName);
        }

        /// <summary>
        /// Provides a template object as reference for the json generator
        /// </summary>
        /// <param name="template">anonymous template object</param>
        /// <returns>Defintion of a template</returns>
        public static TemplateDefinition Template(object template)
        {
            if (template == null) throw new ArgumentNullException("template cant be null");
            return new TemplateDefinition(template);
        }
        
        /// <summary>
        /// Maps a query to a list of objects whice are defined by the template (anonymous object)
        /// </summary>
        /// <param name="query">sql query</param>
        /// <param name="template">anonymous object (child elements)</param>
        /// <returns>Defintion of query</returns>
        public static QueryDefinition QueryResult(string query, object template)
        {
            if (query == null) throw new ArgumentNullException("query cant be null");
            if (template == null) throw new ArgumentNullException("template cant be null");
            return new QueryDefinition(query, template);
        }
        /// <summary>
        /// Maps a query to a list of objects whice are defined by the template (anonymous object)
        /// The query result contains nested objects.
        /// </summary>
        /// <param name="idColumn">column which idenifies, idenical objects</param>
        /// <param name="query">sql query</param>
        /// <param name="template">anonymous object (child elements)</param>
        /// <returns>Defintion of query with nested results</returns>
        public static QueryWithNestedResultsDefinition QueryWithNestedResults(string idColumn, string query, object template)
        {
            if (idColumn == null) throw new ArgumentNullException("idColumn cant be null");
            if (query == null) throw new ArgumentNullException("query cant be null");
            if (template == null) throw new ArgumentNullException("template cant be null");
            return new QueryWithNestedResultsDefinition(idColumn, query, template);
        }

        /// <summary>
        /// Maps a list objects from nested results of the current query.
        /// </summary>
        /// <param name="idColumn">The idColumn definies which columns can be used to identify idenical objects</param>
        /// <param name="template">anonymous object (child elements)</param>
        /// <returns>Definition of nested result mapping</returns>
        public static NestedResultsDefinition NestedResults(string idColumn, object template)
        {
            if (idColumn == null) throw new ArgumentNullException("idColumn cant be null");
            if (template == null) throw new ArgumentNullException("template cant be null");
            return new NestedResultsDefinition(idColumn, template);
        }

        /// <summary>
        /// Target property value will be evaluated by a custom implementiation of <see cref="ICustomValueResolver"/>
        /// </summary>
        /// <typeparam name="TValueResolver">resolver to evaluate property value</typeparam>
        /// <returns></returns>
        public static ValueResolverDefinition ValueResolver<TValueResolver>() where TValueResolver : ICustomValueResolver
        {
            return new ValueResolverDefinition(typeof(TValueResolver));
        }
    }
}
