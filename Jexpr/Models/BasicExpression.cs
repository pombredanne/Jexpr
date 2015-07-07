using Jexpr.Operators;

namespace Jexpr.Models
{
    public class BasicExpression : AbstractExpression
    {
        public ConditionOperator Operator { get; set; }
    }
}