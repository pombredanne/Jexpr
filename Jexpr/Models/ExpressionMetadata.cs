using System.Collections.Generic;

namespace Jexpr.Models
{
    public class ExpressionMetadata
    {
        public List<ExpressionGroup> Items { get; set; }
        public ExpressionGroupOp Operator { get; set; }
        public List<ExpressionGroup> ResultExpression { get; set; }
    }
}