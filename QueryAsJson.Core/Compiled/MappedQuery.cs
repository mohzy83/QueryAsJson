namespace QueryAsJson.Core.Compiled
{
    /// <summary>
    /// Mapped standard query
    /// </summary>
    public class MappedQuery : MappedObject
    {
        /// <summary>
        /// Creates a query with the given statement
        /// </summary>
        /// <param name="query"></param>
        public MappedQuery(string query) : base()
        {
            Query = query;
        }
        /// <summary>
        /// Statement of the query
        /// </summary>
        public string Query { get; }
    }
}
