using System.Collections.Generic;
using Jexpr.Operators;

namespace Jexpr.Models
{
    public class ExpressionMetadata
    {
        public List<ExpressionGroup> Items { get; set; }
        public OperationOperator Operator { get; set; }
        public List<ExpressionGroup> ResultExpression { get; set; }
    }
}