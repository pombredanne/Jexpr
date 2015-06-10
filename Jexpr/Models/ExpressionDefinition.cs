using System.Collections.Generic;

namespace Jexpr.Models
{
    public class ExpressionDefinition
    {
        public ExpressionDefinition()
        {
            ReturnType = ReturnTypes.Object;
        }
        public List<ExpressionGroup> Groups { get; set; }
        public ExpressionGroupOp Op { get; set; }
        public ReturnTypes ReturnType { get; set; }
        public List<ExpressionGroup> ResultExpression { get; set; }
    }
}