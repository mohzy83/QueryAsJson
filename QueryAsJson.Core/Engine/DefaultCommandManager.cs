using QueryAsJson.Core.Compiled;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryAsJson.Core.Engine
{
    /// <summary>
    /// Creates command using a specified connection
    /// </summary>
    public class DefaultCommandManager : ICommandManager
    {
        private IDbConnection connection;
        private IDictionary<MappedQuery, ICommandWithParameterSetter> queryCommands;
        internal ICommandWithParameterSetterFactory commandSetterFactory = new DefaultCommandWithParameterSetterFactory();
        /// <summary>
        /// Creates Commands for a mapped object tree
        /// </summary>
        /// <param name="connection">the command will be created for this specific connection</param>
        public DefaultCommandManager(IDbConnection connection)
        {
            this.connection = connection;
        }
        /// <summary>
        /// Creates new Commands for all queries within the tree.
        /// The complete object graph will be processed.
        /// </summary>
        /// <param name="mappedObject">root Object to start</param>
        /// <param name="prefix">Command parameter prefix type <see cref="CommandParameterPrefix"/></param>
        public void CreateCommandsForAllMappedQueries(MappedObject mappedObject, CommandParameterPrefix prefix)
        {
            if (queryCommands != null) return;
            queryCommands = new Dictionary<MappedQuery, ICommandWithParameterSetter>();
            InternalCreateQueryNewCommands(mappedObject, prefix, queryCommands);
        }

        internal void InternalCreateQueryNewCommands(MappedObject mappedObject, CommandParameterPrefix prefix, IDictionary<MappedQuery, ICommandWithParameterSetter> commands)
        {
            if (mappedObject is MappedQuery)
            {
                var mappedQuery = mappedObject as MappedQuery;
                var queryWithNestedResults = mappedObject as MappedQueryWithNestedResults;
                var queryText = queryWithNestedResults != null ? EnforceOrderingOfQueryWithNestedResults(queryWithNestedResults.Query, queryWithNestedResults.IdColumn) : mappedQuery.Query;
                commands.Add(mappedQuery, commandSetterFactory.CreateCommand(queryText, connection, prefix));
            }
            foreach (var mappedProperty in mappedObject.MappedPropertyList)
            {
                if (mappedProperty.GetValidQuery() != null)
                    InternalCreateQueryNewCommands(mappedProperty.GetValidQuery(), prefix, commands);
                if (mappedProperty.MappedObject != null)
                    InternalCreateQueryNewCommands(mappedProperty.MappedObject, prefix, commands);
            }
        }

        /// <summary>
        /// Ensure the results are ordered by the id column
        /// </summary>
        /// <param name="query"></param>
        /// <param name="orderColumn"></param>
        /// <returns></returns>
        internal string EnforceOrderingOfQueryWithNestedResults(string query, string orderColumn)
        {
            return string.Format("SELECT * FROM ({0}) ORDER BY {1} ASC", query, orderColumn);
        }

        internal void DisposeCommands()
        {
            if (queryCommands != null)
            {
                foreach (var queryCommand in queryCommands.Values)
                {
                    queryCommand.Dispose();
                }
                queryCommands.Clear();
            }
        }
        /// <summary>
        /// Gets the prepared command for the provided mapped query
        /// </summary>
        /// <param name="query">current mapped query</param>
        /// <returns></returns>
        public ICommandWithParameterSetter GetCommandForQuery(MappedQuery query)
        {
            return queryCommands[query];
        }
        /// <summary>
        /// Dispose all command objects
        /// </summary>
        public void Dispose()
        {
            DisposeCommands();
        }
    }
}
