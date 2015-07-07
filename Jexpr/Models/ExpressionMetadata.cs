using System.Collections.Generic;
using Jexpr.Operators;

namespace Jexpr.Models
{
    public class ExpressionMetadata
    {
        public List<Jexpr.ExpressionGroup> Items { get; set; }
        public OperationOperator Operator { get; set; }
        public List<Jexpr.ExpressionGroup> ResultExpression { get; set; }
    }
}