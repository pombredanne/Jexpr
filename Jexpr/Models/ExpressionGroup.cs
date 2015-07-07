using System.Collections.Generic;
using Jexpr.Operators;

namespace Jexpr.Models
{
    public class ExpressionGroup
    {
        public List<AbstractExpression> Criteria { get; set; }

        public OperationOperator Operator { get; set; }
    }
}