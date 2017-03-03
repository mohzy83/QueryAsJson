using System;
using System.Collections.Generic;
using System.Data;

namespace QueryAsJson.Core.Engine
{
    /// <summary>
    /// Extension methods for <see cref="IDataReader"/>
    /// </summary>
    public static class DataReaderExtensions
    {
        /// <summary>
        /// Converts the current row to a dictonary where key is the column name and value is the column value
        /// </summary>
        /// <param name="reader">current <see cref="IDataReader"/></param>
        /// <returns>row as dictionary</returns>
        public static IDictionary<string, object> ToDictionary(this IDataReader reader)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            if (reader == null) throw new ArgumentNullException("reader cant be null");
            for (int i = 0; i < reader.FieldCount; i++)
                result.Add(reader.GetName(i), reader.GetValue(i));
            return result;
        }
        /// <summary>
        /// Get all column names of the current <see cref="IDataReader"/>
        /// </summary>
        /// <param name="reader">current <see cref="IDataReader"/></param>
        /// <returns>List of column names</returns>
        public static List<string> GetFieldNameList(this IDataReader reader)
        {
            List<string> result = new List<string>();
            for (int i = 0; i < reader.FieldCount; i++)
                result.Add(reader.GetName(i));
            return result;
        }

    }
}
