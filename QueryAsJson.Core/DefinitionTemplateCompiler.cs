using QueryAsJson.Core.Compiled;
using QueryAsJson.Core.Definition;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace QueryAsJson.Core
{
    /// <summary>
    /// Compiles a Definition with template objects to an actual mapping which can be executed by the engine
    /// </summary>
    public static class DefinitionTemplateCompiler
    {
        /// <summary>
        /// Compiles a Definition to an actual mapping which can be executed by the engine
        /// Caompatible types are <see cref="QueryDefinition"/>, <see cref="QueryWithNestedResultsDefinition"/> and <see cref="TemplateDefinition"/>
        /// </summary>
        /// <param name="defintion">defintion to compile</param>
        /// <returns>Compiled Defintion</returns>
        public static MappedObject Compile(this TemplateDefinition defintion)
        {
            if (defintion == null) throw new ArgumentNullException("template cant be null");
            if (defintion is QueryWithNestedResultsDefinition) return CompileQueryWithNestedResults(defintion as QueryWithNestedResultsDefinition);
            if (defintion is QueryDefinition) return CompileQuery(defintion as QueryDefinition);
            return CompileTemplate(defintion, null);
        }

        internal static MappedObject CompileTemplate(TemplateDefinition templeteDefintion, QueryDefinition queryParent)
        {
            if (templeteDefintion == null) throw new ArgumentNullException("template cant be null");
            MappedObject newMappedObject = new MappedObject();
            var template = templeteDefintion.Template;
            CompileProperties(template, newMappedObject, queryParent);
            return newMappedObject;
        }

        internal static MappedQuery CompileQuery(QueryDefinition queryMapping)
        {
            if (queryMapping == null) throw new ArgumentNullException("queryMapping cant be null");
            MappedQuery newMappedQuery = new MappedQuery(queryMapping.Query);
            var template = queryMapping.Template;
            CompileProperties(template, newMappedQuery, queryMapping);
            return newMappedQuery;
        }

        internal static MappedQueryWithNestedResults CompileQueryWithNestedResults(QueryWithNestedResultsDefinition queryWithNestesResultsMapping)
        {
            if (queryWithNestesResultsMapping == null) throw new ArgumentNullException("queryWithNestesResultsMapping cant be null");
            MappedQueryWithNestedResults newMappedQueryWithNestedResults = new MappedQueryWithNestedResults(queryWithNestesResultsMapping.IdColumn, queryWithNestesResultsMapping.Query);
            var template = queryWithNestesResultsMapping.Template;
            CompileProperties(template, newMappedQueryWithNestedResults, queryWithNestesResultsMapping);
            return newMappedQueryWithNestedResults;
        }

        internal static MappedNestedResults CompileNestedResults(NestedResultsDefinition nestedResultsMapping, QueryDefinition queryParent)
        {
            if (nestedResultsMapping == null) throw new ArgumentNullException("nestedResultsMapping cant be null");
            if (!(queryParent is QueryWithNestedResultsDefinition)) throw new ArgumentException("No parent defintion of type QueryWithNestedResultsDefinition found");
            MappedNestedResults newNestedResults = new MappedNestedResults(nestedResultsMapping.IdColumn);
            var template = nestedResultsMapping.Template;
            CompileProperties(template, newNestedResults, queryParent);
            return newNestedResults;
        }

        internal static MappedObject CompileProperties(object template, MappedObject mappedObject, QueryDefinition queryParent)
        {
            if (!IsAnonymousType(template.GetType())) throw new ArgumentException("The templete object must be an anonymous type!");

            foreach (var property in template.GetType().GetTypeInfoCompat().GetProperties())
            {
                var value = property.GetValue(template, null);
                if (CheckIsValueType(value) || value.GetType() == typeof(string))
                {
                    mappedObject.MappedPropertyList.Add(new MappedProperty(property.Name, value));
                }
                else if (value is ColumnDefintion)
                {
                    mappedObject.MappedPropertyList.Add(new MappedProperty(property.Name, (value as ColumnDefintion).ColumnName));
                }
                else if (value is ValueResolverDefinition)
                {
                    mappedObject.MappedPropertyList.Add(new MappedProperty(property.Name, new MappedValueResolver((value as ValueResolverDefinition).ValueResolverType)));
                }
                else if (value is QueryWithNestedResultsDefinition)
                {
                    mappedObject.MappedPropertyList.Add(new MappedProperty(property.Name, CompileQueryWithNestedResults(value as QueryWithNestedResultsDefinition)));
                }
                else if (value is QueryDefinition)
                {
                    mappedObject.MappedPropertyList.Add(new MappedProperty(property.Name, CompileQuery(value as QueryDefinition)));
                }
                else if (value is NestedResultsDefinition)
                {
                    mappedObject.MappedPropertyList.Add(new MappedProperty(property.Name, CompileNestedResults((value as NestedResultsDefinition), queryParent)));
                }
                else if (value is TemplateDefinition)
                {
                    mappedObject.MappedPropertyList.Add(new MappedProperty(property.Name, CompileTemplate(value as TemplateDefinition, queryParent)));
                }
                else if (value != null)
                {
                    mappedObject.MappedPropertyList.Add(new MappedProperty(property.Name, CompileProperties(value, new MappedObject(), queryParent)));
                }
            }
            return mappedObject;
        }

        internal static bool CheckIsValueType(object o)
        {
            return o != null && o.GetType().GetTypeInfoCompat().IsValueType;
        }

        /// <summary>
        /// Checks that the supplied type is an anonymous type
        /// see http://www.jefclaes.be/2011/05/checking-for-anonymous-types.html
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        internal static bool IsAnonymousType(Type type)
        {
            var typeInfo = type.GetTypeInfoCompat();
            return typeInfo.IsDefined(typeof(CompilerGeneratedAttribute), false)
                       && typeInfo.IsGenericType && typeInfo.Name.Contains("AnonymousType")
                       && (typeInfo.Name.StartsWith("<>", StringComparison.OrdinalIgnoreCase) ||
                           typeInfo.Name.StartsWith("VB$", StringComparison.OrdinalIgnoreCase))
                       && (typeInfo.Attributes & TypeAttributes.NotPublic) == TypeAttributes.NotPublic;
        }
    }
}
