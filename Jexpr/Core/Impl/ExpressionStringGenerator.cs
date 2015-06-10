using Jexpr.Models;

namespace Jexpr.Core.Impl
{
    public class ExpressionStringGenerator : IExpressionStringGenerator
    {
        public string GenerateFrom(JexprExpression expression)
        {
            string result = null;

            switch (expression.Operator)
            {
                case ExpressionOp.Eq: { result = GenerateJsExprByType(expression, "==="); break; }
                case ExpressionOp.Neq: { result = GenerateJsExprByType(expression, "!=="); break; }
                case ExpressionOp.Gt: { result = GenerateJsExprByType(expression, ">"); break; }
                case ExpressionOp.Lt: { result = GenerateJsExprByType(expression, "<"); break; }
                case ExpressionOp.Gte: { result = GenerateJsExprByType(expression, ">="); break; }
                case ExpressionOp.Lte: { result = GenerateJsExprByType(expression, "<="); break; }
                case ExpressionOp.In: { result = GenerateJsExpr(expression, "includes"); break; }
            }

            return result;
        }

        private static string GenerateJsExpr(JexprExpression expression, string func)
        {
            string parameterName = string.Format("p.{0}", expression.Key);

            string result = string.Format(@"( _.chain({0})
                                         .{1}({2})
                                         .value() )",
                parameterName, func, expression.Value);

            return result;
        }

        private string GenerateJsExprByType(JexprExpression jexprExpression, string op)
        {
            string result;

            JexprMacroExpression jexprMacroExpression = jexprExpression as JexprMacroExpression;

            if (jexprMacroExpression != null)
            {
                result = jexprMacroExpression.Value != null
                    ? string.Format("({0} {1} {2})", GenerateMacroJsExpr(jexprMacroExpression), op, jexprExpression.Value)
                    : string.Format("({0})", GenerateMacroJsExpr(jexprMacroExpression));
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

        private static string GenerateMacroJsExpr(JexprMacroExpression jexprMacroExpression)
        {
            string result = null;

            MacroOpDefinition macroOpDefinition = jexprMacroExpression.MacroOp;

            string parameterToChain = string.Format("p.{0}", jexprMacroExpression.Key);

            switch (macroOpDefinition.Op)
            {
                case MacroOp.SumOfMinXItem:
                    {
                        FilterMacroOpDefinition filterMacroOpDefinition = macroOpDefinition as FilterMacroOpDefinition;

                        if (filterMacroOpDefinition != null)
                            result = string.Format(@"( _.chain({0})
                                                .pluck('{1}')
                                                .sortByOrder(false)
                                                .take({2})
                                                .sum()
                                                .value() )",
                                    parameterToChain,
                                    filterMacroOpDefinition.PropertyToVisit,
                                    filterMacroOpDefinition.Take);
                    }
                    break;
                case MacroOp.SumOfMaxXItem:
                    {
                        FilterMacroOpDefinition filterMacroOpDefinition = macroOpDefinition as FilterMacroOpDefinition;
                        if (filterMacroOpDefinition != null)
                        {
                            result = string.Format(@"( _.chain({0})
                                                .pluck('{1}')
                                                .sortByOrder()
                                                .take({2})
                                                .sum()
                                                .value() )",
                                      parameterToChain,
                                      filterMacroOpDefinition.PropertyToVisit,
                                      filterMacroOpDefinition.Take);
                        }

                        break;
                    }
                case MacroOp.Min:
                    {
                        result = string.Format(@"( _.chain({0})
                                                .pluck('{1}')
                                                .sortByOrder(false)
                                                .min()
                                                .value() )",
                              parameterToChain,
                              macroOpDefinition.PropertyToVisit);
                        break;
                    }
                case MacroOp.Max:
                    {
                        result = string.Format(@"( _.chain({0})
                                                .pluck('{1}')
                                                .sortByOrder()
                                                .max()
                                                .value() )",
                             parameterToChain,
                             macroOpDefinition.PropertyToVisit);
                        break;
                    }
                case MacroOp.In:
                    {
                        result = string.Format(@"( _.chain({0})
                                                .pluck('{1}')
                                                .includes({2})
                                                .value() )",
                            parameterToChain,
                            macroOpDefinition.PropertyToVisit, jexprMacroExpression.Value);
                        break;
                    }
            }

            return result;
        }
    }
}