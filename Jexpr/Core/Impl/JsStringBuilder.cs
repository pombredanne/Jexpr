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
            string result = null;

            AbstractFilter abstractFilter = macroExpression.MacroOperator;

            string parameterToChain = string.Format("p.{0}", macroExpression.Key);

            switch (abstractFilter.Operator)
            {
                case FilterOperator.None:
                    {
                        result = abstractFilter.ToJs(parameterToChain);

                        break;
                    }

                case FilterOperator.SumOfMinXItem:
                    {
                        TakeFilter definition = abstractFilter as TakeFilter;

                        if (definition != null)
                        {
                            definition.SortByOrder = false;
                            result = definition.ToJs(parameterToChain);
                        }

                        break;
                    }

                case FilterOperator.SumOfMaxXItem:
                    {
                        TakeFilter definition = abstractFilter as TakeFilter;

                        if (definition != null)
                        {
                            definition.SortByOrder = true;
                            result = definition.ToJs(parameterToChain);
                        }

                        break;
                    }
                case FilterOperator.Min:
                    {
                        MinFilter definition = abstractFilter as MinFilter;

                        if (definition != null)
                        {
                            result = definition.ToJs(parameterToChain);
                        }

                        break;
                    }
                case FilterOperator.Max:
                    {
                        MaxFilter definition = abstractFilter as MaxFilter;

                        if (definition != null)
                        {
                            result = definition.ToJs(parameterToChain);
                        }

                        break;
                    }
                case FilterOperator.Contains:
                    {
                        if (macroExpression.HasPriority)
                        {
                            IndexOfFilter definition = new IndexOfFilter
                            {
                                ValueToLookup = macroExpression.Value,
                                Operator = abstractFilter.Operator,
                                PropertyToVisit = abstractFilter.PropertyToVisit
                            };

                            result = definition.ToJs(parameterToChain);
                        }
                        else
                        {
                            ExistsFilter definition = abstractFilter as ExistsFilter;

                            if (definition != null)
                            {
                                definition.ValueToSearch = macroExpression.Value;
                                result = definition.ToJs(parameterToChain);
                            }
                        }


                        break;
                    }
                case FilterOperator.GroupBy:
                    {
                        GroupByFilter definition = abstractFilter as GroupByFilter;

                        if (definition != null)
                        {
                            result = definition.ToJs(parameterToChain);
                        }

                        break;
                    }
                case FilterOperator.Sum:
                    {
                        SumFilter definition = abstractFilter as SumFilter;

                        if (definition != null)
                        {
                            result = definition.ToJs(parameterToChain);
                        }
                        break;
                    }

                case FilterOperator.Select:
                    {
                        SelectFilter definition = abstractFilter as SelectFilter;

                        if (definition != null)
                        {
                            result = definition.ToJs(parameterToChain);
                        }
                        break;
                    }
                case FilterOperator.Assign:
                    {
                        AssignToResultFilter definition = abstractFilter as AssignToResultFilter;

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