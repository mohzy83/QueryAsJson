using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using QueryAsJson.Core.Compiled;

namespace QueryAsJson.Core.Engine
{
    /// <summary>
    /// Result processor for queries with nested results
    /// </summary>
    public class NestedQueryResultProcessor : IQueryResultProcessor
    {
        /// <summary>
        /// Processes all rows of a query with nested results
        /// </summary>
        /// <param name="mappedProperty">the created child objects will be assigned to the property as array</param>
        /// <param name="mappedQuery">mapped query object</param>
        /// <param name="reader">current reader</param>
        /// <param name="jsonWriter">current json writer</param>
        /// <param name="rowFinishedAction">action will be fired when the rowset for one object (idColumn equals) was read</param>
        public void ProcessResults(MappedProperty mappedProperty, MappedQuery mappedQuery, IDataReader reader, JsonTextWriter jsonWriter, Action<IDictionary<string, object>, RowSet, List<string>> rowFinishedAction)
        {
            var arrayWriter = new ConditionalJsonArrayWriter(jsonWriter, mappedProperty);
            List<string> fieldNameList = null;
            var queryWithNestedResults = mappedQuery as MappedQueryWithNestedResults;
            var currentRowSet = new RowSet(queryWithNestedResults.IdColumn, new List<IDictionary<string, object>>());
            IDictionary<string, object> lastRow = null;
            while (reader.Read())
            {
                arrayWriter.StartArray();
                fieldNameList = fieldNameList == null ? reader.GetFieldNameList() : fieldNameList;
                var currentRow = reader.ToDictionary();
                if (lastRow != null && !Object.Equals(lastRow[queryWithNestedResults.IdColumn], currentRow[queryWithNestedResults.IdColumn]))
                {
                    rowFinishedAction(lastRow, currentRowSet, fieldNameList);
                    currentRowSet = new RowSet(queryWithNestedResults.IdColumn, new List<IDictionary<string, object>>());
                }
                currentRowSet.Rows.Add(currentRow);
                lastRow = currentRow;
            }
            if (currentRowSet.Rows.Count > 0)
                rowFinishedAction(lastRow, currentRowSet, fieldNameList);
            arrayWriter.EndArray();
        }
    }
}
