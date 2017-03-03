using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace QueryAsJson.Core.Compiled
{
    /// <summary>
    /// Mapped resolver 
    /// </summary>
    public class MappedValueResolver
    {
        /// <summary>
        /// Creates a resolver for the property value
        /// the provided type must implement the interface <see cref="ICustomValueResolver"/>
        /// </summary>
        /// <param name="valueResolverType"></param>
        public MappedValueResolver(Type valueResolverType)
        {
            ValueResolverType = valueResolverType;//.Select(t => t.FullName).ToArray();
            var resolver = Activator.CreateInstance(valueResolverType);
            ResolveValue = (resolver as ICustomValueResolver).ResolveValueFromRow;
        }

        [JsonIgnore]
        internal Func<IDictionary<string, object>, IDictionary<string, object>, object> ResolveValue { get; }

        /// <summary>
        /// Type instance will be used to evaluate the custom value
        /// </summary>
        public Type ValueResolverType { get; private set; }
    }
}
