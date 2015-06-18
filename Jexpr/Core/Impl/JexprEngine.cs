using System;
using System.Collections.Generic;
using Jexpr.Common;
using Jexpr.Models;
using Jexpr.Templates;
using Jint;
using Jint.Native;

namespace Jexpr.Core.Impl
{
    public class JexprEngine : IJexprEngine
    {
        private readonly ISerializer _serializer;
        private readonly ILogger _logger;
        private readonly Engine _jintEngine;

        public readonly string ScriptsCache;
        const string FUNC_NAME = "ExpFunc";

        private readonly Dictionary<string, string> _scriptPaths = new Dictionary<string, string>
        {
            {"lodash", "Jexpr.Scripts.lodash.min.js"}
        };


        internal JexprEngine(ISerializer serializer, IScriptLoader scriptLoader, ILogger logger)
        {
            _serializer = serializer;
            _logger = logger;

            _jintEngine = new Engine();

            ScriptsCache = scriptLoader.Load(_scriptPaths);
        }

        public JexprEngine()
            : this(new JsonNetSerializer(), new JsReferenceLoader(), new NullLogger())
        {
        }

        public EvalResult<T> Evaluate<T>(ExpressionMetadata metadata, Dictionary<string, object> paramerters = null)
        {
            EvalResult<T> result = new EvalResult<T>();

            JexprJsGeneratorTemplate template = new JexprJsGeneratorTemplate(metadata);

            string transformText = template.TransformText();
            result.Js = transformText;

            string jsContent = string.Format("{0}{1};{2}", ScriptsCache, Environment.NewLine, transformText);

            JsValue jsValue;

            try
            {
                var func = _jintEngine.Execute(jsContent);
                jsValue = paramerters != null ? func.Invoke(FUNC_NAME, _serializer.Serialize(paramerters)) : func.Invoke(FUNC_NAME);
            }
            catch (Exception exception)
            {
                _logger.Log("", exception);
                jsValue = new JsValue(false);
            }

            try
            {
                if (typeof(T).IsPrimitive || typeof(T) == typeof(string))
                {
                    result = _serializer.Deserialize<EvalResult<T>>(jsValue.AsString());
                }
                else
                {
                    result.Value = _serializer.Deserialize<T>(jsValue.AsString());
                }
            }
            catch (Exception exception)
            {
                _logger.Log("", exception);
            }

            return result;
        }
    }
}