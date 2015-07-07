using System;
using Jexpr.Common;
using Jexpr.Operators;
using Newtonsoft.Json;

namespace Jexpr.Filters
{
    public class ConditionFilter : AbstractFilter
    {
        public ConditionFilter(string property, ConditionOperator op, object value)
        {
            Property = property;
            Operator = op;
            Value = value;
        }

        public ConditionOperator Operator { get; private set; }
        public object Value { get; private set; }

        public override string ToJs(string parameterToChain)
        {
            string result;

            if (Operator == ConditionOperator.SubSet)
            {
                result = string.Format(@"(  
                                    (function () {{
                                            var _pFound=false;
                                            _.chain({0}).pluck('{1}').each(function (key) {{
                                                if ({2}.indexOf(key) !== -1) {{
                                                   _pFound = true;
                                                    return true;
                                                }}

                                                return false;

                                            }}).value();

                                            return _pFound;
                                        }})()
                                    )",
                    parameterToChain, Property, JsonConvert.SerializeObject(Value));
            }
            else if (Operator == ConditionOperator.Contains)
            {
                string parameter = !string.IsNullOrEmpty(parameterToChain) && (parameterToChain != "p.") ? parameterToChain : string.Format("p.{0}", Property);

                result = string.Format(@"( {0}.indexOf({1}) !== -1 )", JsonConvert.SerializeObject(Value), parameter);
            }
            else if (Operator == ConditionOperator.Mod)
            {
                result = string.Format(@"( (p.{1} % {0}) === 0 )", Value.ToString().ToLower(), Property);
            }
            else
            {
                result = string.Format(@"( {0} {1} p.{2} )", Value.ToString().ToLower(), Operator.ToName(), Property);
            }

            return result;
        }
    }
}