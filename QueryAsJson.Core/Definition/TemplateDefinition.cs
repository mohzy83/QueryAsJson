using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryAsJson.Core.Definition
{
    /// <summary>
    /// Defintion of a basic template object
    /// </summary>
    public class TemplateDefinition : TemplateBase
    {
        /// <summary>
        /// Created a defintion of a basic template object
        /// </summary>
        /// <param name="template">template object of anonymous type is used to specify the layout of the generated JSON object</param>
        public TemplateDefinition(object template) : base(template)
        {

        }
    }
}
