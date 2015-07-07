using System.Collections.Generic;
using Jexpr.Models;
using Jexpr.Operators;

namespace Jexpr
{
    public class ExpressionGroup
    {
        public List<AbstractExpression> Criteria { get; set; }

        public OperationOperator Operator { get; set; }
    }
}