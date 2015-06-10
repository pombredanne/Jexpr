using System.Collections.Generic;
using Jexpr.Models;

namespace Jexpr
{
    public class ExpressionGroup
    {
        public List<BasicExpression> ExpressionList { get; set; }

        public ExpressionGroupOp Op { get; set; }
    }
}