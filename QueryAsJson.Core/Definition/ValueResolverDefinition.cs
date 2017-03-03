using System;
using System.Reflection;

namespace QueryAsJson.Core.Definition
{
    /// <summary>
    /// Defintion of a value resolver
    /// </summary>
    public class ValueResolverDefinition
    {
        /// <summary>
        /// Creates of a value resolver.
        /// The value resolver will evaluate a custom value for the assigned property at runtime.
        /// The resolver must implement the interface <see cref="ICustomValueResolver"/>
        /// </summary>
        /// <param name="valueResolverType">Implementation of the <see cref="ICustomValueResolver"/> interface</param>
        public ValueResolverDefinition(Type valueResolverType)
        {
            if (!typeof(ICustomValueResolver).GetTypeInfoCompat().IsAssignableFrom(valueResolverType)) throw new ArgumentException("The valueResolverType does not implement the interface: ICustomValueResolver");
            ValueResolverType = valueResolverType;
        }
        /// <summary>
        /// Type which implements the <see cref="ICustomValueResolver"/> interface
        /// </summary>
        public Type ValueResolverType { get; }
    }
}
