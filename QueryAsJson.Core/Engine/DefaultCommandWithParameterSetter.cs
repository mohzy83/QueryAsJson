using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text.RegularExpressions;

namespace QueryAsJson.Core.Engine
{
    /// <summary>
    /// Command Parameter Setter Default implementation
    /// </summary>
    public class DefaultCommandWithParameterSetter : ICommandWithParameterSetter
    {
        const string PREFIX_LOOKUP_COLUMN = "column:";
        const string PREFIX_LOOKUP_CONTEXT = "context:";
        const string PLACEHOLDER_PATTERN = "\\$\\{.+?\\}";
        const string PARAM_PREFIX = "p";
        private IDbCommand command;
        internal List<Func<ParentObject, IDictionary<string, object>, object>> parameterExpressions = new List<Func<ParentObject, IDictionary<string, object>, object>>();
        private CommandParameterPrefix prefix;

        /// <summary>
        /// Creates a Command Parameter Setter.
        /// The placeholders ${column:_} and ${context:_} will be replaced by real command parameter like @myParameter.
        /// The parameters will be assigned during execution with parent row column values or values from the context
        /// </summary>
        /// <param name="sqlText">sql text to execute</param>
        /// <param name="connection">Connection which is used to create the command objects</param>
        /// <param name="prefix">Command paramenter prefix types</param>
        public DefaultCommandWithParameterSetter(string sqlText, IDbConnection connection, CommandParameterPrefix prefix)
        {
            this.prefix = prefix;
            command = connection.CreateCommand();
            var processedSqlText = ParseParameterExpression(sqlText, this.parameterExpressions, prefix);
            command.CommandText = processedSqlText;
            command.Prepare();
        }
        internal string ParseParameterExpression(string sqlText, List<Func<ParentObject, IDictionary<string, object>, object>> parsedParameterExpression, CommandParameterPrefix prefix)
        {
            var regExpMatcher = new Regex(PLACEHOLDER_PATTERN, RegexOptions.IgnoreCase);
            return regExpMatcher.Replace(sqlText, match => ReplaceParameter(match, parsedParameterExpression, prefix));
        }

        internal string ReplaceParameter(Match match, List<Func<ParentObject, IDictionary<string, object>, object>> parsedParameterExpression, CommandParameterPrefix prefix)
        {
            var newParameterIndex = parsedParameterExpression.Count;
            var expression = RemovePlaceholderPattern(match.Value);
            if (expression.StartsWith(PREFIX_LOOKUP_COLUMN))
                parsedParameterExpression.Add(CreateLookUpFunc(expression.Remove(0, PREFIX_LOOKUP_COLUMN.Length)));
            if (expression.StartsWith(PREFIX_LOOKUP_CONTEXT))
                parsedParameterExpression.Add(CreateContextLookUpFunc(expression.Remove(0, PREFIX_LOOKUP_CONTEXT.Length)));
            return prefix.ToRealPrefix() + PARAM_PREFIX + newParameterIndex;
        }

        internal Func<ParentObject, IDictionary<string, object>, object> CreateLookUpFunc(string columnName)
        {
            return (ParentObject parent, IDictionary<string, object> context) => parent.Values[columnName];
        }

        internal Func<ParentObject, IDictionary<string, object>, object> CreateContextLookUpFunc(string key)
        {
            return (ParentObject parent, IDictionary<string, object> context) => context[key];
        }

        internal string RemovePlaceholderPattern(string paramWithPlaceholder)
        {
            if (paramWithPlaceholder.Length > 3)
            {
                return paramWithPlaceholder.Substring(2, paramWithPlaceholder.Length - 3);
            }
            return paramWithPlaceholder;
        }
        /// <summary>
        /// Prepare the command for the current parent object and context.
        /// This will set all command parameters according to parameter setup
        /// </summary>
        /// <param name="parentObject">Current parent object</param>
        /// <param name="context">current Context</param>
        /// <returns>The prepared command with assigned command parameters</returns>
        public IDbCommand Prepare(ParentObject parentObject, IDictionary<string, object> context)
        {
            for (int i = 0; i < parameterExpressions.Count; i++)
            {
                object value = parameterExpressions[i](parentObject, context);
                CreateDbParameterAndAssignValue(command, value, prefix.ToRealPrefix() + PARAM_PREFIX + i);
            }
            return command;
        }

        internal void CreateDbParameterAndAssignValue(IDbCommand command, object value, string parameterName)
        {
            var parameterIndex = command.Parameters.IndexOf(parameterName);
            if (parameterIndex < 0)
            {
                var newParameter = command.CreateParameter();
                newParameter.ParameterName = parameterName;
                newParameter.Value = value;
                command.Parameters.Add(newParameter);
            }
            else
            {
                var existingParameter = command.Parameters[parameterIndex];
                (existingParameter as IDbDataParameter).Value = value;
            }
        }
        /// <summary>
        /// Dispose the actual command object and clear all expression
        /// </summary>
        public void Dispose()
        {
            command.Dispose();
            parameterExpressions.Clear();
        }


    }
}
