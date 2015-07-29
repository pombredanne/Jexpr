using System.Collections.Generic;
using Jexpr.Models;
using Jexpr.Operators;

namespace Jexpr.Core
{
    internal interface IJsExpressionConcatService
    {
        string ConcatJsExpressionBody(List<string> expressions, OperationOperator op);
        List<string> ConcatCompiledExpressions(List<ExpressionGroup> groups);
        List<string> ConcatCompiledExpressions(List<AbstractExpression> expressions, OperationOperator operationOperator);
    }
}