using QueryAsJson.Core.Compiled;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryAsJson.Core.Engine
{
    /// <summary>
    /// Interface for Command Manager
    /// </summary>
    public interface ICommandManager : IDisposable
    {
        /// <summary>
        /// Creates new Commands for all queries within the tree
        /// The complete object graph will be processed.
        /// </summary>
        /// <param name="mappedObject">root Object to start</param>
        /// <param name="prefix">Command parameter prefix type <see cref="CommandParameterPrefix"/></param>
        void CreateCommandsForAllMappedQueries(MappedObject mappedObject, CommandParameterPrefix prefix);
        /// <summary>
        /// Gets the prepared command for the provided mapped query
        /// </summary>
        /// <param name="query">current mapped query</param>
        /// <returns></returns>
        ICommandWithParameterSetter GetCommandForQuery(MappedQuery query);
    }
}
