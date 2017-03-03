namespace QueryAsJson.Core.Definition
{
    /// <summary>
    /// Defintion of a mapped Column
    /// </summary>
    public class ColumnDefintion
    {
        /// <summary>
        /// Created a field mapping
        /// </summary>
        /// <param name="columnName">Columnname of the column, which value will be mapped to target.
        /// Null assumes the column name equals the property name</param>
        public ColumnDefintion(string columnName)
        {
            this.ColumnName = columnName;
        }
        /// <summary>
        /// Columnname of Column in Dataset
        /// </summary>
        public string ColumnName { get; }
    }
}