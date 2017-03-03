using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryAsJson.Core.Engine
{
    /// <summary>
    /// Factory which creates a new ICommandWithParameterSetter implementations
    /// </summary>
    public interface ICommandWithParameterSetterFactory
    {
        /// <summary>
        /// Creates a new ICommandWithParameterSetter
        /// </summary>
        /// <param name="sqlText">sql text to execute</param>
        /// <param name="connection">Connection which is used to create the command objects</param>
        /// <param name="prefix">Command paramenter prefix types</param>
        /// <returns>new ICommandWithParameterSetter instance</returns>
        ICommandWithParameterSetter CreateCommand(string sqlText, IDbConnection connection, CommandParameterPrefix prefix);
    }
}
