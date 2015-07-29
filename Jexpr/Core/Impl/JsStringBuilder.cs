using Jexpr.Common;
using Jexpr.Filters;
using Jexpr.Models;

namespace Jexpr.Core.Impl
{
    internal sealed class JsStringBuilder : IJsStringBuilder
    {
        public string BuildFrom(AbstractExpression expression)
        {
            BasicExpression basicExpression = expression as BasicExpression;

            string op = string.Empty;

            if (basicExpression != null)
            {
                op = basicExpression.Operator.ToName();
            }

            var result = GenerateJsExprByType(expression, op);

            return result;
        }

        private string GenerateJsExprByType(AbstractExpression abstractExpression, string op)
        {
            string result;

            OperationExpression operationExpression = abstractExpression as OperationExpression;

            if (operationExpression != null)
            {
                if (string.IsNullOrEmpty(op))
                {
                    result = string.Format("{0}", GenerateMacroJsExpr(operationExpression));
                }
                else
                {
                    result = operationExpression.Value != null
                  ? string.Format("({0} {1} {2})", GenerateMacroJsExpr(operationExpression), op, abstractExpression.Value)
                  : string.Format("{0}", GenerateMacroJsExpr(operationExpression));
                }

            }
            else
            {
                if (abstractExpression.Value is string)
                {
                    result = string.Format("({0} {1} '{2}')", string.Format("p.{0}", abstractExpression.Key), op, abstractExpression.Value);
                }
                else
                {
                    result = string.Format("({0} {1} {2})", string.Format("p.{0}", abstractExpression.Key), op, abstractExpression.Value);
                }
            }

            return result;
        }

        private string GenerateMacroJsExpr(OperationExpression operationExpression)
        {
            AbstractFilter abstractFilter = operationExpression.Filter;

            string parameterToChain = string.Format("p.{0}", operationExpression.Key);
            
            var result = abstractFilter.ToJs(parameterToChain);

            return result;
        }
    }
}