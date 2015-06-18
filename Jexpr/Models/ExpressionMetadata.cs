using System.Collections.Generic;

namespace Jexpr.Models
{
    public class ExpressionMetadata
    {
        public ExpressionMetadata()
        {
            ReturnType = ReturnTypes.Object;
        }
        public List<ExpressionGroup> Items { get; set; }
        public ExpressionGroupOp Operator { get; set; }
        public ReturnTypes ReturnType { get; set; }
        public List<ExpressionGroup> ResultExpression { get; set; }
    }
}