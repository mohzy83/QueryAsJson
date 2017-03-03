namespace QueryAsJson.Core.Definition
{
    /// <summary>
    /// Defintion of nested results of the parent query which must be of type <see cref="QueryWithNestedResultsDefinition"/>
    /// </summary>
    public class NestedResultsDefinition : TemplateBase
    {
        /// <summary>
        /// Creates a defintion of nested results
        /// </summary>
        /// <param name="idColumn">column which is used to identify identical objects</param>
        /// <param name="template">template object which will be outputed for every row in the result set</param>
        public NestedResultsDefinition(string idColumn, object template) : base(template)
        {
            IdColumn = idColumn;
        }
        /// <summary>
        /// Column which is used to identify identical objects
        /// </summary>
        public string IdColumn { get; }
    }
}
