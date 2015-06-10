using Jexpr.Models;

namespace Jexpr.Core.Impl
{
    public class ExpressionStringGenerator : IExpressionStringGenerator
    {
        public string GenerateFrom(BasicExpression expression)
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

        private static string GenerateJsExpr(BasicExpression expression, string func)
        {
            string parameterName = string.Format("p.{0}", expression.Key);

            string result = string.Format(@"( _.chain({0})
                                         .{1}({2})
                                         .value() )",
                parameterName, func, expression.Value);

            return result;
        }

        private string GenerateJsExprByType(BasicExpression basicExpression, string op)
        {
            string result;

            MacroExpression macroExpression = basicExpression as MacroExpression;

            if (macroExpression != null)
            {
                result = string.Format("({0} {1} {2})", GenerateMacroJsExpr(macroExpression), op, basicExpression.Value);
            }
            else
            {
                if (basicExpression.Value is string)
                {
                    result = string.Format("({0} {1} '{2}')", string.Format("p.{0}", basicExpression.Key), op, basicExpression.Value);
                }
                else
                {
                    result = string.Format("({0} {1} {2})", string.Format("p.{0}", basicExpression.Key), op, basicExpression.Value);
                }
            }

            return result;
        }

        private static string GenerateMacroJsExpr(MacroExpression macroExpression)
        {
            string result = null;

            MacroOpDefinition macroOpDefinition = macroExpression.MacroOp;

            string parameterToChain = string.Format("p.{0}", macroExpression.Key);

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
                            macroOpDefinition.PropertyToVisit, macroExpression.Value);
                        break;
                    }
            }

            return result;
        }
    }
}