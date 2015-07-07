using System.Collections.Generic;
using Jexpr.Models;
using Jexpr.Operators;

namespace Jexpr.Core
{
    internal interface IJsExpressionConcatService
    {
        string ConcatJsExpressionBody(OperationOperator op, List<string> expressions);
        List<string> ConcatCompiledExpressions(List<OperationExpression> groups);
        List<string> ConcatCompiledExpressions(List<AbstractExpression> expressions, OperationOperator operationOperator);
    }
}