using Jexpr.Common;
using Jexpr.Filters;
using Jexpr.Models;

namespace Jexpr.Core.Impl
{
    internal class JsStringBuilder : IJsStringBuilder
    {
        public string BuildFrom(JexprExpression expression)
        {
            SimpleJexprExpression simpleJexprExpression = expression as SimpleJexprExpression;

            string op = string.Empty;

            if (simpleJexprExpression != null)
            {
                op = simpleJexprExpression.Operator.ToName();
            }

            var result = GenerateJsExprByType(expression, op);

            return result;
        }

        private string GenerateJsExprByType(JexprExpression jexprExpression, string op)
        {
            string result;

            JexprMacroExpression jexprMacroExpression = jexprExpression as JexprMacroExpression;

            if (jexprMacroExpression != null)
            {
                if (string.IsNullOrEmpty(op))
                {
                    result = string.Format("{0}", GenerateMacroJsExpr(jexprMacroExpression));
                }
                else
                {
                    result = jexprMacroExpression.Value != null
                  ? string.Format("({0} {1} {2})", GenerateMacroJsExpr(jexprMacroExpression), op, jexprExpression.Value)
                  : string.Format("{0}", GenerateMacroJsExpr(jexprMacroExpression));
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

        private string GenerateMacroJsExpr(JexprMacroExpression jexprMacroExpression)
        {
            string result = null;

            JexprFilter jexprFilter = jexprMacroExpression.MacroOp;

            string parameterToChain = string.Format("p.{0}", jexprMacroExpression.Key);

            switch (jexprFilter.Op)
            {
                case MacroOp.SumOfMinXItem:
                    {
                        TakeFilter definition = jexprFilter as TakeFilter;

                        if (definition != null)
                        {
                            definition.SortByOrder = false;
                            result = definition.ToJs(parameterToChain);
                        }

                        break;
                    }

                case MacroOp.SumOfMaxXItem:
                    {
                        TakeFilter definition = jexprFilter as TakeFilter;

                        if (definition != null)
                        {
                            definition.SortByOrder = true;
                            result = definition.ToJs(parameterToChain);
                        }

                        break;
                    }
                case MacroOp.Min:
                    {
                        MinFilter definition = jexprFilter as MinFilter;

                        if (definition != null)
                        {
                            result = definition.ToJs(parameterToChain);
                        }

                        break;
                    }
                case MacroOp.Max:
                    {
                        MaxFilter definition = jexprFilter as MaxFilter;

                        if (definition != null)
                        {
                            result = definition.ToJs(parameterToChain);
                        }

                        break;
                    }
                case MacroOp.Contains:
                    {
                        if (jexprMacroExpression.HasPriority)
                        {
                            IndexOfFilter definition = new IndexOfFilter
                            {
                                ValueToLookup = jexprMacroExpression.Value,
                                Op = jexprFilter.Op,
                                PropertyToVisit = jexprFilter.PropertyToVisit
                            };

                            result = definition.ToJs(parameterToChain);
                        }
                        else
                        {
                            ExistsFilter definition = jexprFilter as ExistsFilter;

                            if (definition != null)
                            {
                                definition.ValueToSearch = jexprMacroExpression.Value;
                                result = definition.ToJs(parameterToChain);
                            }
                        }


                        break;
                    }
                case MacroOp.GroupBy:
                    {
                        GroupByFilter definition = jexprFilter as GroupByFilter;

                        if (definition != null)
                        {
                            result = definition.ToJs(parameterToChain);
                        }

                        break;
                    }
                case MacroOp.Sum:
                    {
                        SumFilter definition = jexprFilter as SumFilter;

                        if (definition != null)
                        {
                            result = definition.ToJs(parameterToChain);
                        }
                        break;
                    }

                case MacroOp.Select:
                    {
                        SelectFilter definition = jexprFilter as SelectFilter;

                        if (definition != null)
                        {
                            result = definition.ToJs(parameterToChain);
                        }
                        break;
                    }
                case MacroOp.Assign:
                    {
                        SetResultFilter definition = jexprFilter as SetResultFilter;

                        if (definition != null)
                        {
                            result = definition.ToJs(parameterToChain);
                        }
                        break;
                    }

            }

            return result;
        }
    }
}