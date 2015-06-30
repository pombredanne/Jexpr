using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jexpr.Models;

namespace Jexpr.Core.Impl
{
    internal sealed class JsExpressionConcatService : IJsExpressionConcatService
    {
        private readonly IJsStringBuilder _jsStringBuilder;

        public JsExpressionConcatService(IJsStringBuilder jsStringBuilder)
        {
            _jsStringBuilder = jsStringBuilder;
        }


        public string ConcatJsExpressionBody(ExpressionGroupOp op, List<string> expressions)
        {
            StringBuilder result = new StringBuilder();

            switch (op)
            {
                case ExpressionGroupOp.And:
                    {
                        result.Append(string.Join(" && ", expressions));
                        break;
                    }
                case ExpressionGroupOp.Or:
                    {
                        result.Append(string.Join(" || ", expressions));
                        break;
                    }
                case ExpressionGroupOp.Return:
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
                List<string> groupExpressionList = expressionGroup.Items
                    .Select(basicExpression => _jsStringBuilder.BuildFrom(basicExpression))
                    .ToList();

                switch (expressionGroup.Operator)
                {
                    case ExpressionGroupOp.And:
                        {
                            result.Add(string.Join(" && ", groupExpressionList));
                            break;
                        }
                    case ExpressionGroupOp.Or:
                        {
                            result.Add(string.Join(" || ", groupExpressionList));
                            break;
                        }
                }

                groupExpressionList.Clear();
            }

            return result;
        }

        public List<string> ConcatCompiledExpressions(List<JexprExpression> expressions, ExpressionGroupOp expressionGroupOp)
        {
            List<string> result = new List<string>();

            List<string> groupExpressionList = expressions
                    .Select(basicExpression => _jsStringBuilder.BuildFrom(basicExpression))
                    .ToList();

            switch (expressionGroupOp)
            {
                case ExpressionGroupOp.And:
                    {
                        result.Add(string.Join(" && ", groupExpressionList));
                        break;
                    }
                case ExpressionGroupOp.Or:
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