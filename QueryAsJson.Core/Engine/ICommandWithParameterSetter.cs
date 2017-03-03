using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryAsJson.Core.Engine
{
    /// <summary>
    /// Interface for command with parameter setter
    /// </summary>
    public interface ICommandWithParameterSetter : IDisposable
    {
        /// <summary>
        /// Prepare the command for the current parent object and context.
        /// This will set all command parameters according to parameter setup
        /// </summary>
        /// <param name="parentObject">Current parent object</param>
        /// <param name="context">current Context</param>
        /// <returns>The prepared command with assigned command parameters</returns>
        IDbCommand Prepare(ParentObject parentObject, IDictionary<string, object> context);
    }
}
