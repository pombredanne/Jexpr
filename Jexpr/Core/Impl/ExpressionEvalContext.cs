using System;
using System.Collections.Generic;
using Jexpr.Common;
using Jexpr.Models;
using Jint;
using Newtonsoft.Json.Linq;

namespace Jexpr.Core.Impl
{
    public class ExpressionEvalContext : IExpressionEvalContext
    {
        private readonly ISerializer _serializer;
        private readonly IExpressionBuilder _expressionBuilder;
        private readonly Engine _jintEngine;

        public readonly string ScriptsCache;

        private readonly Dictionary<string, string> _scriptPaths = new Dictionary<string, string>
        {
            {"lodash", "Jexpr.Scripts.lodash.min.js"}
        };


        internal ExpressionEvalContext(IExpressionBuilder expressionBuilder, ISerializer serializer, IScriptLoader scriptLoader)
        {
            _expressionBuilder = expressionBuilder;
            _serializer = serializer;

            _jintEngine = new Engine();
            _expressionBuilder = expressionBuilder;

            ScriptsCache = scriptLoader.Load(_scriptPaths);
        }

        public ExpressionEvalContext()
            : this(new JsExpressionBuilder(new ExpressionStringGenerator()), new JsonNetSerializer(), new ScriptLoader())
        {
        }

        public ExpressionEvalResult Evaluate(ExpressionDefinition expression, Dictionary<string, object> paramerters = null)
        {
            var result = _expressionBuilder.Build(expression, paramerters);

            string jsContent = string.Format("{0}{1};{2}", ScriptsCache, Environment.NewLine, result.Body);

            var func = _jintEngine.Execute(jsContent);

            var arguments = _serializer.Serialize(paramerters);

            var jsValue = paramerters != null ? func.Invoke(result.FuncName, arguments) : func.Invoke(result.FuncName);

            var exprEvalResult = new ExpressionEvalResult();


            switch (expression.ReturnType)
            {
                case ReturnTypes.Number:
                    exprEvalResult.Value = jsValue.AsNumber();
                    break;
                case ReturnTypes.Boolean:
                    exprEvalResult.Value = jsValue.AsBoolean();
                    break;
                case ReturnTypes.String:
                    exprEvalResult.Value = jsValue.AsString();
                    break;
                case ReturnTypes.JsonString:
                    exprEvalResult.Value = _serializer.Deserialize<JsFuncResult>(jsValue.AsString());
                    break;
                default:
                    exprEvalResult.Value = jsValue.AsObject();
                    break;
            }

            return exprEvalResult;
        }
    }
}