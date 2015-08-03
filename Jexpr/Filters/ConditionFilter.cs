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
        public ConditionOperator Operator { get; private set; }
        public object Value { get; private set; }

        public ConditionFilter(string propertyToVisit, ConditionOperator @operator, object value)
            : base(propertyToVisit)
        {
            Operator = @operator;
            Value = value;
        }

        public override string ToJs(string parameterToChain)
        {
            string result;

            switch (Operator)
            {
                case ConditionOperator.SubSetExact:
                    {
                        result = string.Format(@"(  
                                    (function () {{
                                            var _pFound=false;
                                            _.chain({0}).each(function (key) {{
                                                if ({1}.indexOf(key) !== -1) {{
                                                   _pFound = true;
                                                    return true;
                                                }}

                                                return false;

                                            }}).value();

                                            return _pFound;
                                        }})()
                                    )",
                            parameterToChain, JsonConvert.SerializeObject(Value));

                        break;
                    }
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
                          parameterToChain, PropertyToVisit, JsonConvert.SerializeObject(Value));
                        break;
                    }
                case ConditionOperator.Contains:
                    {
                        string parameter = !string.IsNullOrEmpty(parameterToChain) && (parameterToChain != "p.")
                         ? parameterToChain
                         : string.Format("p.{0}", PropertyToVisit);

                        result = string.Format(@"( {0}.indexOf({1}) !== -1 )", JsonConvert.SerializeObject(Value), parameter);
                        break;
                    }
                case ConditionOperator.Mod:
                    {
                        if (!Value.IsNumeric())
                        {
                            throw new InvalidOperationException("Value must be number for evaluate MODULUS operation.");
                        }

                        result = string.Format(@"( (p.{1} % {0}) === 0 )", Value.ToString().ToLower(), PropertyToVisit);
                        break;
                    }

                case ConditionOperator.DistinctCount:
                    {
                        if (!Value.IsNumeric())
                        {
                            throw new InvalidOperationException("Value must be number for evaluate DistinctCount operation.");
                        }

                        result = string.Format(@"( _.chain({0}).pluck('{1}').uniq().value().length >= {2}  )", parameterToChain, PropertyToVisit, Value);

                        break;
                    }
                case ConditionOperator.Sum:
                    {
                        result = string.Format(@"( _.chain({0})
                                                .pluck('{1}')
                                                .sum()
                                                .value() )",
              parameterToChain, PropertyToVisit);
                        break;
                    }
                default:
                    {
                        string tmpValue = Value is bool ? Value.ToString().ToLower() : Value.ToString();

                        result = string.Format(@"( {0} {1} p.{2} )", tmpValue, Operator.ToName(), PropertyToVisit);
                        break;
                    }
            }

            return result;
        }
    }
}