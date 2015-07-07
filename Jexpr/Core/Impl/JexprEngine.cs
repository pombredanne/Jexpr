using System;
using System.Collections.Generic;
using Jexpr.Common;
using Jexpr.Models;
using Jexpr.Templates;
using Jint;
using Jint.Native;

namespace Jexpr.Core.Impl
{
    public sealed class JexprEngine : IJexprEngine
    {
        private readonly IInternalSerializer _internalSerializer;
        private readonly ILogger _logger;
        private Engine _jintEngine;

        public readonly string ScriptsCache;
        const string FUNC_NAME = "ExpFunc";

        private readonly Dictionary<string, string> _scriptPaths = new Dictionary<string, string>
        {
            {"lodash", "Jexpr.Scripts.lodash.min.js"}
        };


        internal JexprEngine(IInternalSerializer internalSerializer, IScriptLoader scriptLoader, ILogger logger)
        {
            _internalSerializer = internalSerializer;
            _logger = logger;
            ScriptsCache = scriptLoader.Load(_scriptPaths);
        }

        public JexprEngine()
            : this(new InternalJsonNetSerializer(), new JsReferenceLoader(), new NullLogger())
        {
        }
        public JexprResult<T> Evaluate<T>(ExpressionMetadata metadata, Dictionary<string, object> paramerters = null)
        {
            string script = Compile(metadata);

            var result = EvaluateImpl<T>(script, paramerters);
            result.Js = script;

            return result;
        }

        public JexprResult<T> Evaluate<T>(string script, Dictionary<string, object> paramerters = null)
        {
            return EvaluateImpl<T>(script, paramerters);
        }

        public JexprResult<T> Evaluate<T>(string script, dynamic paramerters = null)
        {
            return EvaluateImpl<T>(script, ((object)paramerters).ToDictionary());
        }

        public string Compile(ExpressionMetadata metadata)
        {
            JexprJsGeneratorTemplate template = new JexprJsGeneratorTemplate(metadata);
            string result = template.TransformText();

            return result;
        }

        private JexprResult<T> EvaluateImpl<T>(string script, Dictionary<string, object> paramerters)
        {
            //TODO: JINT SCRPIPT CACHE BY FUNCNAME
            _jintEngine = new Engine();
            JexprResult<T> result = new JexprResult<T>();

            string js = string.Format("{0}{1};{2}", ScriptsCache, Environment.NewLine, script);

            JsValue jsValue;

            try
            {
                var func = _jintEngine.Execute(js);
                jsValue = paramerters != null
                    ? func.Invoke(FUNC_NAME, _internalSerializer.Serialize(paramerters))
                    : func.Invoke(FUNC_NAME);
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
                    result = _internalSerializer.Deserialize<JexprResult<T>>(jsValue.AsString());
                }
                else
                {
                    result.Value = _internalSerializer.Deserialize<T>(jsValue.AsString());
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