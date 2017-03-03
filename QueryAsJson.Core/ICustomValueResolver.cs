using System.Collections.Generic;

namespace QueryAsJson.Core
{
    /// <summary>
    /// Interface for a custom property value resolver.
    /// This allows to call custom code during mapping execution
    /// </summary>
    public interface ICustomValueResolver
    {
        /// <summary>
        /// Calculate a custom value for the current row and context
        /// </summary>
        /// <param name="row">Current Row with all field values as as <see cref="Dictionary{TKey, TValue}"></see> where key is the columnName</param>
        /// <param name="context">Current mapping exection context as <see cref="Dictionary{TKey, TValue}"/></param>
        /// <returns>Custom value</returns>
        object ResolveValueFromRow(IDictionary<string, object> row, IDictionary<string, object> context);
    }
}
