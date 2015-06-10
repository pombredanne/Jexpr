using System.Collections.Generic;
using Jexpr.Models;

namespace Jexpr
{
    public class ExpressionGroup
    {
        public List<JexprExpression> Items { get; set; }

        public ExpressionGroupOp Op { get; set; }
    }
}