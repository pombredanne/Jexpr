using Jexpr.Filters;

namespace Jexpr.Models
{
    public class JexprMacroExpression : JexprExpression
    {
        public JexprFilter MacroOp { get; set; }
    }
}