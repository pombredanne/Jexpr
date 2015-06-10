using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jexpr.Models;

namespace Jexpr.Core.Impl
{
    public class JsExpressionBuilder : IExpressionBuilder
    {
        private readonly IExpressionStringGenerator _expressionStringGenerator;

        public JsExpressionBuilder(IExpressionStringGenerator expressionStringGenerator)
        {
            _expressionStringGenerator = expressionStringGenerator;
        }

        const string FUNC_NAME = "ExpFunc";

        public JsExpressionResult Build(ExpressionDefinition definition, Dictionary<string, object> paramerters)
        {
            StringBuilder jsExpression = new StringBuilder();
            List<string> groupExpressions = new List<string>();

            foreach (ExpressionGroup expressionGroup in definition.ExpressionGroup)
            {
                List<string> groupExpressionList = expressionGroup.ExpressionList.Select(basicExpression => _expressionStringGenerator.GenerateFrom(basicExpression)).ToList();

                switch (expressionGroup.Op)
                {
                    case ExpressionGroupOp.And:
                        {
                            groupExpressions.Add(String.Join(" && ", groupExpressionList));
                            break;
                        }
                    case ExpressionGroupOp.Or:
                        {
                            groupExpressions.Add(String.Join(" || ", groupExpressionList));
                            break;
                        }
                }

                groupExpressionList.Clear();
            }

            switch (definition.Op)
            {
                case ExpressionGroupOp.And:
                    {
                        jsExpression.Append(string.Join(" && ", groupExpressions));
                        break;
                    }
                case ExpressionGroupOp.Or:
                    {
                        jsExpression.Append(string.Join(" || ", groupExpressions));
                        break;
                    }
            }


            string format = string.Format("function {0}(jsonParam) {{ var p = JSON.parse(jsonParam); return {1}; }}", FUNC_NAME, jsExpression);

            return new JsExpressionResult
            {
                Body = format,
                FuncName = FUNC_NAME
            };

        }
    }
}