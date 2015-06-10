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


            var expressions = GetCompiledExpressions(definition.Groups);

            var jsExpressionBody = GetJsExpressionBody(definition.Op, expressions);

            string js;

            if (definition.ResultExpression != null && definition.ResultExpression.Any())
            {
                var resultExpressions = GetCompiledExpressions(definition.ResultExpression);

                var resultJsExpression = GetJsExpressionBody(definition.Op, resultExpressions);

                js = string.Format(@"function {0}(jsonParam) {{ 
                                             var p = JSON.parse(jsonParam); 
                                             var result = {{ Value : '', Success :false}};
                                             if({1}){{ 
                                                result = {{ Value: {2}, Success: true}};
                                             }}
                                             return JSON.stringify(result); 
                                             
                                      }}", FUNC_NAME, jsExpressionBody, resultJsExpression);
            }
            else
            {
                js = string.Format("function {0}(jsonParam) {{ var p = JSON.parse(jsonParam); return {1}; }}", FUNC_NAME, jsExpressionBody);
            }

            return new JsExpressionResult
            {
                Body = js,
                FuncName = FUNC_NAME
            };

        }

        private static string GetJsExpressionBody(ExpressionGroupOp op, List<string> expressions)
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
            }

            return result.ToString();
        }

        private List<string> GetCompiledExpressions(List<ExpressionGroup> groups)
        {
            List<string> result = new List<string>();

            foreach (ExpressionGroup expressionGroup in groups)
            {
                List<string> groupExpressionList =
                    expressionGroup.Items.Select(basicExpression => _expressionStringGenerator.GenerateFrom(basicExpression))
                        .ToList();

                switch (expressionGroup.Op)
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
    }
}