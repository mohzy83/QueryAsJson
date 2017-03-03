using System;

namespace QueryAsJson.Core.Definition
{
    /// <summary>
    /// Defintion with object template
    /// </summary>
    public abstract class TemplateBase
    {
        /// <summary>
        /// Create a defintion with a object template
        /// </summary>
        /// <param name="template">template object of anonymous type is used to specify the layout of the generated JSON object</param>
        public TemplateBase(object template)
        {
            if (template == null) throw new ArgumentNullException("template cant be null");
            Template = template;
        }
        /// <summary>
        /// template object of anonymous type is used to specify the layout of the generated JSON object
        /// </summary>
        public object Template { get; }
    }
}
