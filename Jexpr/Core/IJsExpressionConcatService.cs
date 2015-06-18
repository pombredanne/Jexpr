using System.Collections.Generic;
using Jexpr.Models;

namespace Jexpr.Core
{
    internal interface IJsExpressionConcatService
    {
        string ConcatJsExpressionBody(ExpressionGroupOp op, List<string> expressions);
        List<string> ConcatCompiledExpressions(List<ExpressionGroup> groups);
        List<string> ConcatCompiledExpressions(List<JexprExpression> expressions, ExpressionGroupOp expressionGroupOp);
    }
}