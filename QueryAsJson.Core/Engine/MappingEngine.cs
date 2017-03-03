using QueryAsJson.Core.Compiled;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Text;

namespace QueryAsJson.Core.Engine
{
    /// <summary>
    /// This will execute compiled mappings
    /// </summary>
    public class MappingEngine : IDisposable
    {
        private IDbConnection connection;
        private MappedObject root;
        internal ICommandManager commandManager;
        internal IQueryResultProcessor standardQueryResultProcessor;
        internal IQueryResultProcessor nestedQueryResultProcessor;

        private CommandParameterPrefix parameterPrefix;
        /// <summary>
        /// Command Parameter prefix which will be used for all commands
        /// </summary>
        public CommandParameterPrefix ParameterPrefix
        {
            get { return parameterPrefix; }
            set
            {
                if (parameterPrefix != value)
                {
                    commandManager.Dispose();
                    commandManager = new DefaultCommandManager(connection);
                    commandManager.CreateCommandsForAllMappedQueries(root, value);
                }
                parameterPrefix = value;
            }
        }

        /// <summary>
        /// Creates a new Mapping Engine.
        /// The connection wont be closed by the mapping engine.
        /// 
        /// </summary>
        /// <param name="connection">connection which is used to query the database</param>
        /// <param name="root">mapped object which is the root</param>
        public MappingEngine(IDbConnection connection, MappedObject root)
        {
            if (connection == null) throw new ArgumentNullException("connection cant be null");
            if (root == null) throw new ArgumentNullException("mappedObject cant be null");
            this.connection = connection;
            this.root = root;
            commandManager = new DefaultCommandManager(connection);
            standardQueryResultProcessor = new StandardQueryResultProcessor();
            nestedQueryResultProcessor = new NestedQueryResultProcessor();
        }

        /// <summary>
        /// Executes the actual mapping process.
        /// Writes the resuling json to the specified output stream.
        /// </summary>
        /// <param name="output">Generated JSON will be written to this stream</param>
        /// <param name="context">Current execution context dictionary (values can be used by the queries as command parameters)</param>
        /// <param name="autoCloseStream">true if the output stream should be closed after successful execution</param>
        /// <param name="encoding">text encoding of target json</param>
        public void ExecuteMapping(Stream output, IDictionary<string, object> context, bool autoCloseStream = true, string encoding = "UTF-8")
        {
            if (output == null) throw new ArgumentNullException("output cant be null");
            var writer = new StreamWriter(output, Encoding.GetEncoding(encoding));
            try
            {
                using (var jsonWriter = new JsonTextWriter(writer))
                {
                    jsonWriter.CloseOutput = autoCloseStream;
                    commandManager.CreateCommandsForAllMappedQueries(root, parameterPrefix);
                    if (root is MappedQuery)
                        WriteQueryResults(root as MappedQuery, jsonWriter, null, context, null);
                    else
                        WriteJsonObject(root, jsonWriter, new Dictionary<string, object>(), null, new List<string>(), null, context);
                }
            }
            finally
            {
                if (autoCloseStream) writer.Dispose();
                else writer.Flush();
            }

        }

        internal void WriteQueryResults(MappedQuery query, JsonTextWriter jsonWriter, ParentObject parentObject, IDictionary<string, object> context, MappedProperty mappedProperty)
        {
            if (query == null) throw new ArgumentNullException("query cant be null");
            if (jsonWriter == null) throw new ArgumentNullException("jsonWriter cant be null");
            var command = commandManager.GetCommandForQuery(query);
            using (var reader = command.Prepare(parentObject, context).ExecuteReader())
            {
                if (query is MappedQueryWithNestedResults)
                    nestedQueryResultProcessor.ProcessResults(mappedProperty, query, reader, jsonWriter, (c, r, l) => WriteJsonObject(query, jsonWriter, c, r, l, parentObject, context));
                else
                    standardQueryResultProcessor.ProcessResults(mappedProperty, query, reader, jsonWriter, (c, r, l) => WriteJsonObject(query, jsonWriter, c, null, l, parentObject, context));
            }
        }

        internal void WriteColumnValue(MappedProperty mappedProperty, JsonTextWriter jsonWriter, IDictionary<string, object> row, List<string> fieldNameList)
        {

            var columnName = mappedProperty.GetColumnNameOrPropertyName();
            if (columnName != null)
            {
                var matchingFieldName = fieldNameList.FirstOrDefault(fn => String.Equals(columnName, fn, StringComparison.CurrentCultureIgnoreCase));
                if (!string.IsNullOrEmpty(matchingFieldName))
                {
                    var fieldValue = row[matchingFieldName];
                    jsonWriter.WritePropertyName(mappedProperty.TargetPropertyName);
                    jsonWriter.WriteValue(fieldValue);
                }
            }
        }

        internal void WriteStaticValue(MappedProperty mappedProperty, JsonTextWriter jsonWriter)
        {
            if (mappedProperty.StaticValue != null)
            {
                jsonWriter.WritePropertyName(mappedProperty.TargetPropertyName);
                jsonWriter.WriteValue(mappedProperty.StaticValue);
            }
        }

        internal void WriteCustomResolvedValue(MappedProperty mappedProperty, JsonTextWriter jsonWriter, IDictionary<string, object> row, IDictionary<string, object> context)
        {
            if (mappedProperty.ValueResolver != null)
            {
                var resolver = mappedProperty.ValueResolver;
                jsonWriter.WritePropertyName(mappedProperty.TargetPropertyName);
                jsonWriter.WriteValue(resolver.ResolveValue(row, context));
            }
        }

        internal void WriteJsonObject(MappedObject mappedObject, JsonTextWriter jsonWriter, IDictionary<string, object> currentRow, RowSet rowSet, List<string> fieldNameList, ParentObject parent, IDictionary<string, object> context)
        {
            jsonWriter.WriteStartObject();
            foreach (var prop in mappedObject.MappedPropertyList)
            {
                if (prop.GetValidQuery() != null)
                {
                    var localParent = new ParentObject(parent, currentRow, prop);
                    WriteQueryResults(prop.GetValidQuery(), jsonWriter, localParent, context, prop);
                }
                else if (prop.MappedObject != null)
                {
                    jsonWriter.WritePropertyName(prop.TargetPropertyName);
                    WriteJsonObject(prop.MappedObject, jsonWriter, currentRow, rowSet, fieldNameList, parent, context);
                }
                else if (prop.MappedNestedResults != null)
                {

                    WriteNestedResult(prop, prop.MappedNestedResults, jsonWriter, rowSet, fieldNameList, parent, context);
                }
                else if (prop.ValueResolver != null)
                {
                    WriteCustomResolvedValue(prop, jsonWriter, currentRow, context);
                }
                else
                {
                    WriteColumnValue(prop, jsonWriter, currentRow, fieldNameList);
                    WriteStaticValue(prop, jsonWriter);
                }
            }
            jsonWriter.WriteEndObject();
        }


        internal void WriteNestedResult(MappedProperty prop, MappedNestedResults mappedGroup, JsonTextWriter jsonWriter, RowSet rowSet, List<string> fieldNameList, ParentObject parent, IDictionary<string, object> context)
        {
            HashSet<object> ids = new HashSet<object>(rowSet.Rows.Select(r => r[mappedGroup.IdColumn]).Where(v => v != null && v != System.DBNull.Value));
            if (ids.Count() > 0)
            {
                jsonWriter.WritePropertyName(prop.TargetPropertyName);
                jsonWriter.WriteStartArray();
                foreach (var currentId in ids)
                {
                    var filteredRowSet = new RowSet(mappedGroup.IdColumn, rowSet.Rows.Where(r => r[mappedGroup.IdColumn].Equals(currentId)).ToList());
                    WriteJsonObject(mappedGroup, jsonWriter, rowSet.Rows.First(r => r[mappedGroup.IdColumn].Equals(currentId)), filteredRowSet, fieldNameList, parent, context);
                }
                jsonWriter.WriteEndArray();
            }
        }
        /// <summary>
        /// Dispose all associated Commands
        /// </summary>
        public void Dispose()
        {
            commandManager.Dispose();
        }
    }
}
