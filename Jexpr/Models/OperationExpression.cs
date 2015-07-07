using Jexpr.Filters;

namespace Jexpr.Models
{
    public class OperationExpression : AbstractExpression
    {
        public AbstractFilter Filter { get; set; }
    }
}