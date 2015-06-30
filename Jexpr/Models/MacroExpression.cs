using Jexpr.Filters;

namespace Jexpr.Models
{
    public class MacroExpression : JexprExpression
    {
        public AbstractFilter MacroOperator { get; set; }
    }
}