using System.Collections.Generic;

namespace Jexpr.Models
{
    public class ExpressionDefinition
    {
        public ExpressionDefinition()
        {
            ReturnType = ReturnTypes.Object;
        }
        public List<ExpressionGroup> ExpressionGroup { get; set; }
        public ExpressionGroupOp Op { get; set; }
        public ReturnTypes ReturnType { get; set; }
    }
}