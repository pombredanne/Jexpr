using Jexpr.Common;
using Jexpr.Filters;
using Jexpr.Models;

namespace Jexpr.Core.Impl
{
    internal sealed class JsStringBuilder : IJsStringBuilder
    {
        public string BuildFrom(JexprExpression expression)
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

        private string GenerateJsExprByType(JexprExpression jexprExpression, string op)
        {
            string result;

            MacroExpression macroExpression = jexprExpression as MacroExpression;

            if (macroExpression != null)
            {
                if (string.IsNullOrEmpty(op))
                {
                    result = string.Format("{0}", GenerateMacroJsExpr(macroExpression));
                }
                else
                {
                    result = macroExpression.Value != null
                  ? string.Format("({0} {1} {2})", GenerateMacroJsExpr(macroExpression), op, jexprExpression.Value)
                  : string.Format("{0}", GenerateMacroJsExpr(macroExpression));
                }

            }
            else
            {
                if (jexprExpression.Value is string)
                {
                    result = string.Format("({0} {1} '{2}')", string.Format("p.{0}", jexprExpression.Key), op, jexprExpression.Value);
                }
                else
                {
                    result = string.Format("({0} {1} {2})", string.Format("p.{0}", jexprExpression.Key), op, jexprExpression.Value);
                }
            }

            return result;
        }

        private string GenerateMacroJsExpr(MacroExpression macroExpression)
        {
            AbstractFilter abstractFilter = macroExpression.Filter;

            string parameterToChain = string.Format("p.{0}", macroExpression.Key);
            
            var result = abstractFilter.ToJs(parameterToChain);

            return result;
        }
    }
}