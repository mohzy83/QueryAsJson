using System;
using System.Collections.Generic;

namespace QueryAsJson.Core.Engine
{
    /// <summary>
    /// Row Set - rows with identical idColumn value
    /// </summary>
    public class RowSet
    {
        /// <summary>
        /// Creates a set of rows
        /// </summary>
        /// <param name="idColumn">Column with equal values</param>
        /// <param name="rows">Rows of the row set</param>
        public RowSet(string idColumn, List<IDictionary<string, object>> rows)
        {
            if (rows == null) throw new ArgumentNullException("rows cant be null");
            IdColumn = idColumn;
            Rows = rows;
        }
        /// <summary>
        /// Rows of the row set
        /// </summary>
        public List<IDictionary<string, object>> Rows { get; }
        /// <summary>
        /// Column with equal values
        /// </summary>
        public string IdColumn { get; }
    }
}
