using System.Collections.Generic;
using Jexpr.Operators;

namespace Jexpr.Models
{
    public class ExpressionMetadata
    {
        public List<Jexpr.OperationExpression> Items { get; set; }
        public OperationOperator Operator { get; set; }
        public List<Jexpr.OperationExpression> ResultExpression { get; set; }
    }
}