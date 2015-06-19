using Jexpr.Models;

namespace Jexpr.Templates
{
    /// <summary>
    /// A partial class implementation used to pass parameters to the template.
    /// </summary>
    internal partial class JexprJsGeneratorTemplate
    {
        public JexprJsGeneratorTemplate(ExpressionMetadata config)
        {
            ExpressionMetadata = config;
        }
        public ExpressionMetadata ExpressionMetadata { get; set; }
    }
}
