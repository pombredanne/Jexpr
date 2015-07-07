using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jexpr.Models;
using Jexpr.Operators;

namespace Jexpr.Core.Impl
{
    internal sealed class JsExpressionConcatService : IJsExpressionConcatService
    {
        private readonly IJsStringBuilder _jsStringBuilder;

        public JsExpressionConcatService(IJsStringBuilder jsStringBuilder)
        {
            _jsStringBuilder = jsStringBuilder;
        }


        public string ConcatJsExpressionBody(OperationOperator op, List<string> expressions)
        {
            StringBuilder result = new StringBuilder();

            switch (op)
            {
                case OperationOperator.And:
                    {
                        result.Append(string.Join(" && ", expressions));
                        break;
                    }
                case OperationOperator.Or:
                    {
                        result.Append(string.Join(" || ", expressions));
                        break;
                    }
                case OperationOperator.None:
                    {
                        result.Append(string.Join("", expressions));
                        break;
                    }
            }

            return result.ToString();
        }

        public List<string> ConcatCompiledExpressions(List<ExpressionGroup> groups)
        {
            List<string> result = new List<string>();

            foreach (ExpressionGroup expressionGroup in groups)
            {
                List<string> groupExpressionList = expressionGroup.Criteria
                    .Select(basicExpression => _jsStringBuilder.BuildFrom(basicExpression))
                    .ToList();

                switch (expressionGroup.Operator)
                {
                    case OperationOperator.And:
                        {
                            result.Add(string.Join(" && ", groupExpressionList));
                            break;
                        }
                    case OperationOperator.Or:
                        {
                            result.Add(string.Join(" || ", groupExpressionList));
                            break;
                        }
                }

                groupExpressionList.Clear();
            }

            return result;
        }

        public List<string> ConcatCompiledExpressions(List<AbstractExpression> expressions, OperationOperator operationOperator)
        {
            List<string> result = new List<string>();

            List<string> groupExpressionList = expressions
                    .Select(basicExpression => _jsStringBuilder.BuildFrom(basicExpression))
                    .ToList();

            switch (operationOperator)
            {
                case OperationOperator.And:
                    {
                        result.Add(string.Join(" && ", groupExpressionList));
                        break;
                    }
                case OperationOperator.Or:
                    {
                        result.Add(string.Join(" || ", groupExpressionList));
                        break;
                    }
            }

            groupExpressionList.Clear();


            return result;
        }
    }
}