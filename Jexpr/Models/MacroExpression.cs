using Jexpr.Filters;

namespace Jexpr.Models
{
    public class MacroExpression : JexprExpression
    {
        public AbstractFilter Filter { get; set; }
    }
}