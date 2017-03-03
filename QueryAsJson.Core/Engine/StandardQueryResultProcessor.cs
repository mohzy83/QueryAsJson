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
    /// Result processor for standard queries
    /// </summary>
    public class StandardQueryResultProcessor : IQueryResultProcessor
    {
        /// <summary>
        /// Processes all rows of a query result
        /// </summary>
        /// <param name="mappedProperty">the created child objects will be assigned to the property as array</param>
        /// <param name="mappedQuery">mapped query object</param>
        /// <param name="reader">current reader</param>
        /// <param name="jsonWriter">current json writer</param>
        /// <param name="rowFinishedAction">action will be fired when the row was read (rowSet is always null)</param>
        public void ProcessResults(MappedProperty mappedProperty, MappedQuery mappedQuery, IDataReader reader, JsonTextWriter jsonWriter, Action<IDictionary<string, object>, RowSet, List<string>> rowFinishedAction)
        {
            var arrayWriter = new ConditionalJsonArrayWriter(jsonWriter, mappedProperty);
            List<string> fieldNameList = null;
            while (reader.Read())
            {
                arrayWriter.StartArray();
                fieldNameList = fieldNameList == null ? reader.GetFieldNameList() : fieldNameList;
                rowFinishedAction(reader.ToDictionary(), null, fieldNameList);
            }
            arrayWriter.EndArray();
        }
    }
}
