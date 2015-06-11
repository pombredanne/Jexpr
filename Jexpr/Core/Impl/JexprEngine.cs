using System;
using System.Collections.Generic;
using Jexpr.Common;
using Jexpr.Models;
using Jint;
using Jint.Native;

namespace Jexpr.Core.Impl
{
    public class JexprEngine : IJexprEngine
    {
        private readonly ISerializer _serializer;
        private readonly IExpressionBuilder _expressionBuilder;
        private readonly Engine _jintEngine;

        public readonly string ScriptsCache;

        private readonly Dictionary<string, string> _scriptPaths = new Dictionary<string, string>
        {
            {"lodash", "Jexpr.Scripts.lodash.min.js"}
        };


        internal JexprEngine(IExpressionBuilder expressionBuilder, ISerializer serializer, IScriptLoader scriptLoader)
        {
            _expressionBuilder = expressionBuilder;
            _serializer = serializer;

            _jintEngine = new Engine();
            _expressionBuilder = expressionBuilder;

            ScriptsCache = scriptLoader.Load(_scriptPaths);
        }

        public JexprEngine()
            : this(new JsExpressionBuilder(new ExpressionStringGenerator()), new JsonNetSerializer(), new ScriptLoader())
        {
        }

        public EvalResult Evaluate(ExpressionDefinition expression, Dictionary<string, object> paramerters = null)
        {
            var result = new EvalResult();

            var build = _expressionBuilder.Build(expression, paramerters);

            string jsContent = string.Format("{0}{1};{2}", ScriptsCache, Environment.NewLine, build.Body);

            JsValue jsValue;

            try
            {
                var func = _jintEngine.Execute(jsContent);
                jsValue = paramerters != null ? func.Invoke(build.FuncName, _serializer.Serialize(paramerters)) : func.Invoke(build.FuncName);
            }
            catch (Exception exception)
            {
                //TODO
                jsValue = new JsValue(false);
            }

            switch (expression.ReturnType)
            {
                case ReturnTypes.Number:
                    result.Value = jsValue.AsNumber();
                    break;
                case ReturnTypes.Boolean:
                    result.Value = jsValue.AsBoolean();
                    break;
                case ReturnTypes.String:
                    result.Value = jsValue.AsString();
                    break;
                case ReturnTypes.JsonString:
                    result.Value = _serializer.Deserialize<JsExecResult>(jsValue.AsString());
                    break;
                default:
                    result.Value = jsValue.AsObject();
                    break;
            }

            return result;
        }
    }
}