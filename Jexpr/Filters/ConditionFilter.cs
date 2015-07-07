using System;
using Jexpr.Common;
using Jexpr.Operators;
using Newtonsoft.Json;

namespace Jexpr.Filters
{
    /// <summary>
    /// 
    /// </summary>
    public class ConditionFilter : AbstractFilter
    {
        private readonly ConditionOperator _operator;
        private readonly object _value;

        public ConditionFilter(string propertyToVisit, ConditionOperator @operator, object value)
        {
            _operator = @operator;
            _value = value;
            PropertyToVisit = propertyToVisit;
        }

        public override string ToJs(string parameterToChain)
        {
            string result;

            switch (_operator)
            {
                case ConditionOperator.SubSet:
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
                          parameterToChain, PropertyToVisit, JsonConvert.SerializeObject(_value));
                        break;
                    }
                case ConditionOperator.Contains:
                    {
                        string parameter = !string.IsNullOrEmpty(parameterToChain) && (parameterToChain != "p.")
                         ? parameterToChain
                         : string.Format("p.{0}", PropertyToVisit);

                        result = string.Format(@"( {0}.indexOf({1}) !== -1 )", JsonConvert.SerializeObject(_value), parameter);
                        break;
                    }
                case ConditionOperator.Mod:
                    {
                        if (!_value.IsNumeric())
                        {
                            throw new InvalidOperationException("Value must be number for evaluate MODULUS operation.");
                        }

                        result = string.Format(@"( (p.{1} % {0}) === 0 )", _value.ToString().ToLower(), PropertyToVisit);
                        break;
                    }
                default:
                    {
                        string tmpValue = _value is bool ? _value.ToString().ToLower() : _value.ToString();

                        result = string.Format(@"( {0} {1} p.{2} )", tmpValue, _operator.ToName(), PropertyToVisit);
                        break;
                    }
            }

            return result;
        }
    }
}